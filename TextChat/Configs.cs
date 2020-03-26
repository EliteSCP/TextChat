using EXILED;
using System;
using System.Collections.Generic;

namespace TextChat
{
	internal static class Configs
	{
		public static bool isEnabled;
		public static string databaseName;
		public static string generalChatColor;
		public static string privateMessageColor;
		public static int maxMessageLength;
		public static bool censorBadWords;
		public static char censorBadWordsChar;
		public static List<string> badWords;
		public static bool saveChatToDatabase;
		public static bool canSpectatorSendMessagesToAlive;
		public static bool showChatMutedBroadcast;
		public static uint chatMutedBroadcastDuration;
		public static bool showPrivateMessageNotificationBroadcast;
		public static uint privateMessageNotificationBroadcastDuration;
		public static bool isSlowModeEnabled;
		public static TimeSpan slowModeCooldown;

		public static void Reload()
		{
			isEnabled = Plugin.Config.GetBool("tc_enabled", true);
			databaseName = Plugin.Config.GetString("tc_database_name", "TextChat");
			generalChatColor = Plugin.Config.GetString("tc_general_chat_color", "cyan");
			privateMessageColor = Plugin.Config.GetString("tc_private_message_color", "magenta");
			maxMessageLength = Plugin.Config.GetInt("tc_max_message_length", 75);
			censorBadWords = Plugin.Config.GetBool("tc_censor_bad_words");
			censorBadWordsChar = Plugin.Config.GetChar("tc_censor_bad_words_char", '*');
			badWords = Plugin.Config.GetStringList("tc_bad_words");
			saveChatToDatabase = Plugin.Config.GetBool("tc_save_chat_to_database", true);
			canSpectatorSendMessagesToAlive = Plugin.Config.GetBool("tc_can_spectator_send_messages_to_alive");
			showChatMutedBroadcast = Plugin.Config.GetBool("tc_show_chat_muted_broadcast", true);
			chatMutedBroadcastDuration = Plugin.Config.GetUInt("tc_chat_muted_broadcast_duration", 10);
			showPrivateMessageNotificationBroadcast = Plugin.Config.GetBool("tc_show_private_message_notification_broadcast");
			privateMessageNotificationBroadcastDuration = Plugin.Config.GetUInt("tc_private_message_notification_broadcast_duration", 6);
			isSlowModeEnabled = Plugin.Config.GetBool("tc_is_slow_mode_enabled", true);
			slowModeCooldown = new TimeSpan(0, 0, 0, 0, (int)(Plugin.Config.GetFloat("tc_slow_mode_interval", 1f) * 1000));
		}
	}
}
