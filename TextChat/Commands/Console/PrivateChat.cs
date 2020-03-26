using EXILED.Extensions;
using System.Collections.Generic;
using TextChat.Enums;
using TextChat.Extensions;
using TextChat.Interfaces;
using static TextChat.Database;

namespace TextChat.Commands.Console
{
	public class PrivateChat : Chat, ICommand
	{
		public PrivateChat() : base(ChatRoomType.Private, Configs.privateMessageColor)
		{ }

		public string Description => "Sends a private chat message to a player.";

		public string Usage => ".chat_private [Nickname/UserID/PlayerID] [Message]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			(string message, bool isValid) = CheckMessageValidity(args.GetMessage(1), ChatPlayers[sender], sender);

			if (!isValid) return (message, "red");

			message = $"[{sender.GetNickname()}][PRIVATE]: {message}";

			ReferenceHub target = Player.GetPlayer(args[0]);

			if (target == null) return ($"Player {target.GetNickname()} was not found!", "red");
			else if (sender == target) return ("You cannot send a message to yourself!", "red");
			else if (!Configs.canSpectatorSendMessagesToAlive && sender.GetTeam() == Team.RIP && target.GetTeam() != Team.RIP)
			{
				return ("You cannot send messages to alive players!", "red");
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
