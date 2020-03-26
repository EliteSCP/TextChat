using static TextChat.Database;

namespace TextChat.Events
{
	public class RoundHandler
	{
		public void OnRoundRestart() => LiteDatabase.Checkpoint();
	}
}
