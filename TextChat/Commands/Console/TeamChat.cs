using EXILED.Extensions;
using System.Collections.Generic;
using System.Linq;
using TextChat.Enums;
using TextChat.Extensions;
using TextChat.Interfaces;
using TextChat.Localizations;
using static TextChat.Database;

namespace TextChat.Commands.Console
{
	public class TeamChat : Chat, ICommand
	{
		public TeamChat() : base(ChatRoomType.Team)
		{ }

		public string Description => Language.TeamChatDescription;

		public string Usage => Language.TeamChatUsage;

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			(string message, bool isValid) = CheckMessageValidity(args.GetMessage(), ChatPlayers[sender], sender);

			if (!isValid) return (message, "red");

			message = $"[{sender.GetNickname()}][{Language.Team} ({sender.GetRole().ToString().ToUpper()})]: {message}";

			IEnumerable<ReferenceHub> targets = Player.GetHubs().Where(chatPlayer => chatPlayer != sender && chatPlayer.GetTeam() == sender.GetTeam());
			List<Collections.Chat.Player> chatTargets = targets.GetChatPlayers(ChatPlayers);

			if (chatTargets.Count == 0) return (Language.NoAvailablePlayersToChatWithError, "red");

			color = sender.GetColor();

			if (Configs.saveChatToDatabase) SaveMessage(message, ChatPlayers[sender], chatTargets, type);

			SendMessage(ref message, ChatPlayers[sender], targets);

			return (message, color);
		}
	}
}
