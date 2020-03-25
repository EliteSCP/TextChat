using EXILED.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TextChat.Enums;
using TextChat.Extensions;
using TextChat.Interfaces;
using static TextChat.TextChat;

namespace TextChat.Commands.Console
{
	public class TeamChat : ICommand
	{
		private readonly TextChat pluginInstance;

		public TeamChat(TextChat pluginInstance) => this.pluginInstance = pluginInstance;

		public string Description => "Sends a chat message to your team.";

		public string Usage => ".chat_team [Message]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (args.Length == 0) return ("The message cannot be empty!", "red");
			else if (sender.IsChatMuted()) return ("You are muted from the chat room!", "red");
			else if (pluginInstance.ChatPlayers[sender].IsFlooding(pluginInstance.slowModeCooldown)) return ("You're sending messages too fast!", "red");

			string message = string.Join(" ", args);

			if (message.Length > pluginInstance.maxMessageLength) return ($"The message is too long! Maximum length: {pluginInstance.maxMessageLength}", "red");

			message = $"[{sender.GetNickname()}][TEAM ({sender.GetRole().ToString().ToUpper()})]: {message}";

			IEnumerable<ReferenceHub> targets = Player.GetHubs().Where(chatPlayer => chatPlayer != sender && chatPlayer.GetTeam() == sender.GetTeam());
			List<Collections.Chat.Player> chatTargets = targets.GetChatPlayers(pluginInstance.ChatPlayers);

			if (chatTargets.Count == 0) return ("There are no available players to chat with!", "red");

			string color = sender.GetColor();

			pluginInstance.ChatPlayers[sender].lastMessageSentTimestamp = DateTime.Now;

			if (pluginInstance.saveChatToDatabase)
			{
				Database.GetCollection<Collections.Chat.Room>().Insert(new Collections.Chat.Room()
				{
					Message = new Collections.Chat.Message()
					{
						Sender = pluginInstance.ChatPlayers[sender],
						Targets = chatTargets,
						Content = message,
						Timestamp = DateTime.Now
					},
					Type = ChatRoomType.Team
				});
			}

			targets.SendConsoleMessage(pluginInstance.censorBadWords ? message.Sanitize(pluginInstance.badWords, pluginInstance.censorBadWordsChar) : message, color);

			return (message, color);
		}
	}
}
