namespace TextChat.Commands.RemoteAdmin.Mute
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using Extensions;
    using Localizations;
    using NorthwoodLib.Pools;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Database;

    public class Show : ICommand
	{
		public string Description => Language.ShowMutesCommandDescription;

		public string Command => "show" ;

		public string[] Aliases => new[] { "s" };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
			if (!sender.CheckPermission("tc.showmutes"))
			{
				response = Language.CommandNotEnoughPermissionsError;
				return false;
			}

			if (arguments.Count == 0)
			{
				response = GetPlayerMutes(LiteDatabase.GetCollection<Collections.Chat.Mute>().FindAll().ToList()).ToString();
				return false;
			}
			else if (arguments.Count == 1)
			{
				Collections.Chat.Player chatPlayer = arguments.At(0).GetChatPlayer();

				if (chatPlayer == null)
				{
					response = string.Format(Language.PlayerNotFoundError, arguments.At(0));
					return false;
				}

				response = GetPlayerMutes(LiteDatabase.GetCollection<Collections.Chat.Mute>().Find(mute => mute.Target.Id == chatPlayer.Id).ToList()).ToString();
				return true;
			}

			response = Language.CommandTooManyArgumentsError;
			return false;
		}

		private string GetPlayerMutes(List<Collections.Chat.Mute> playerMutesList)
		{
			var playerMutes = StringBuilderPool.Shared.Rent($"\n[{Language.MutesList} ({playerMutesList.Count})]\n");

			foreach (Collections.Chat.Mute playerMute in playerMutesList)
			{
				playerMutes.AppendLine($"\n[{playerMute.Target.Name} ({playerMute.Target.Id}@{playerMute.Target.Authentication})]");
				playerMutes.AppendLine($"{Language.Issuer}: {playerMute.Issuer.Name} ({playerMute.Issuer.Id}@{playerMute.Issuer.Authentication})");
				playerMutes.AppendLine($"{Language.Reason}: {playerMute.Reason}");
				playerMutes.AppendLine($"{Language.Duration}: {playerMute.Duration} {Language.Minutes}");
				playerMutes.AppendLine($"{Language.Timestamp}: {playerMute.Timestamp}");
				playerMutes.AppendLine($"{Language.Expire}: {playerMute.Expire}");
			}

			var playerMutesString = playerMutes.ToString();

			StringBuilderPool.Shared.Return(playerMutes);

			return playerMutesString;
		}
	}
}
