namespace TextChat.Commands.Console.Chat
{
    using CommandSystem;
    using Enums;
    using Exiled.API.Features;
    using Localizations;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Public : ICommand
    {
        private Public()
        {
        }

        public static Public Instance { get; } = new Public();

        public string Description { get; } = Language.PublicChatDescription;

        public string Usage { get; } = Language.PublicChatUsage;

        public string Command { get; } = "public";

        public string[] Aliases { get; } = new[] { "p" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(((CommandSender)sender).SenderId);

            if (player == null)
            {
                response = Language.CommandError;
                return false;
            }

            IEnumerable<Player> targets = Player.List.Where(target =>
            {
                return player != target && (TextChat.Instance.Config.CanSpectatorSendMessagesToAlive || !Round.IsStarted || (!TextChat.Instance.Config.CanSpectatorSendMessagesToAlive && (player.IsAlive || target.IsDead)));
            });

            Collections.Chat.Message message = new Collections.Chat.Message(player.GetChatPlayer(), targets.GetChatPlayers().ToList(), arguments.GetMessage(), DateTime.Now);

            if (message.Sender == null)
            {
                response = Language.CommandError;
                return false;
            }

            if (!message.IsValid(out response))
                return false;

            if (message.Targets.Count == 0)
            {
                response = Language.NoAvailablePlayersToChatWithError;
                return false;
            }

            message.Content = response = $"[{player.Nickname}][{Language.Public}]: {response}";

            if (TextChat.Instance.Config.ShouldSaveChatToDatabase)
                message.Save(ChatRoomType.Team);

            message.Send(targets, TextChat.Instance.Config.PublicChatColor);

            response = $"<color={TextChat.Instance.Config.PublicChatColor}>{response}</color>";
            return true;
        }
    }
}
