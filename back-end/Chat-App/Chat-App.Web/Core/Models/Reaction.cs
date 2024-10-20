namespace Chat_App.Web.Core.Models;

public class Reaction
{
	public int Id { get; set; }
	public int MessageId { get; set; }
	public string ReactionType { get; set; }
	public string Username { get; set; }

	// Navigation property
	public virtual Message? Message { get; set; }
}
