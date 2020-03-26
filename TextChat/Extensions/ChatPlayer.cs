using EXILED.Extensions;
using System;
using System.Collections.Generic;
using static TextChat.Database;

namespace TextChat.Extensions
{
	public static class ChatPlayer
	{
		public static void SendConsoleMessage(this ReferenceHub player, string message, string color)
		{
			player.characterClassManager.TargetConsolePrint(player.scp079PlayerScript.connectionToClient, message, color);
		}

		public static void SendConsoleMessage(this IEnumerable<ReferenceHub> targets, string message, string color)
		{
			foreach (ReferenceHub target in targets)
			{
				if (target != null) target.SendConsoleMessage(message, color);
			}
		}

		public static string GetColor(this ReferenceHub player) => player.GetTeam().GetColor();

		public static string GetColor(this Team team)
		{
			switch (team)
			{
				case Team.SCP:
					return "red";
				case Team.MTF:
					return "blue";
				case Team.CHI:
					return "green";
				case Team.RSC:
					return "yellow";
				case Team.CDP:
					return "yellow";
				case Team.TUT:
					return "green";
				case Team.RIP:
				default:
					return "white";
			}
		}

		public static string GetAuthentication(this ReferenceHub player) => player.GetUserId().Split('@')[1];

		public static string GetRawUserId(this ReferenceHub player) => player.GetUserId().Split('@')[0];

		public static bool IsChatMuted(this ReferenceHub player)
		{
			return LiteDatabase.GetCollection<Collections.Chat.Mute>().Exists(mute => mute.Target.Id == player.GetRawUserId() && mute.Expire > DateTime.Now);
		}

		public static List<Collections.Chat.Player> GetChatPlayers(this IEnumerable<ReferenceHub> players, Dictionary<ReferenceHub, Collections.Chat.Player> chatPlayers)
		{
			List<Collections.Chat.Player> chatPlayersList = new List<Collections.Chat.Player>();

			foreach (ReferenceHub player in players)
			{
				if (player != null && chatPlayers.TryGetValue(player, out Collections.Chat.Player chatPlayer))
				{
					chatPlayersList.Add(chatPlayer);
				}
			}

			return chatPlayersList;
		}
	}
}
