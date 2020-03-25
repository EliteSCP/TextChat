using EXILED.Extensions;
using System;
using System.Linq;
using TextChat.Extensions;
using TextChat.Interfaces;
using static TextChat.TextChat;

namespace TextChat.Commands.RemoteAdmin
{
	public class Mute : ICommand
	{
		private readonly TextChat pluginInstance;

		public Mute(TextChat pluginInstance) => this.pluginInstance = pluginInstance;

		public string Description => "Mute a player from the chat.";

		public string Usage => "chatmute [PlayerID/UserID/Name] [Duration (Minutes)] [Reason]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (!sender.CheckPermission("tc.mute")) return ("You don't have enough permissions to run this command!", "red");

			if (args.Length < 2) return ($"You have to provide two parameter! {Usage}", "red");

			ReferenceHub target = Player.GetPlayer(args[0]);

			if (target == null) return ($"Player {args[0]} was not found!", "red");

			if (!double.TryParse(args[1], out double duration) || duration < 1) return ($"{args[1]} is an invalid duration!", "red");

			string reason = string.Join(" ", args.Skip(2).Take(args.Length - 2));

			if (string.IsNullOrEmpty(reason)) return ("The reason cannot be empty!", "red");

			if (target.IsChatMuted()) return ($"{target.GetNickname()} is already muted!", "red");

			Database.GetCollection<Collections.Chat.Mute>().Insert(new Collections.Chat.Mute()
			{
				Target = pluginInstance.ChatPlayers[target],
				Issuer = pluginInstance.ChatPlayers[sender],
				Reason = reason,
				Timestamp = DateTime.Now,
				Expire = DateTime.Now.AddMinutes(duration)
			});

			if (pluginInstance.showChatMutedBroadcast)
			{
				target.ClearBroadcasts();
				target.Broadcast(pluginInstance.chatMutedBroadcastDuration, $"You have been muted from the chat for {duration} minute{(duration != 1 ? "s" : "")}! Reason: {reason}", false);
			}

			target.SendConsoleMessage($"You have been muted from the chat for {duration} minute{(duration != 1 ? "s" : "")}! Reason: {reason}", "red");

			return ($"{target.GetNickname()} has been muted from the chat for {duration} minute{(duration != 1 ? "s" : "")}, reason: {reason}", "green");
		}
	}
}
