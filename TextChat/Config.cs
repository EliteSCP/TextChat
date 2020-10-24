namespace TextChat
{
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;

    public class Config : IConfig
	{
		private string language;

		[Description("Indicates whether the plugin is enabled or not")]
		public bool IsEnabled { get; set; } = true;

		[Description("The database name")]
		public string DatabaseName { get; private set; } = "TextChat";

		[Description("The general chat color")]
		public string GeneralChatColor { get; private set; } = "cyan";

		[Description("The private message color")]
		public string PrivateMessageColor { get; private set; } = "magenta";

		[Description("The maximum length of a single message")]
		public ushort MaxMessageLength { get; private set; } = 75;

		[Description("Indicates whether bad words have to be censored or not")]
		public bool ShouldCensorBadWords { get; private set; }

		[Description("The character that would replace bad words")]
		public char CensorBadWordsChar { get; private set; } = '*';

		[Description("The list of bad words")]
		public List<string> BadWords { get; private set; } = new List<string>();

		[Description("Indicates whether the chat have to be saved into the database or not")]
		public bool ShouldSaveChatToDatabase { get; private set; } = true;

		[Description("Indicates whether spectators can send messages to alive players or not")]
		public bool CanSpectatorSendMessagesToAlive { get; private set; }

		[Description("The broadcast shown to muted players")]
		public Broadcast ChatMutedBroadcast { get; private set; } = new Broadcast();

		[Description("The broadcast which warns you about a new private message")]
		public Broadcast PrivateMessageNotificationBroadcast { get; private set; } = new Broadcast("", 6);

		[Description("Indicates whether the slow mode is enabled or not")]
		public bool IsSlowModeEnabled { get; private set; } = true;

		[Description("The slowmode cooldown, in seconds")]
		public ushort SlowModeCooldown { get; private set; }

		[Description("The plugin language")]
		public string Language
		{
			get => language;
			private set
			{
				Localizations.Language.Culture = CultureInfo.GetCultureInfo(value) ?? CultureInfo.GetCultureInfo("");

				if (string.IsNullOrEmpty(ChatMutedBroadcast?.Content)) ChatMutedBroadcast.Content = Localizations.Language.PlayerHasBeenChatMuted;
				if (string.IsNullOrEmpty(PrivateMessageNotificationBroadcast?.Content)) PrivateMessageNotificationBroadcast.Content = Localizations.Language.PlayerReceivedPrivateMessage;

				language = value;
			}
		}
	}
}
