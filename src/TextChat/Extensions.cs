namespace TextChat
{
    using Exiled.API.Features;
    using PlayerRoles;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using static Database;

    public static class Extensions
    {
        public static void SendConsoleMessage(this IEnumerable<Player> targets, string message, string color)
        {
            foreach (Player target in targets.Where(target => target != null))
                target.SendConsoleMessage(message, color);
        }

        public static string GetColor(this Player player) => player.Role.Team.GetColor();

        public static string GetColor(this Team team)
        {
            switch (team)
            {
                case Team.SCPs:
                    return "red";
                case Team.FoundationForces:
                    return "blue";
                case Team.ChaosInsurgency:
                    return "green";
                case Team.Scientists:
                    return "yellow";
                case Team.ClassD:
                    return "yellow";
                case Team.OtherAlive:
                    return "green";
                case Team.Dead:
                default:
                    return "white";
            }
        }

        public static bool IsChatMuted(this Player player)
        {
            return LiteDatabase.GetCollection<Collections.Chat.Mute>().Exists(mute => mute.Target.Id == player.UserId && mute.Expire > DateTime.Now);
        }

        public static Collections.Chat.Player GetChatPlayer(this string player)
        {
            return Player.Get(player)?.GetChatPlayer() ??
                LiteDatabase.GetCollection<Collections.Chat.Player>().Query().Where(queryPlayer => queryPlayer.Id == player.GetRawUserId() || queryPlayer.Name == player).FirstOrDefault();
        }

        public static Collections.Chat.Player GetChatPlayer(this Player player)
        {
            if (player == null || (string.IsNullOrEmpty(player.UserId) && !player.IsHost))
                return null;
            else if (player.IsHost)
                return ServerChatPlayer;
            else if (ChatPlayersCache.TryGetValue(player, out Collections.Chat.Player chatPlayer))
                return chatPlayer;
            else
                return LiteDatabase.GetCollection<Collections.Chat.Player>().FindById(player.RawUserId);
        }

        public static Collections.Chat.Player GetStaffer(this CommandSender sender) => new Collections.Chat.Player
        (
            sender?.SenderId?.GetRawUserId() ?? "Server",
            sender?.SenderId?.GetAuthentication() ?? "Server",
            sender?.Nickname ?? "Server",
            DateTime.MinValue
        );

        public static IEnumerable<Collections.Chat.Player> GetChatPlayers(this IEnumerable<Player> players)
        {
            return players.Select(player => player?.GetChatPlayer()).Where(player => player != null);
        }
        public static string GetAuthentication(this string userId) => userId.Substring(userId.LastIndexOf('@') + 1);

        public static string GetRawUserId(this string userId)
        {
            int index = userId.LastIndexOf('@');

            if (index == -1)
                return userId;

            return userId.Substring(0, index);
        }

        public static string Sanitize(this string stringToSanitize, IEnumerable<string> badWords, char badWordsChar)
        {
            foreach (string badWord in badWords)
                stringToSanitize = Regex.Replace(stringToSanitize, badWord, new string(badWordsChar, badWord.Length), RegexOptions.IgnoreCase);

            return stringToSanitize;
        }

        public static string GetMessage(this ArraySegment<string> args, int skips = 0, string separator = " ")
        {
            return string.Join(separator, skips == 0 ? args : args.Skip(skips).Take(args.Count - skips));
        }
    }
}
