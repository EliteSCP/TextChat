using EXILED;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using TextChat.Commands.Console;
using TextChat.Commands.RemoteAdmin;
using TextChat.Events;
using TextChat.Interfaces;

namespace TextChat
{
	public class TextChat : Plugin
	{
		#region Properties
		internal RoundHandler RoundEvent { get; private set; }
		internal PlayerHandler PlayerEvent { get; private set; }

		public static LiteDatabase Database { get; private set; }

		public Dictionary<string, ICommand> ConsoleCommands { get; private set; }
		public Dictionary<string, ICommand> RemoteAdminCommands { get; private set; }
		public Dictionary<ReferenceHub, Collections.Chat.Player> ChatPlayers { get; private set; }

		public string DatabaseDirectory => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Plugins"), getName);
		public string DatabaseFullPath => Path.Combine(DatabaseDirectory, $"{databaseName}.db");
		#endregion

		#region Configs
		internal bool isEnabled;
		internal string databaseName;
		internal string generalChatColor;
		internal string privateMessageColor;
		internal int maxMessageLength;
		internal bool censorBadWords;
		internal char censorBadWordsChar;
		internal List<string> badWords;
		internal bool saveChatToDatabase;
		internal bool canSpectatorSendMessagesToAlive;
		internal bool showChatMutedBroadcast;
		internal uint chatMutedBroadcastDuration;
		internal bool showPrivateMessageNotificationBroadcast;
		internal uint privateMessageNotificationBroadcastDuration;
		internal bool isSlowModeEnabled;
		internal TimeSpan slowModeCooldown;
		#endregion

		public override string getName => "TextChat";

		public TextChat()
		{
			ConsoleCommands = new Dictionary<string, ICommand>();
			RemoteAdminCommands = new Dictionary<string, ICommand>();
			ChatPlayers = new Dictionary<ReferenceHub, Collections.Chat.Player>();
		}

		public override void OnEnable()
		{
			LoadConfigs();

			if (!isEnabled) return;

			RegisterEvents();
			RegisterCommands();

			OpenDatabase();

			Log.Info($"{getName} has been Enabled!");
		}

		public override void OnDisable()
		{
			isEnabled = false;

			UnregisterEvents();
			UnregisterCommands();

			CloseDatabase();

			Log.Info($"{getName} has been Disabled!");
		}

		public override void OnReload() => Log.Info($"{getName} has been Reloaded!");

		internal void LoadConfigs()
		{
			isEnabled = Config.GetBool("tc_enabled", true);
			databaseName = Config.GetString("tc_database_name", getName);
			generalChatColor = Config.GetString("tc_general_chat_color", "cyan");
			privateMessageColor = Config.GetString("tc_private_message_color", "magenta");
			maxMessageLength = Config.GetInt("tc_max_message_length", 75);
			censorBadWords = Config.GetBool("tc_censor_bad_words");
			censorBadWordsChar = Config.GetChar("tc_censor_bad_words_char", '*');
			badWords = Config.GetStringList("tc_bad_words");
			saveChatToDatabase = Config.GetBool("tc_save_chat_to_database", true);
			canSpectatorSendMessagesToAlive = Config.GetBool("tc_can_spectator_send_messages_to_alive");
			showChatMutedBroadcast = Config.GetBool("tc_show_chat_muted_broadcast", true);
			chatMutedBroadcastDuration = Config.GetUInt("tc_chat_muted_broadcast_duration", 10);
			showPrivateMessageNotificationBroadcast = Config.GetBool("tc_show_private_message_notification_broadcast");
			privateMessageNotificationBroadcastDuration = Config.GetUInt("tc_private_message_notification_broadcast_duration", 6);
			isSlowModeEnabled = Config.GetBool("tc_is_slow_mode_enabled", true);
			slowModeCooldown = new TimeSpan(0, 0, 0, 0, (int)(Config.GetFloat("tc_slow_mode_interval", 1f) * 1000));
		}

		#region Events
		private void RegisterEvents()
		{
			RoundEvent = new RoundHandler(this);
			PlayerEvent = new PlayerHandler(this);

			EXILED.Events.WaitingForPlayersEvent += RoundEvent.OnWaitingForPlayers;
			EXILED.Events.RoundRestartEvent += RoundEvent.OnRoundRestart;

			EXILED.Events.ConsoleCommandEvent += PlayerEvent.OnConsoleCommand;
			EXILED.Events.RemoteAdminCommandEvent += PlayerEvent.OnRemoteAdminCommand;
			EXILED.Events.PlayerJoinEvent += PlayerEvent.OnPlayerJoin;
			EXILED.Events.PlayerLeaveEvent += PlayerEvent.OnPlayerLeave;
		}

		private void UnregisterEvents()
		{
			EXILED.Events.WaitingForPlayersEvent -= RoundEvent.OnWaitingForPlayers;
			EXILED.Events.RoundRestartEvent -= RoundEvent.OnRoundRestart;

			EXILED.Events.ConsoleCommandEvent -= PlayerEvent.OnConsoleCommand;
			EXILED.Events.RemoteAdminCommandEvent -= PlayerEvent.OnRemoteAdminCommand;
			EXILED.Events.PlayerJoinEvent -= PlayerEvent.OnPlayerJoin;
			EXILED.Events.PlayerLeaveEvent -= PlayerEvent.OnPlayerLeave;

			RoundEvent = null;
			PlayerEvent = null;
		}
		#endregion

		#region Commands
		private void RegisterCommands()
		{
			ConsoleCommands.Add("chat", new PublicChat(this));
			ConsoleCommands.Add("chat_team", new TeamChat(this));
			ConsoleCommands.Add("chat_private", new PrivateChat(this));
			ConsoleCommands.Add("help", new Help(this));

			RemoteAdminCommands.Add("chatmute", new Mute(this));
			RemoteAdminCommands.Add("chatunmute", new Unmute(this));
		}

		private void UnregisterCommands()
		{
			ConsoleCommands.Clear();
			RemoteAdminCommands.Clear();
		}
		#endregion

		#region Database
		private void OpenDatabase()
		{
			try
			{
				if (!Directory.Exists(DatabaseDirectory)) Directory.CreateDirectory(DatabaseDirectory);

				Database = new LiteDatabase(DatabaseFullPath);

				Database.GetCollection<Collections.Chat.Mute>().EnsureIndex(mute => mute.Target.Id);
				Database.GetCollection<Collections.Chat.Mute>().EnsureIndex(mute => mute.Issuer.Id);
				Database.GetCollection<Collections.Chat.Mute>().EnsureIndex(mute => mute.Timestamp);
				Database.GetCollection<Collections.Chat.Mute>().EnsureIndex(mute => mute.Expire);
				Database.GetCollection<Collections.Chat.Room>().EnsureIndex(room => room.Type);
				Database.GetCollection<Collections.Chat.Room>().EnsureIndex(room => room.Message.Sender.Id);

				Log.Info("The database has been loaded successfully!");
			}
			catch (Exception exception)
			{
				Log.Error($"An error has occurred while opening the database: {exception}");
			}
		}

		private void CloseDatabase()
		{
			try
			{
				Database.Dispose();
				Database = null;

				Log.Info("The database has been closed successfully!");
			}
			catch (Exception exception)
			{
				Log.Error($"An error has occurred while closing the database: {exception}");
			}
		}
		#endregion
	}
}
