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

			Collections.Chat.Player chatPlayer = args[0].GetChatPlayer();

			if (chatPlayer == null) return (string.Format(Language.PlayerNotFoundError, args[0]), "red");

			var mute = LiteDatabase.GetCollection<Collections.Chat.Mute>().FindOne(queryMute => queryMute.Target.Id == chatPlayer.Id && queryMute.Expire > DateTime.Now);

			if (mute == null) return (string.Format(Language.PlayerIsNotMutedError, chatPlayer.Name), "red");

			mute.Expire = DateTime.Now;

			LiteDatabase.GetCollection<Collections.Chat.Mute>().Update(mute);

			Player.GetPlayer(args[0])?.SendConsoleMessage(Language.UnmuteCommandSuccessPlayer, "green");

			return (string.Format(Language.UnmuteCommandSuccessModerator, chatPlayer.Name), "green");
		}
	}
}
