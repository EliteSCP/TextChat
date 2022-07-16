namespace TextChat.Commands.Console.Chat
{
    using CommandSystem;
    using Enums;
    using Exiled.API.Features;
    using Localizations;
    using System;

    public class Private : ICommand
    {
        private Private()
        {
        }

        public static Private Instance { get; } = new Private();

        public string Description { get; } = Language.PrivateChatDescription;

        public string Usage { get; } = Language.PrivateChatUsage;

        public string Command { get; } = "private";

        public string[] Aliases { get; } = new[] { "pr" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(((CommandSender)sender).SenderId);

            if (player == null)
            {
                response = Language.CommandError;
                return false;
            }

            Player target = Player.Get(arguments.At(0));

            if (target == null)
            {
                response = string.Format(Language.PlayerNotFoundError, arguments.At(0));
                return false;
            }
            else if (player == target)
            {
                response = Language.CannotSendMessageToThemselvesError;
                return false;
            }
            else if (!TextChat.Instance.Config.CanSpectatorSendMessagesToAlive && player.IsDead && target.IsAlive)
            {
                response = Language.CannotSendMessageToAlivePlayersError;
                return false;
            }

            Collections.Chat.Message message = new Collections.Chat.Message(player.GetChatPlayer(), target.GetChatPlayer(), arguments.GetMessage(1), DateTime.Now);

            if (!message.IsValid(out response))
                return false;

            message.Content = response = $"[{player.Nickname}][{Language.Private}]: {response}";

            if (TextChat.Instance.Config.ShouldSaveChatToDatabase)
                message.Save(ChatRoomType.Private);

            message.Send(target, TextChat.Instance.Config.PrivateChatColor);

            if (TextChat.Instance.Config.PrivateMessageNotificationBroadcast.Show)
            {
                target?.ClearBroadcasts();
                target?.Broadcast(TextChat.Instance.Config.PrivateMessageNotificationBroadcast.Duration, TextChat.Instance.Config.PrivateMessageNotificationBroadcast.Content, TextChat.Instance.Config.PrivateMessageNotificationBroadcast.Type);
            }

            response = $"<color={TextChat.Instance.Config.PrivateChatColor}>{response}</color>";
            return true;
        }
    }
}
