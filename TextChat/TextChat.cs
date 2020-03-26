using EXILED;
using System.Collections.Generic;
using TextChat.Commands.Console;
using TextChat.Commands.RemoteAdmin;
using TextChat.Events;
using TextChat.Interfaces;

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

			Log.Info($"{getName} has been Enabled!");
		}

		public override void OnDisable()
		{
			Configs.isEnabled = false;

			UnregisterEvents();
			UnregisterCommands();

			Database.Close();

			Log.Info($"{getName} has been Disabled!");
		}

		public override void OnReload() => Log.Info($"{getName} has been Reloaded!");

		#region Events
		private void RegisterEvents()
		{
			RoundHandler = new RoundHandler(this);
			PlayerHandler = new PlayerHandler(this);

			EXILED.Events.WaitingForPlayersEvent += RoundHandler.OnWaitingForPlayers;
			EXILED.Events.RoundRestartEvent += RoundHandler.OnRoundRestart;

			EXILED.Events.ConsoleCommandEvent += PlayerHandler.OnConsoleCommand;
			EXILED.Events.RemoteAdminCommandEvent += PlayerHandler.OnRemoteAdminCommand;
			EXILED.Events.PlayerJoinEvent += PlayerHandler.OnPlayerJoin;
			EXILED.Events.PlayerLeaveEvent += PlayerHandler.OnPlayerLeave;
		}

		private void UnregisterEvents()
		{
			EXILED.Events.WaitingForPlayersEvent -= RoundHandler.OnWaitingForPlayers;
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
			ConsoleCommands.Add("help", new Help(this));

			RemoteAdminCommands.Add("chatmute", new Mute());
			RemoteAdminCommands.Add("chatunmute", new Unmute());
		}

		private void UnregisterCommands()
		{
			ConsoleCommands.Clear();
			RemoteAdminCommands.Clear();
		}
		#endregion
	}
}
