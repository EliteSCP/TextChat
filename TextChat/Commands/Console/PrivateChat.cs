using EXILED.Extensions;
using System.Collections.Generic;
using TextChat.Enums;
using TextChat.Extensions;
using TextChat.Interfaces;
using TextChat.Localizations;
using static TextChat.Database;

namespace TextChat.Commands.Console
{
	public class PrivateChat : Chat, ICommand
	{
		public PrivateChat() : base(ChatRoomType.Private, Configs.privateMessageColor)
		{ }

		public string Description => Language.PrivateChatDescription;

		public string Usage => Language.PrivateChatUsage;

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			(string message, bool isValid) = CheckMessageValidity(args.GetMessage(1), ChatPlayers[sender], sender);

			if (!isValid) return (message, "red");

			message = $"[{sender.GetNickname()}][{Language.Private}]: {message}";

			ReferenceHub target = Player.GetPlayer(args[0]);

			if (target == null) return (string.Format(Language.PlayerNotFoundError, args[0]), "red");
			else if (sender == target) return (Language.CannotSendMessageToThemselvesError, "red");
			else if (!Configs.canSpectatorSendMessagesToAlive && sender.GetTeam() == Team.RIP && target.GetTeam() != Team.RIP)
			{
				return (Language.CannotSendMessageToAlivePlayersError, "red");
			}

			if (Configs.saveChatToDatabase) SaveMessage(message, ChatPlayers[sender], new List<Collections.Chat.Player>() { ChatPlayers[sender] }, type);

			SendMessage(ref message, ChatPlayers[sender], new List<ReferenceHub>() { target });

			if (Configs.showPrivateMessageNotificationBroadcast)
			{
				target.ClearBroadcasts();
				target.Broadcast(Configs.privateMessageNotificationBroadcastDuration, Configs.privateMessageNotificationBroadcast, false);
			}

			return (message, color);
		}
	}
}
