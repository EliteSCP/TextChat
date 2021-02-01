namespace TextChat.Commands.Console.Chat
{
    using Enums;
    using Exiled.API.Features;
    using Extensions;
    using Localizations;
    using System;
    using System.Collections.Generic;
    using static TextChat;

    public class Message
	{
		protected readonly ChatRoomType type;
		protected string color;

		protected Message(ChatRoomType type) => this.type = type;

		protected Message(ChatRoomType type, string color) : this(type) => this.color = color;

		protected bool CheckValidity(string message, Player sender, out string response)
		{
			if (string.IsNullOrEmpty(message.Trim()))
			{
				response = Language.ChatMessageCannotBeEmptyError;
				return false;
			}
			else if (sender.IsChatMuted())
			{
				response = Language.PlayerIsMutedError;
				return false;
			}
			else if (sender.GetChatPlayer().IsFlooding(Instance.Config.SlowModeCooldown))
			{
				response = Language.PlayerIsFloodingError;
				return false;
			}
			else if (message.Length > Instance.Config.MaxMessageLength)
			{
				response = string.Format(Language.ChatMessageTooLongError, Instance.Config.MaxMessageLength);
				return false;
			}

			response = message;
			return true;
		}

		protected void Send(ref string message, Player sender, IEnumerable<Player> targets)
		{
			targets.SendConsoleMessage(message = Instance.Config.ShouldCensorBadWords ? message.Sanitize(Instance.Config.BadWords, Instance.Config.CensorBadWordsChar) : message, color);

			sender.GetChatPlayer().LastMessageSentTimestamp = DateTime.Now;
		}
	}
}
