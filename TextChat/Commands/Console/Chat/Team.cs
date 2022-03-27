namespace TextChat.Commands.Console.Chat
{
    using CommandSystem;
    using Enums;
    using Exiled.API.Features;
    using Localizations;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Team : ICommand
    {
        public static Team Instance { get; } = new Team();

        public string Description { get; } = Language.TeamChatDescription;

        public string Command { get; } = "t";

        public string[] Aliases { get; } = new[] { "team" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(((CommandSender)sender).SenderId);

            if (player == null)
            {
                response = Language.CommandError;
                return false;
            }

            IEnumerable<Player> targets = Player.List.Where(tempPlayer => tempPlayer != player && tempPlayer.Role.Team == player.Role.Team);

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

            message.Content = response = $"[{player.Nickname}][{Language.Team} ({player.Role.ToString().ToUpper()})]: {response}";

            if (TextChat.Instance.Config.ShouldSaveChatToDatabase)
                message.Save(ChatRoomType.Team);

            message.Send(targets, player.GetColor());

            response = $"<color={player.GetColor()}>{response}</color>";
            return true;
        }
    }
}
