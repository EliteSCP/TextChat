using static TextChat.TextChat;

namespace TextChat.Events
{
	public class RoundHandler
	{
		private readonly TextChat pluginInstance;

		public RoundHandler(TextChat pluginInstance) => this.pluginInstance = pluginInstance;

		public void OnWaitingForPlayers() => pluginInstance.LoadConfigs();

		public void OnRoundRestart() => Database.Dispose();
	}
}
