using EXILED.Extensions;
using System.Collections.Generic;
using System.Linq;
using TextChat.Enums;
using TextChat.Extensions;
using TextChat.Interfaces;

namespace TextChat.Commands.Console
{
	public class PrivateChat : Chat, ICommand
	{
		public PrivateChat(TextChat pluginInstance) : base(pluginInstance, ChatRoomType.Private, pluginInstance.privateMessageColor)
		{ }

		public string Description => "Sends a private chat message to a player.";

		public string Usage => ".chat_private [Nickname/UserID/PlayerID] [Message]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			(string message, bool isValid) = CheckMessageValidity(args.GetMessage(1), sender);

			if (!isValid) return (message, "red");

			message = $"[{sender.GetNickname()}][PRIVATE]: {message}";

			ReferenceHub target = Player.GetPlayer(args[0]);

			if (target == null) return ($"Player {target.GetNickname()} was not found!", "red");
			else if (sender == target) return ("You cannot send a message to yourself!", "red");
			else if (!pluginInstance.canSpectatorSendMessagesToAlive && sender.GetTeam() == Team.RIP && target.GetTeam() != Team.RIP)
			{
				return ("You cannot send messages to alive players!", "red");
			}

			SendMessage(message, sender, new List<ReferenceHub>() { target });

			if (pluginInstance.saveChatToDatabase) SaveMessage(message, pluginInstance.ChatPlayers[sender], new List<Collections.Chat.Player>() { pluginInstance.ChatPlayers[sender] });

			if (pluginInstance.showPrivateMessageNotificationBroadcast)
			{
				target.ClearBroadcasts();
				target.Broadcast(pluginInstance.privateMessageNotificationBroadcastDuration, "You received a private chat message!", false);
			}

			return (message, color);
		}
	}
}
