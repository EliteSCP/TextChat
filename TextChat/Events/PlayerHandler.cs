namespace TextChat.Events
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Localizations;
    using System;
    using System.Linq;
    using static Database;

    internal class PlayerHandler
    {
        public void OnVerified(VerifiedEventArgs ev)
        {
            ChatPlayersCache.Add(ev.Player, ev.Player.GetChatPlayer() ?? new Collections.Chat.Player(
                ev.Player.RawUserId,
                ev.Player.AuthenticationType.ToString().ToLower(),
                ev.Player.Nickname,
                DateTime.Now));

            ev.Player.SendConsoleMessage(Language.ChatWelcome, "green");

            Player.List.Where(player => player != ev.Player).SendConsoleMessage(string.Format(Language.PlayerHasJoinedTheChat, ev.Player.Nickname), "green");
        }

        public void OnDestroying(DestroyingEventArgs ev)
        {
            Player.List.Where(player => player != ev.Player).SendConsoleMessage(string.Format(Language.PlayerHasLeftTheChat, ev.Player.Nickname), "red");

            ev.Player.GetChatPlayer().Save();

            ChatPlayersCache.Remove(ev.Player);
        }
    }
}
