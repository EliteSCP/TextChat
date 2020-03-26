using static TextChat.Database;

namespace TextChat.Events
{
	public class RoundHandler
	{
		private readonly TextChat pluginInstance;

		public RoundHandler(TextChat pluginInstance) => this.pluginInstance = pluginInstance;

		public void OnWaitingForPlayers() => Configs.Reload();

		public void OnRoundRestart() => LiteDatabase.Dispose();
	}
}
