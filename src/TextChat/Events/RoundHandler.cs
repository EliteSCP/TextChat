namespace TextChat.Events
{
    using static Database;

    internal class RoundHandler
    {
        public void OnRestartingRound() => LiteDatabase.Checkpoint();
    }
}
