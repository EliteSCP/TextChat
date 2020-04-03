using EXILED;
using System;
using System.Collections.Generic;
using System.Reflection;
using TextChat.Commands.Console;
using TextChat.Commands.RemoteAdmin;
using TextChat.Events;
using TextChat.Interfaces;
using TextChat.Localizations;

namespace TextChat
{
	public class TextChat : Plugin
	{
		#region Properties
		internal RoundHandler RoundHandler { get; private set; }
		internal PlayerHandler PlayerHandler { get; private set; }
		internal ExiledVersion ExiledVersion { get; private set; } = new ExiledVersion() { Major = 1, Minor = 9, Patch = 8 };
		internal Version Version { get; private set; } = Assembly.GetExecutingAssembly().GetName().Version;

		public Dictionary<string, ICommand> ConsoleCommands { get; private set; } = new Dictionary<string, ICommand>();
		public Dictionary<string, ICommand> RemoteAdminCommands { get; private set; } = new Dictionary<string, ICommand>();
		#endregion

		public override string getName => $"TextChat {Version.Major}.{Version.Minor}.{Version.Build}";

		public override void OnEnable()
		{
			Configs.Reload();

			if (Version.Parse($"{EventPlugin.Version.Major}.{EventPlugin.Version.Minor}.{EventPlugin.Version.Patch}") < Version.Parse($"{ExiledVersion.Major}.{ExiledVersion.Minor}.{ExiledVersion.Patch}"))
			{
				Log.Warn(string.Format(Language.OutdatedVersionError, $"{EventPlugin.Version.Major}.{EventPlugin.Version.Minor}.{EventPlugin.Version.Patch}", $"{ExiledVersion.Major}.{ExiledVersion.Minor}.{ExiledVersion.Patch}"));
			}

			if (!Configs.isEnabled) return;

			RegisterEvents();
			RegisterCommands();

			Database.Open();

			Log.Info(string.Format(Language.PluginEnabled, getName));
		}

		public override void OnDisable()
		{
			Configs.isEnabled = false;

			UnregisterEvents();
			UnregisterCommands();

			Database.Close();

			Log.Info(string.Format(Language.PluginDisabled, getName));
		}

		public override void OnReload()
		{
			Config.Reload();

			Log.Info(string.Format(Language.PluginReloaded, getName));
		}

		#region Events
		private void RegisterEvents()
		{
			RoundHandler = new RoundHandler();
			PlayerHandler = new PlayerHandler(this);

			EXILED.Events.RoundRestartEvent += RoundHandler.OnRoundRestart;

			EXILED.Events.ConsoleCommandEvent += PlayerHandler.OnConsoleCommand;
			EXILED.Events.RemoteAdminCommandEvent += PlayerHandler.OnRemoteAdminCommand;
			EXILED.Events.PlayerJoinEvent += PlayerHandler.OnPlayerJoin;
			EXILED.Events.PlayerLeaveEvent += PlayerHandler.OnPlayerLeave;
		}

		private void UnregisterEvents()
		{
			EXILED.Events.RoundRestartEvent -= RoundHandler.OnRoundRestart;

			EXILED.Events.ConsoleCommandEvent -= PlayerHandler.OnConsoleCommand;
			EXILED.Events.RemoteAdminCommandEvent -= PlayerHandler.OnRemoteAdminCommand;
			EXILED.Events.PlayerJoinEvent -= PlayerHandler.OnPlayerJoin;
			EXILED.Events.PlayerLeaveEvent -= PlayerHandler.OnPlayerLeave;

			RoundHandler = null;
			PlayerHandler = null;
		}
		#endregion

		#region Commands
		private void RegisterCommands()
		{
			ConsoleCommands.Add("chat", new PublicChat());
			ConsoleCommands.Add("chat_team", new TeamChat());
			ConsoleCommands.Add("chat_private", new PrivateChat());
			ConsoleCommands.Add("chat_help", new Help(this));

			RemoteAdminCommands.Add("chat_mute", new Mute());
			RemoteAdminCommands.Add("chat_unmute", new Unmute());
			RemoteAdminCommands.Add("chat_delete_mutes", new DeleteMutes());
			RemoteAdminCommands.Add("chat_show_mutes", new ShowMutes());
		}

		private void UnregisterCommands()
		{
			ConsoleCommands.Clear();
			RemoteAdminCommands.Clear();
		}
		#endregion
	}
}
