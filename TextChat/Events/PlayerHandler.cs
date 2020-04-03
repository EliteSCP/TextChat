using EXILED;
using EXILED.Extensions;
using System;
using System.Linq;
using TextChat.Extensions;
using TextChat.Interfaces;
using TextChat.Localizations;
using static TextChat.Database;

namespace TextChat.Events
{
	public class PlayerHandler
	{
		private readonly TextChat pluginInstance;

		public PlayerHandler(TextChat pluginInstance) => this.pluginInstance = pluginInstance;

		public void OnConsoleCommand(ConsoleCommandEvent ev)
		{
			if (ev.Player == null) return;

			if (ev.Player.gameObject == PlayerManager.localPlayer)
			{
				ev.ReturnMessage = Language.CommandNotAllowedForConsoleError;
				ev.Color = "red";

				return;
			}

			(string commandName, string[] commandArguments) = ev.Command.ExtractCommand();

			if (!pluginInstance.ConsoleCommands.TryGetValue(commandName, out ICommand command)) return;

			try
			{
				(string response, string color) = command.OnCall(ev.Player, commandArguments);

				ev.ReturnMessage = response;
				ev.Color = color;
			}
			catch (Exception exception)
			{
				Log.Error(string.Format(Language.CommandException, commandName, exception));
				ev.ReturnMessage = Language.CommandError;
				ev.Color = "red";
			}
		}

		public void OnRemoteAdminCommand(ref RACommandEvent ev)
		{
			(string commandName, string[] commandArguments) = ev.Command.ExtractCommand();

			if (!pluginInstance.RemoteAdminCommands.TryGetValue(commandName, out ICommand command)) return;

			try
			{
				ReferenceHub sender;

				if (ev.Sender.SenderId == "GAME CONSOLE") ReferenceHub.Hubs.TryGetValue(PlayerManager.localPlayer, out sender);
				else sender = Player.GetPlayer(ev.Sender.SenderId);

				if (sender == null) return;

				(string response, string color) = command.OnCall(sender, commandArguments);

				ev.Sender.RAMessage($"<color={color}>{response}</color>", color == "green");
				ev.Allow = false;
			}
			catch (Exception exception)
			{
				Log.Error(string.Format(Language.CommandException, commandName, exception));
				ev.Sender.RAMessage(Language.CommandError, false);
				ev.Allow = false;
			}
		}

		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			ChatPlayers.Add(ev.Player, ev.Player.GetChatPlayer() ?? new Collections.Chat.Player()
			{
				Id = ev.Player.GetRawUserId(),
				Authentication = ev.Player.GetAuthentication(),
				Name = ev.Player.GetNickname()
			});

			ev.Player.SendConsoleMessage(Language.ChatWelcome, "green");

			Player.GetHubs().Where(player => player != ev.Player).SendConsoleMessage(string.Format(Language.PlayerHasJoinedTheChat, ev.Player.GetNickname()), "green");
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player?.GetUserId())) return;

			Player.GetHubs().Where(player => player != ev.Player).SendConsoleMessage(string.Format(Language.PlayerHasLeftTheChat, ev.Player.GetNickname()), "red");

			LiteDatabase.GetCollection<Collections.Chat.Player>().Upsert(ev.Player.GetChatPlayer());

			ChatPlayers.Remove(ev.Player);
		}
	}
}
