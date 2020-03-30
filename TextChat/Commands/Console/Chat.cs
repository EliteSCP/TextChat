using System.Collections.Generic;
using TextChat.Collections.Chat;
using TextChat.Enums;
using TextChat.Extensions;

namespace TextChat.Commands.Console
{
	public class Chat
	{
		protected readonly ChatRoomType type;
		protected string color;

		protected Chat(ChatRoomType type) => this.type = type;

		protected Chat(ChatRoomType type, string color) : this(type) => this.color = color;

		protected (string message, bool isValid) CheckMessageValidity(string message, Player messageSender, ReferenceHub sender)
		{
			if (string.IsNullOrEmpty(message.Trim())) return ("The message cannot be empty!", false);
			else if (sender.IsChatMuted()) return ("You're muted from the chat room!", false);
			else if (messageSender.IsFlooding(Configs.slowModeCooldown)) return ("You're sending messages too fast!", false);
			else if (message.Length > Configs.maxMessageLength) return ($"The message is too long! Maximum length: {Configs.maxMessageLength}", false);

			return (message, true);
		}

		protected void SendMessage(ref string message, Player sender, IEnumerable<ReferenceHub> targets)
		{
			targets.SendConsoleMessage(message = Configs.censorBadWords ? message.Sanitize(Configs.badWords, Configs.censorBadWordsChar) : message, color);

			sender.SentAMessage();
		}
	}
}
