using System;
using System.Collections.Generic;
using TextChat.Enums;
using TextChat.Extensions;
using static TextChat.TextChat;

namespace TextChat.Commands.Console
{
	public class Chat
	{
		protected readonly TextChat pluginInstance;
		protected readonly ChatRoomType type;
		protected string color;

		protected Chat(TextChat pluginInstance, ChatRoomType type)
		{
			this.pluginInstance = pluginInstance;
			this.type = type;
		}

		protected Chat(TextChat pluginInstance, ChatRoomType type, string color) : this(pluginInstance, type) => this.color = color;

		protected (string message, bool isValid) CheckMessageValidity(string message, ReferenceHub sender)
		{
			if (string.IsNullOrEmpty(message.Trim())) return ("The message cannot be empty!", false);
			else if (sender.IsChatMuted()) return ("You're muted from the chat room!", false);
			else if (pluginInstance.ChatPlayers[sender].IsFlooding(pluginInstance.slowModeCooldown)) return ("You're sending messages too fast!", false);
			else if (message.Length > pluginInstance.maxMessageLength) return ($"The message is too long! Maximum length: {pluginInstance.maxMessageLength}", false);

			return (message, true);
		}

		protected void SendMessage(string message, ReferenceHub sender, IEnumerable<ReferenceHub> targets)
		{
			pluginInstance.ChatPlayers[sender].lastMessageSentTimestamp = DateTime.Now;

			targets.SendConsoleMessage(pluginInstance.censorBadWords ? message.Sanitize(pluginInstance.badWords, pluginInstance.censorBadWordsChar) : message, color);
		}

		protected void SaveMessage(string message, Collections.Chat.Player sender, List<Collections.Chat.Player> targets)
		{
			Database.GetCollection<Collections.Chat.Room>().Insert(new Collections.Chat.Room()
			{
				Message = new Collections.Chat.Message()
				{
					Sender = sender,
					Targets = targets,
					Content = message,
					Timestamp = DateTime.Now
				},
				Type = ChatRoomType.Private
			});
		}
	}
}
