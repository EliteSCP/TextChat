using System;
using System.Collections.Generic;
using TextChat.Collections.Chat;
using TextChat.Enums;
using TextChat.Extensions;
using TextChat.Localizations;

namespace TextChat.Commands.Console
{
	public class Chat
	{
		protected readonly ChatRoomType type;
		protected string color;

		protected Chat(ChatRoomType type) => this.type = type;

		protected Chat(ChatRoomType type, string color) : this(type) => this.color = color;

		protected (string message, bool isValid) CheckMessageValidity(string message, ReferenceHub sender)
		{
			if (string.IsNullOrEmpty(message.Trim())) return (Language.ChatMessageCannotBeEmptyError, false);
			else if (sender.IsChatMuted()) return (Language.PlayerIsMutedError, false);
			else if (sender.GetChatPlayer().IsFlooding(Configs.slowModeCooldown)) return (Language.PlayerIsFloodingError, false);
			else if (message.Length > Configs.maxMessageLength) return (string.Format(Language.ChatMessageTooLongError, Configs.maxMessageLength), false);

			return (message, true);
		}

		protected void SendMessage(ref string message, ReferenceHub sender, IEnumerable<ReferenceHub> targets)
		{
			targets.SendConsoleMessage(message = Configs.censorBadWords ? message.Sanitize(Configs.badWords, Configs.censorBadWordsChar) : message, color);

			sender.GetChatPlayer().LastMessageSentTimestamp = DateTime.Now;
		}
	}
}
