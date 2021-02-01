namespace TextChat.Commands.Console.Chat
{
    using CommandSystem;
    using Enums;
    using Exiled.API.Features;
    using Extensions;
    using Localizations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Database;
    using static TextChat;

    public class Team : Message, ICommand
	{
		public Team() : base(ChatRoomType.Team)
		{ }

		public string Description { get; } = Language.TeamChatDescription;

        public string Command { get; } = "t";

		public string[] Aliases { get; } = new[] { "team" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
			Player player = Player.Get(((CommandSender)sender).SenderId);

			if (!CheckValidity(arguments.GetMessage(), player, out response)) return false;

			response = $"[{player.Nickname}][{Language.Team} ({player.Role.ToString().ToUpper()})]: {response}";

			IEnumerable<Player> targets = Player.List.Where(chatPlayer => chatPlayer != player && chatPlayer.Team == player.Team);
			List<Collections.Chat.Player> chatTargets = targets.GetChatPlayers().ToList();

			if (chatTargets.Count == 0)
			{
				response = Language.NoAvailablePlayersToChatWithError;
				return false;
			}

			color = player.GetColor();

			if (Instance.Config.ShouldSaveChatToDatabase) SaveMessage(response, player.GetChatPlayer(), chatTargets, type);

			Send(ref response, player, targets);

			response = $"<color={player?.Team.GetColor()}>{response}</color>";
			return true;
		}
	}
}
