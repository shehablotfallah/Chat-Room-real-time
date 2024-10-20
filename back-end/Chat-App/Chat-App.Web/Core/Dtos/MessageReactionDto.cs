namespace Chat_App.Web.Core.Dtos;

public class MessageReactionDto
{
	public int MessageId { get; set; }
	public string Reaction { get; set; }
	public string Username { get; set; }
}
