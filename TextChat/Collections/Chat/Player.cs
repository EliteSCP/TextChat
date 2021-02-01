namespace TextChat.Collections.Chat
{
    using System;
    public class Player
	{
		public string Id { get; set; }

		public string Authentication { get; set; }

		public string Name { get; set; }

		public DateTime LastMessageSentTimestamp { get; set; }

		public bool IsFlooding(float cooldown) => LastMessageSentTimestamp.AddMilliseconds(cooldown) > DateTime.Now;
	}
}
