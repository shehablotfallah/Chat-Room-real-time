namespace Chat_App.Web.Services;

public class MessageService : IMessageService
{
	private readonly ApplicationDbContext _context;

	public MessageService(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Message>> GetMessagesAsync()
	{
		return await _context.Messages
							 .Include(m => m.Reactions)
							 .ToListAsync();
	}

	public async Task<Message> SendMessageAsync(MessageDto messageDto)
	{
		var message = new Message
		{
			Username = messageDto.Username,
			Text = messageDto.Message,
			Timestamp = DateTime.Now.ToString("hh:mm tt")
		};

		_context.Messages.Add(message);
		await _context.SaveChangesAsync();

		return message;
	}

	public async Task<bool> UpdateMessageAsync(MessageUpdateDto messageUpdateDto)
	{
		var message = await _context.Messages.FindAsync(messageUpdateDto.Id);
		if (message == null)
			return false;

		message.Text = messageUpdateDto.Message;
		_context.Messages.Update(message);
		await _context.SaveChangesAsync();

		return true;
	}

	public async Task<bool> DeleteMessageAsync(int id)
	{
		var message = await _context.Messages.FindAsync(id);
		if (message == null)
			return false;

		_context.Messages.Remove(message);
		await _context.SaveChangesAsync();

		return true;
	}

	public async Task<bool> AddReactionAsync(MessageReactionDto reactionDto)
	{
		var message = await _context.Messages.FindAsync(reactionDto.MessageId);
		if (message == null)
			return false;

		var reaction = new Reaction
		{
			MessageId = reactionDto.MessageId,
			ReactionType = reactionDto.Reaction,
			Username = reactionDto.Username
		};

		_context.Reactions.Add(reaction);
		await _context.SaveChangesAsync();

		return true;
	}

	public async Task<bool> RemoveReactionAsync(MessageReactionDto reactionDto)
	{
		var reaction = await _context.Reactions
								   .FirstOrDefaultAsync(r => r.MessageId == reactionDto.MessageId &&
														   r.ReactionType == reactionDto.Reaction &&
														   r.Username == reactionDto.Username);

		if (reaction == null)
			return false;

		_context.Reactions.Remove(reaction);
		await _context.SaveChangesAsync();

		return true;
	}
}
