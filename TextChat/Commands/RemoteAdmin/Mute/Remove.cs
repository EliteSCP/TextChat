namespace TextChat.Commands.RemoteAdmin.Mute
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using Extensions;
	using Localizations;
    using System;
    using static Database;

	public class Remove : ICommand
	{
		public string Description => Language.RemoveMutesCommandDescription;

        public string Command => "remove";

		public string[] Aliases => new[] { "r" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
			if (!sender.CheckPermission("tc.deletemutes"))
			{
				response = Language.CommandNotEnoughPermissionsError;
				return false;
			}

			if (arguments.Count == 0)
			{
				response = string.Format(Language.RemoveAllMutesSuccess, LiteDatabase.GetCollection<Collections.Chat.Mute>().DeleteAll());
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

				response = string.Format(Language.RemoveMutesSuccess, LiteDatabase.GetCollection<Collections.Chat.Mute>().DeleteMany(mute => mute.Target.Id == chatPlayer.Id), chatPlayer.Name);
				return true;
			}

			response = Language.CommandTooManyArgumentsError;
			return false;
		}
	}
}
