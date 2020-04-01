using EXILED.Extensions;
using System;
using System.Collections.Generic;
using static TextChat.Database;

namespace TextChat.Extensions
{
	public static class ChatPlayer
	{
		/// <summary>
		/// Sends a console message to a List of <see cref="ReferenceHub"/>
		/// </summary>
		/// <param name="targets"></param>
		/// <param name="message"></param>
		/// <param name="color"></param>
		public static void SendConsoleMessage(this IEnumerable<ReferenceHub> targets, string message, string color)
		{
			foreach (ReferenceHub target in targets)
			{
				if (target != null) target.SendConsoleMessage(message, color);
			}
		}

		/// <summary>
		/// Gets the team color of a <see cref="ReferenceHub"/>
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string GetColor(this ReferenceHub player) => player.GetTeam().GetColor();

		/// <summary>
		/// Gets the team color of a <see cref="Team"/>
		/// </summary>
		/// <param name="team"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Gets the Authentication type of a <see cref="ReferenceHub"/>
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string GetAuthentication(this ReferenceHub player) => player.GetUserId().Split('@')[1];

		/// <summary>
		/// Gets the numeric ID of a <see cref="ReferenceHub"/>
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string GetRawUserId(this ReferenceHub player) => player.GetUserId().GetRawUserId();

		/// <summary>
		/// Gets the numer ID from a <see cref="string"/>
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string GetRawUserId(this string player) => player.Split('@')[0];

		/// <summary>
		/// Gets the mute status from the chat of a <see cref="ReferenceHub"/>
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool IsChatMuted(this ReferenceHub player)
		{
			return LiteDatabase.GetCollection<Collections.Chat.Mute>().Exists(mute => mute.Target.Id == player.GetRawUserId() && mute.Expire > DateTime.Now);
		}

		/// <summary>
		/// Gets the mute status from the chat of a <see cref="Collections.Chat.Player"/>
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool IsChatMuted(this Collections.Chat.Player player)
		{
			return LiteDatabase.GetCollection<Collections.Chat.Mute>().Exists(mute => mute.Target.Id == player.Id && mute.Expire > DateTime.Now);
		}

		/// <summary>
		/// Gets a <see cref="Collections.Chat.Player"/> from a <see cref="string"/>
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static Collections.Chat.Player GetChatPlayer(this string player)
		{
			return Player.GetPlayer(player)?.GetChatPlayer() ?? 
				LiteDatabase.GetCollection<Collections.Chat.Player>().FindOne(queryPlayer => queryPlayer.Id == player.GetRawUserId() || queryPlayer.Name == player);
		}

		/// <summary>
		/// Gets a <see cref="Collections.Chat.Player"/> from a <see cref="ReferenceHub"/>
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static Collections.Chat.Player GetChatPlayer(this ReferenceHub player)
		{
			if (string.IsNullOrEmpty(player?.GetUserId())) return null;

			if (ChatPlayers.TryGetValue(player, out Collections.Chat.Player chatPlayer)) return chatPlayer;
			else return LiteDatabase.GetCollection<Collections.Chat.Player>().FindOne(queryPlayer => queryPlayer.Id == player.GetRawUserId());
		}

		/// <summary>
		/// Gets a List of <see cref="Collections.Chat.Player"/> from a List of <see cref="ReferenceHub"/>
		/// </summary>
		/// <param name="players"></param>
		/// <returns></returns>
		public static List<Collections.Chat.Player> GetChatPlayers(this IEnumerable<ReferenceHub> players)
		{
			List<Collections.Chat.Player> chatPlayersList = new List<Collections.Chat.Player>();

			foreach (ReferenceHub player in players)
			{
				Collections.Chat.Player chatPlayer = player.GetChatPlayer();

				if (chatPlayer != null) chatPlayersList.Add(chatPlayer);
			}

			return chatPlayersList;
		}
	}
}
