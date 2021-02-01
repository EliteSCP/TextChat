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

    public class Public : Message, ICommand
	{
		public Public() : base(ChatRoomType.Public, Instance.Config.PublicChatColor)
		{ }

		public string Description { get; } = Language.PublicChatDescription;

		public string Usage { get; } = Language.PublicChatUsage;

		public string Command { get; } = "public";

		public string[] Aliases { get; } = new[] { "p" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
			Player player = Player.Get(((CommandSender)sender).SenderId);

			if (!CheckValidity(arguments.GetMessage(), player, out response)) return false;

			response = $"[{player.Nickname}][{Language.Public}]: {response}";

			IEnumerable<Player> targets = Player.List.Where(target =>
			{
				return player != target && (Instance.Config.CanSpectatorSendMessagesToAlive || player.Team != global::Team.RIP || target.Team == global::Team.RIP);
			});

			List<Collections.Chat.Player> chatPlayers = targets.GetChatPlayers().ToList();

			if (chatPlayers.Count == 0)
			{
				response = Language.NoAvailablePlayersToChatWithError;
				return false;
			}

			if (Instance.Config.ShouldSaveChatToDatabase) SaveMessage(response, player.GetChatPlayer(), chatPlayers, type);

			Send(ref response, player, targets);

			response = $"<color={color}>{response}</color>";
			return true;
		}
	}
}
