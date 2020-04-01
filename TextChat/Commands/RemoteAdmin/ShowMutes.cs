using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextChat.Extensions;
using TextChat.Interfaces;
using TextChat.Localizations;
using static TextChat.Database;

namespace TextChat.Commands.RemoteAdmin
{
	public class ShowMutes : ICommand
	{
		public string Description => Language.ShowMutesCommandDescription;

		public string Usage => Language.ShowMutesCommandUsage;

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (!sender.CheckPermission("tc.showmutes")) return (Language.CommandNotEnoughPermissionsError, "red");

			if (args.Length == 0) return (GetPlayerMutes(LiteDatabase.GetCollection<Collections.Chat.Mute>().FindAll().ToList()).ToString(), "white");
			else if (args.Length == 1)
			{
				Collections.Chat.Player chatPlayer = args[0].GetChatPlayer();

				if (chatPlayer == null) return (string.Format(Language.PlayerNotFoundError, args[0]), "red");

				return (GetPlayerMutes(LiteDatabase.GetCollection<Collections.Chat.Mute>().Find(mute => mute.Target.Id == chatPlayer.Id).ToList()).ToString(), "white");
			}

			return (Language.CommandTooManyArgumentsError, "red");
		}

		private StringBuilder GetPlayerMutes(List<Collections.Chat.Mute> playerMutesList)
		{
			StringBuilder playerMutes = new StringBuilder($"\n[{Language.MutesList} ({playerMutesList.Count})]\n");

			foreach (Collections.Chat.Mute playerMute in playerMutesList)
			{
				playerMutes.AppendLine($"\n[{playerMute.Target.Name} ({playerMute.Target.Id}@{playerMute.Target.Authentication})]");
				playerMutes.AppendLine($"{Language.Issuer}: {playerMute.Issuer.Name} ({playerMute.Issuer.Id}@{playerMute.Issuer.Authentication})");
				playerMutes.AppendLine($"{Language.Reason}: {playerMute.Reason}");
				playerMutes.AppendLine($"{Language.Duration}: {playerMute.Duration} {Language.Minutes}");
				playerMutes.AppendLine($"{Language.Timestamp}: {playerMute.Timestamp}");
				playerMutes.AppendLine($"{Language.Expire}: {playerMute.Expire}");
			}

			return playerMutes;
		}
	}
}
