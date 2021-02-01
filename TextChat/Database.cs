namespace TextChat
{
    using Collections.Chat;
    using Enums;
    using LiteDB;
    using Localizations;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using static TextChat;
    using Log = Exiled.API.Features.Log;

    internal static class Database
	{
		public static LiteDatabase LiteDatabase { get; private set; }
		public static Dictionary<Exiled.API.Features.Player, Player> ChatPlayers { get; private set; } = new Dictionary<Exiled.API.Features.Player, Player>();
		public static Player ServerChatPlayer = new Player()
		{
			Id = "Server",
			Authentication = "Server",
			Name = "Server"
		};

		public static string Folder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Plugins"), Instance.Config.DatabaseName);
		public static string FullPath => Path.Combine(Folder, $"{Instance.Config.DatabaseName}.db");

		public static void Open()
		{
			try
			{
				if (!Directory.Exists(Folder)) Directory.CreateDirectory(Folder);

				LiteDatabase = new LiteDatabase(FullPath);

				LiteDatabase.GetCollection<Player>().EnsureIndex(player => player.Id, true);
				LiteDatabase.GetCollection<Mute>().EnsureIndex(mute => mute.Target.Id);
				LiteDatabase.GetCollection<Mute>().EnsureIndex(mute => mute.Issuer.Id);
				LiteDatabase.GetCollection<Mute>().EnsureIndex(mute => mute.Timestamp);
				LiteDatabase.GetCollection<Mute>().EnsureIndex(mute => mute.Expire);
				LiteDatabase.GetCollection<Room>().EnsureIndex(room => room.Type);
				LiteDatabase.GetCollection<Room>().EnsureIndex(room => room.Message.Sender.Id);

                Log.Info(Language.DatabaseLoaded);
			}
			catch (Exception exception)
			{
                Log.Error(string.Format(Language.DatabaseLoadError, exception));
			}
		}

		public static void Close()
		{
			try
			{
				LiteDatabase.Checkpoint();
				LiteDatabase.Dispose();
				LiteDatabase = null;

                Log.Info(Language.DatabaseClosed);
			}
			catch (Exception exception)
			{
                Log.Error(string.Format(Language.DatabaseCloseError, exception));
			}
		}

		public static void SaveMessage(string message, Player sender, List<Player> targets, ChatRoomType chatRoomType)
		{
			LiteDatabase.GetCollection<Room>().Insert(new Room()
			{
				Message = new Message()
				{
					Sender = sender,
					Targets = targets,
					Content = message,
					Timestamp = DateTime.Now
				},
				Type = chatRoomType
			});
		}
	}
}
