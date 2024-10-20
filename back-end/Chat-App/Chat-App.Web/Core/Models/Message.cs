namespace Chat_App.Web.Core.Models;

public class Message
{
	public int Id { get; set; }
	public string Username { get; set; }
	public string Text { get; set; }
	public string Timestamp { get; set; }


	public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
}
