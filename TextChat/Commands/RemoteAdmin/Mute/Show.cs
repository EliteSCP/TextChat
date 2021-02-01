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
		public string Description { get; } = Language.ShowMutesCommandDescription;

		public string Command { get; } = "show" ;

		public string[] Aliases { get; } = new[] { "s" };

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
			var message = StringBuilderPool.Shared.Rent();

			message.AppendLine().Append('[').Append(Language.MutesList).Append(" (").Append(playerMutesList.Count).Append(")]").AppendLine();

			foreach (Collections.Chat.Mute playerMute in playerMutesList)
			{
				message.AppendLine().Append('[').Append(playerMute.Target.Name).Append(" (").Append(playerMute.Target.Id).Append('@').Append(playerMute.Target.Authentication).Append(")]").AppendLine()
					.Append(Language.Issuer).Append(": ").Append(playerMute.Issuer.Name).Append(" (").Append(playerMute.Issuer.Id).Append('@').Append(playerMute.Issuer.Authentication).Append(')').AppendLine()
					.Append(Language.Reason).Append(": ").Append(playerMute.Reason).AppendLine()
					.Append(Language.Duration).Append(": ").Append(playerMute.Duration).Append(' ').Append(Language.Minutes).AppendLine()
					.Append(Language.Timestamp).Append(": ").Append(playerMute.Timestamp).AppendLine()
					.Append(Language.Expire).Append(": ").Append(playerMute.Expire).AppendLine();
			}

			var playerMutesString = message.ToString();

			StringBuilderPool.Shared.Return(message);

			return playerMutesString;
		}
	}
}
