using System;

namespace TextChat.Collections.Chat
{
	public class Player
	{
		public string Id { get; set; }
		public string Authentication { get; set; }
		public string Name { get; set; }
		public DateTime LastMessageSentTimestamp { get; set; }

		public bool IsFlooding(TimeSpan cooldown) => LastMessageSentTimestamp.Add(cooldown) > DateTime.Now;
	}
}
