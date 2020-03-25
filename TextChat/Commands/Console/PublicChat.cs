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
	public class PublicChat : ICommand
	{
		private readonly TextChat pluginInstance;

		public PublicChat(TextChat pluginInstance) => this.pluginInstance = pluginInstance;

		public string Description => "Sends a chat message to everyone in the server.";

		public string Usage => ".chat [Message]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (args.Length == 0) return ("You cannot send an empty message!", "red");
			else if (sender.IsChatMuted()) return ("You're muted from the chat room!", "red");
			else if (pluginInstance.ChatPlayers[sender].IsFlooding(pluginInstance.slowModeCooldown)) return ("You're sending messages too fast!", "red");

			string message = string.Join(" ", args);

			if (message.Length > pluginInstance.maxMessageLength) return ($"The message is too long! Maximum length: {pluginInstance.maxMessageLength}", "red");

			message = $"[{sender.GetNickname()}][PUBLIC]: {message}";

			IEnumerable<ReferenceHub> targets = Player.GetHubs().Where(target =>
			{
				return sender != target && (pluginInstance.canSpectatorSendMessagesToAlive || sender.GetTeam() != Team.RIP || target.GetTeam() == Team.RIP);
			});

			List<Collections.Chat.Player> chatPlayers = targets.GetChatPlayers(pluginInstance.ChatPlayers);

			if (chatPlayers.Count == 0) return ("There are no available players to chat with!", "red");

			string color = pluginInstance.generalChatColor;

			pluginInstance.ChatPlayers[sender].lastMessageSentTimestamp = DateTime.Now;

			if (pluginInstance.saveChatToDatabase)
			{
				Database.GetCollection<Collections.Chat.Room>().Insert(new Collections.Chat.Room()
				{
					Message = new Collections.Chat.Message()
					{
						Sender = pluginInstance.ChatPlayers[sender],
						Targets = chatPlayers,
						Content = message,
						Timestamp = DateTime.Now
					},
					Type = ChatRoomType.Public
				});
			}

			targets.SendConsoleMessage(message.Sanitize(pluginInstance.badWords, pluginInstance.censorBadWordsChar), color);

			return (message, color);
		}
	}
}
