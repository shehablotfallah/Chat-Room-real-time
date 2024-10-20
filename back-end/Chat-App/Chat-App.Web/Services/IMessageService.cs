namespace Chat_App.Web.Services;

public interface IMessageService
{
	Task<IEnumerable<Message>> GetMessagesAsync();
	Task<Message> SendMessageAsync(MessageDto messageDto);
	Task<bool> UpdateMessageAsync(MessageUpdateDto messageUpdateDto);
	Task<bool> DeleteMessageAsync(int id);
	Task<bool> AddReactionAsync(MessageReactionDto reactionDto);
	Task<bool> RemoveReactionAsync(MessageReactionDto reactionDto);
}
