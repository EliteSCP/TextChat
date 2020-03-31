using EXILED.Extensions;
using System;
using TextChat.Extensions;
using TextChat.Interfaces;
using TextChat.Localizations;
using static TextChat.Database;

namespace TextChat.Commands.RemoteAdmin
{
	public class Unmute : ICommand
	{
		public string Description => Language.UnmuteCommandDescription;

		public string Usage => Language.UnmuteCommandUsage;

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (!sender.CheckPermission("tc.unmute")) return (Language.CommandNotEnoughPermissionsError, "red");

			if (args.Length != 1) return (string.Format(Language.CommandNotEnoughParametersError, 1, Usage), "red");

			ReferenceHub target = Player.GetPlayer(args[0]);

			if (target == null) return (string.Format(Language.PlayerNotFoundError, args[0]), "red");

			var mutedPlayer = LiteDatabase.GetCollection<Collections.Chat.Mute>().FindOne(mute => mute.Target.Id == target.GetRawUserId() && mute.Expire > DateTime.Now);

			if (mutedPlayer == null) return (string.Format(Language.PlayerIsNotMutedError, target.GetNickname()), "red");

			mutedPlayer.Expire = DateTime.Now;

			LiteDatabase.GetCollection<Collections.Chat.Mute>().Update(mutedPlayer);

			target.SendConsoleMessage(Language.UnmuteCommandSuccessPlayer, "green");

			return (string.Format(Language.UnmuteCommandSuccessModerator, target.GetNickname()), "green");
		}
	}
}
