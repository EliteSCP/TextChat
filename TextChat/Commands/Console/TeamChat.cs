using EXILED.Extensions;
using System.Collections.Generic;
using System.Linq;
using TextChat.Enums;
using TextChat.Extensions;
using TextChat.Interfaces;
using static TextChat.Database;

namespace TextChat.Commands.Console
{
	public class TeamChat : Chat, ICommand
	{
		public TeamChat() : base(ChatRoomType.Team)
		{ }

		public string Description => "Sends a chat message to your team.";

		public string Usage => ".chat_team [Message]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			(string message, bool isValid) = CheckMessageValidity(args.GetMessage(), ChatPlayers[sender], sender);

			if (!isValid) return (message, "red");

			message = $"[{sender.GetNickname()}][TEAM ({sender.GetRole().ToString().ToUpper()})]: {message}";

			IEnumerable<ReferenceHub> targets = Player.GetHubs().Where(chatPlayer => chatPlayer != sender && chatPlayer.GetTeam() == sender.GetTeam());
			List<Collections.Chat.Player> chatTargets = targets.GetChatPlayers(ChatPlayers);

			if (chatTargets.Count == 0) return ("There are no available players to chat with!", "red");

			color = sender.GetColor();

			SendMessage(message, ChatPlayers[sender], targets);

			if (Configs.saveChatToDatabase) SaveMessage(message, ChatPlayers[sender], chatTargets, type);

			return (message, color);
		}
	}
}
