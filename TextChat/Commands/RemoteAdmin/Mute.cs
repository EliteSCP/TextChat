using EXILED.Extensions;
using System;
using System.Linq;
using TextChat.Extensions;
using TextChat.Interfaces;
using TextChat.Localizations;
using static TextChat.Database;

namespace TextChat.Commands.RemoteAdmin
{
	public class Mute : ICommand
	{
		public string Description => Language.MuteCommandDescription;

		public string Usage => Language.MuteCommandUsage;

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (!sender.CheckPermission("tc.mute")) return (Language.CommandNotEnoughPermissionsError, "red");

			if (args.Length < 2) return (string.Format(Language.CommandNotEnoughParametersError, 2, Usage), "red");

			ReferenceHub target = Player.GetPlayer(args[0]);

			if (target == null) return (string.Format(Language.PlayerNotFoundError, args[0]), "red");

			if (!double.TryParse(args[1], out double duration) || duration < 1) return (string.Format(Language.InvalidDurationError, args[1]), "red");

			string reason = string.Join(" ", args.Skip(2).Take(args.Length - 2));

			if (string.IsNullOrEmpty(reason)) return (Language.ReasonCannotBeEmptyError, "red");

			if (target.IsChatMuted()) return (string.Format(Language.PlayerIsAlreadyMutedError, target.GetNickname()), "red");

			LiteDatabase.GetCollection<Collections.Chat.Mute>().Insert(new Collections.Chat.Mute()
			{
				Target = ChatPlayers[target],
				Issuer = ChatPlayers[sender],
				Reason = reason,
				Timestamp = DateTime.Now,
				Expire = DateTime.Now.AddMinutes(duration)
			});

			if (Configs.showChatMutedBroadcast)
			{
				target.ClearBroadcasts();
				target.Broadcast(Configs.chatMutedBroadcastDuration, string.Format(Configs.chatMutedBroadcast, duration, reason), false);
			}

			target.SendConsoleMessage(string.Format(Language.MuteCommandSuccessPlayer, duration, reason), "red");

			return (string.Format(Language.MuteCommandSuccessModerator, target.GetNickname(), duration, reason), "green");
		}
	}
}
