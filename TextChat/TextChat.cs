using EXILED;
using System.Collections.Generic;
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

		public Dictionary<string, ICommand> ConsoleCommands { get; private set; } = new Dictionary<string, ICommand>();
		public Dictionary<string, ICommand> RemoteAdminCommands { get; private set; } = new Dictionary<string, ICommand>();
		#endregion

		public override string getName => "TextChat";

		public override void OnEnable()
		{
			Configs.Reload();

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
