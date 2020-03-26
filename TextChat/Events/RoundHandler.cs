using static TextChat.Database;

namespace TextChat.Events
{
	public class RoundHandler
	{
		public void OnWaitingForPlayers() => Configs.Reload();

		public void OnRoundRestart() => LiteDatabase.Dispose();
	}
}
