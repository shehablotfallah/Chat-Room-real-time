using Microsoft.Extensions.Options;
using PusherServer;

namespace Chat_App.Web.Services;

public class PusherService : IPusherService
{
	private readonly Pusher _pusher;

	public PusherService(IOptions<PusherSettings> options)
	{
		var settings = options.Value;
		_pusher = new Pusher(
			settings.AppId,
			settings.Key,
			settings.Secret,
			new PusherOptions
			{
				Cluster = settings.Cluster,
				Encrypted = true
			});
	}

	public async Task TriggerAsync(string channel, string eventName, object data)
	{
		await _pusher.TriggerAsync(channel, eventName, data);
	}
}
