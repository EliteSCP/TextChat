namespace TextChat.Commands.Console.Chat
{
    using System;
    using System.Collections.Generic;
    using CommandSystem;
    using Enums;
    using Exiled.API.Features;
    using Extensions;
    using Localizations;
    using static Database;
	using static TextChat;

	public class Private : Message, ICommand
	{
		public Private() : base(ChatRoomType.Private, Instance.Config.PrivateChatColor)
		{ }

		public string Description => Language.PrivateChatDescription;

		public string Usage => Language.PrivateChatUsage;

		public string Command => "private";

		public string[] Aliases => new[] { "pr" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
			Player player = Player.Get(((CommandSender)sender).SenderId);

			if (!CheckValidity(arguments.GetMessage(1), player, out response)) return false;

			response = $"[{player.Nickname}][{Language.Private}]: {response}";

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
			else if (!Instance.Config.CanSpectatorSendMessagesToAlive && player.Team == global::Team.RIP && target.Team != global::Team.RIP)
			{
				response = Language.CannotSendMessageToAlivePlayersError;
				return false;
			}

			if (Instance.Config.ShouldSaveChatToDatabase) SaveMessage(response, player.GetChatPlayer(), new List<Collections.Chat.Player>() { target.GetChatPlayer() }, type);

			Send(ref response, player, new List<Player>() { target });

			if (Instance.Config.PrivateMessageNotificationBroadcast.Show)
			{
				target?.ClearBroadcasts();
				target?.Broadcast(Instance.Config.PrivateMessageNotificationBroadcast.Duration, Instance.Config.PrivateMessageNotificationBroadcast.Content, Instance.Config.PrivateMessageNotificationBroadcast.Type);
			}

			response = $"<color={color}>{response}</color>";
			return true;
		}
	}
}
