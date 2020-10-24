namespace TextChat.Extensions
{
    using Exiled.API.Features;
    using System;
	using System.Collections.Generic;
    using System.Linq;
    using static Database;

	public static class ChatPlayer
	{
		/// <summary>
		/// Sends a console message to a List of <see cref="Player"/>
		/// </summary>
		/// <param name="targets"></param>
		/// <param name="message"></param>
		/// <param name="color"></param>
		public static void SendConsoleMessage(this IEnumerable<Player> targets, string message, string color)
		{
			foreach (Player target in targets)
			{
				if (target != null) target.SendConsoleMessage(message, color);
			}
		}

		/// <summary>
		/// Gets the team color of a <see cref="Player"/>
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static string GetColor(this Player player) => player.Team.GetColor();

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
		/// Gets the numeric ID from a <see cref="string"/>
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public static string GetRawUserId(this string userId)
		{
			int index = userId.LastIndexOf('@');

			if (index == -1)
				return userId;

			return userId.Substring(0, index);
		}

		/// <summary>
		/// Gets the mute status from the chat of a <see cref="Player"/>
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool IsChatMuted(this Player player)
		{
			return LiteDatabase.GetCollection<Collections.Chat.Mute>().Exists(mute => mute.Target.Id == player.UserId && mute.Expire > DateTime.Now);
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
			return Player.Get(player)?.GetChatPlayer() ?? 
				LiteDatabase.GetCollection<Collections.Chat.Player>().FindOne(queryPlayer => queryPlayer.Id == player.GetRawUserId() || queryPlayer.Name == player);
		}

		/// <summary>
		/// Gets a <see cref="Collections.Chat.Player"/> from a <see cref="ReferenceHub"/>
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public static Collections.Chat.Player GetChatPlayer(this Player player)
		{
			if (player == null || (string.IsNullOrEmpty(player.UserId) && !player.IsHost)) return null;
			else if (player.IsHost) return ServerChatPlayer;
			else if (ChatPlayers.TryGetValue(player, out Collections.Chat.Player chatPlayer)) return chatPlayer;
			else return LiteDatabase.GetCollection<Collections.Chat.Player>().FindOne(queryPlayer => queryPlayer.Id == player.RawUserId);
		}

		/// <summary>
		/// Gets a List of <see cref="Collections.Chat.Player"/> from a List of <see cref="ReferenceHub"/>
		/// </summary>
		/// <param name="players"></param>
		/// <returns></returns>
		public static IEnumerable<Collections.Chat.Player> GetChatPlayers(this IEnumerable<Player> players)
		{
			return players.Select(player => player?.GetChatPlayer()).Where(player => player != null);
		}
	}
}
