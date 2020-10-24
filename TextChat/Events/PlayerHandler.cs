namespace TextChat.Events
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Extensions;
    using Localizations;
    using System.Linq;
    using static Database;

	internal class PlayerHandler
	{
		public void OnJoined(JoinedEventArgs ev)
		{
			ChatPlayers.Add(ev.Player, ev.Player.GetChatPlayer() ?? new Collections.Chat.Player()
			{
				Id = ev.Player.RawUserId,
				Authentication = ev.Player.AuthenticationType.ToString().ToLower(),
				Name = ev.Player.Nickname
			});

			ev.Player.SendConsoleMessage(Language.ChatWelcome, "green");

			Player.List.Where(player => player != ev.Player).SendConsoleMessage(string.Format(Language.PlayerHasJoinedTheChat, ev.Player.Nickname), "green");
		}

		public void OnLeft(LeftEventArgs ev)
		{
			Player.List.Where(player => player != ev.Player).SendConsoleMessage(string.Format(Language.PlayerHasLeftTheChat, ev.Player.Nickname), "red");

			LiteDatabase.GetCollection<Collections.Chat.Player>().Upsert(ev.Player.GetChatPlayer());

			ChatPlayers.Remove(ev.Player);
		}
	}
}
