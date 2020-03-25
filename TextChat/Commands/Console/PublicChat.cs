using EXILED.Extensions;
using System.Collections.Generic;
using System.Linq;
using TextChat.Enums;
using TextChat.Extensions;
using TextChat.Interfaces;

namespace TextChat.Commands.Console
{
	public class PublicChat : Chat, ICommand
	{
		public PublicChat(TextChat pluginInstance) : base(pluginInstance, ChatRoomType.Public, pluginInstance.generalChatColor)
		{ }

		public string Description => "Sends a chat message to everyone in the server.";

		public string Usage => ".chat [Message]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			(string message, bool isValid) = CheckMessageValidity(args.GetMessage(), sender);

			if (!isValid) return (message, "red");

			message = $"[{sender.GetNickname()}][PUBLIC]: {message}";

			IEnumerable<ReferenceHub> targets = Player.GetHubs().Where(target =>
			{
				return sender != target && (pluginInstance.canSpectatorSendMessagesToAlive || sender.GetTeam() != Team.RIP || target.GetTeam() == Team.RIP);
			});

			List<Collections.Chat.Player> chatPlayers = targets.GetChatPlayers(pluginInstance.ChatPlayers);

			if (chatPlayers.Count == 0) return ("There are no available players to chat with!", "red");

			SendMessage(message, sender, targets);

			if (pluginInstance.saveChatToDatabase) SaveMessage(message, pluginInstance.ChatPlayers[sender], chatPlayers);

			return (message, color);
		}
	}
}
