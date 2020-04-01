using TextChat.Extensions;
using TextChat.Interfaces;
using TextChat.Localizations;
using static TextChat.Database;

namespace TextChat.Commands.RemoteAdmin
{
	public class DeleteMutes : ICommand
	{
		public string Description => Language.DeleteMutesCommandDescription;

		public string Usage => Language.DeleteMutesCommandUsage;

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (!sender.CheckPermission("tc.deletemutes")) return (Language.CommandNotEnoughPermissionsError, "red");

			if (args.Length == 0) return (string.Format(Language.DeleteAllMutesSuccess, LiteDatabase.GetCollection<Collections.Chat.Mute>().DeleteAll()), "green");
			else if (args.Length == 1)
			{
				Collections.Chat.Player chatPlayer = args[0].GetChatPlayer();

				if (chatPlayer == null) return (string.Format(Language.PlayerNotFoundError, args[0]), "red");

				return (string.Format(Language.DeleteMutesSuccess, LiteDatabase.GetCollection<Collections.Chat.Mute>().DeleteMany(mute => mute.Target.Id == chatPlayer.Id), chatPlayer.Name), "green");
			}

			return (Language.CommandTooManyArgumentsError, "red");
		}
	}
}
