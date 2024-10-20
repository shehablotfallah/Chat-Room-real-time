using Microsoft.AspNetCore.Mvc;

namespace Chat_App.Web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ChatController : ControllerBase
	{
		private readonly IMessageService _messageService;
		private readonly IPusherService _pusherService;
		private readonly ILogger<ChatController> _logger;

		public ChatController(IMessageService messageService, IPusherService pusherService, ILogger<ChatController> logger)
		{
			_messageService = messageService;
			_pusherService = pusherService;
			_logger = logger;
		}

		// Send a new message
		[HttpPost("send-message")]
		public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
		{
			if (string.IsNullOrWhiteSpace(messageDto.Username) || string.IsNullOrWhiteSpace(messageDto.Message))
				return BadRequest("Username and message cannot be empty.");

			var message = await _messageService.SendMessageAsync(messageDto);

			await _pusherService.TriggerAsync("chat-room", "new-message", new
			{
				id = message.Id,
				username = message.Username,
				text = message.Text,
				timestamp = DateTime.Now.ToString("hh:mm tt")
			});

			return Ok(new
			{
				id = message.Id,
				username = message.Username,
				text = message.Text,
				timestamp = DateTime.Now.ToString("hh:mm tt")
			});
		}

		// Edit an existing message
		[HttpPut("update-message")]
		public async Task<IActionResult> UpdateMessage([FromBody] MessageUpdateDto messageUpdateDto)
		{
			if (string.IsNullOrWhiteSpace(messageUpdateDto.Message))
				return BadRequest("Message cannot be empty.");

			var updated = await _messageService.UpdateMessageAsync(messageUpdateDto);

			if (updated)
			{
				await _pusherService.TriggerAsync("chat-room", "message-updated", new
				{
					messageId = messageUpdateDto.Id,
					text = messageUpdateDto.Message
				});
				return Ok();
			}

			return NotFound("Message not found.");
		}

		// Delete a message
		[HttpDelete("delete-message/{id}")]
		public async Task<IActionResult> DeleteMessage(int id)
		{
			var deleted = await _messageService.DeleteMessageAsync(id);

			if (deleted)
			{
				await _pusherService.TriggerAsync("chat-room", "message-deleted", new { messageId = id });
				return NoContent();
			}

			return NotFound("Message not found.");
		}

		// React to a message
		[HttpPost("react-message")]
		public async Task<IActionResult> ReactToMessage([FromBody] MessageReactionDto reactionDto)
		{
			if (reactionDto.MessageId <= 0 || string.IsNullOrWhiteSpace(reactionDto.Reaction))
				return BadRequest("Invalid reaction or message ID.");

			var added = await _messageService.AddReactionAsync(reactionDto);
			if (!added)
				return NotFound("Message not found.");

			await _pusherService.TriggerAsync("chat-room", "message-reacted", new
			{
				messageId = reactionDto.MessageId,
				reaction = reactionDto.Reaction,
				numOfReactions = 1,
				username = reactionDto.Username
			});

			return Ok();
		}

		//remove reaction from a message
		[HttpPost("remove-reaction")]
		public async Task<IActionResult> RemoveReaction([FromBody] MessageReactionDto reactionDto)
		{
			if (reactionDto.MessageId <= 0 || string.IsNullOrWhiteSpace(reactionDto.Reaction))
				return BadRequest("Invalid reaction or message ID.");

			var removed = await _messageService.RemoveReactionAsync(reactionDto);
			if (!removed)
				return NotFound("Message not found.");

			await _pusherService.TriggerAsync("chat-room", "message-reaction-removed", new
			{
				messageId = reactionDto.MessageId,
				reaction = reactionDto.Reaction,
				numOfReactions = -1,
				username = reactionDto.Username
			});

			return Ok();
		}

		// Notify when a user starts typing
		[HttpPost("start-typing")]
		public async Task<IActionResult> StartTyping([FromBody] TypingEventDto typingEvent)
		{
			await _pusherService.TriggerAsync("chat-room", "user-typing", new
			{ username = typingEvent.Username });
			return Ok();
		}

		// Notify when a user stops typing
		[HttpPost("stop-typing")]
		public async Task<IActionResult> StopTyping([FromBody] TypingEventDto typingEvent)
		{
			await _pusherService.TriggerAsync("chat-room", "user-stop-typing", new
			{ username = typingEvent.Username });
			return Ok();
		}

		// Get all messages (for initial load)
		[HttpGet("get-messages")]
		public async Task<IActionResult> GetMessages()
		{
			var messages = await _messageService.GetMessagesAsync();
			return Ok(messages);
		}
	}
}
