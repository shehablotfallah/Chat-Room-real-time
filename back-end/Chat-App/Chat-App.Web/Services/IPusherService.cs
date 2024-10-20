namespace Chat_App.Web.Services;

public interface IPusherService
{
	Task TriggerAsync(string channel, string eventName, object data);
}
