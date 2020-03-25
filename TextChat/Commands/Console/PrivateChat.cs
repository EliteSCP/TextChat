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
	public class PrivateChat : ICommand
	{
		private readonly TextChat pluginInstance;

		public PrivateChat(TextChat pluginInstance) => this.pluginInstance = pluginInstance;

		public string Description => "Sends a private chat message to a player.";

		public string Usage => ".chat_private [Nickname/UserID/PlayerID] [Message]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (args.Length == 0) return ("The message cannot be empty!", "red");
			else if (sender.IsChatMuted()) return ("You're muted from the chat room!", "red");
			else if (pluginInstance.ChatPlayers[sender].IsFlooding(pluginInstance.slowModeCooldown)) return ("You're sending messages too fast!", "red");

			string message = string.Join(" ", args.Skip(1).Take(args.Length - 1));

			if (message.Length > pluginInstance.maxMessageLength) return ($"The message is too long! Maximum length: {pluginInstance.maxMessageLength}", "red");

			message = $"[{sender.GetNickname()}][PRIVATE]: {message}";

			ReferenceHub target = Player.GetPlayer(args[0]);

			if (target == null) return ($"Player {target.GetNickname()} was not found!", "red");
			else if (sender == target) return ("You cannot send a message to yourself!", "red");
			else if (!pluginInstance.canSpectatorSendMessagesToAlive && sender.GetTeam() == Team.RIP && target.GetTeam() != Team.RIP)
			{
				return ("You cannot send messages to alive players!", "red");
			}

			string color = pluginInstance.privateMessageColor;

			pluginInstance.ChatPlayers[sender].lastMessageSentTimestamp = DateTime.Now;

			if (pluginInstance.saveChatToDatabase)
			{
				Database.GetCollection<Collections.Chat.Room>().Insert(new Collections.Chat.Room()
				{
					Message = new Collections.Chat.Message()
					{
						Sender = pluginInstance.ChatPlayers[sender],
						Targets = new List<Collections.Chat.Player>() { pluginInstance.ChatPlayers[target] },
						Content = message,
						Timestamp = DateTime.Now
					},
					Type = ChatRoomType.Private
				});
			}

			target.SendConsoleMessage(pluginInstance.censorBadWords ? message.Sanitize(pluginInstance.badWords, pluginInstance.censorBadWordsChar) : message, color);

			if (pluginInstance.showPrivateMessageNotificationBroadcast)
			{
				target.ClearBroadcasts();
				target.Broadcast(pluginInstance.privateMessageNotificationBroadcastDuration, "You received a private chat message!", false);
			}

			return (message, color);
		}
	}
}
