using System;

namespace TextChat.Collections.Chat
{
	public class Player
	{
		public string Id { get; set; }
		public string Authentication { get; set; }
		public string Name { get; set; }

		public DateTime lastMessageSentTimestamp;

		public bool IsFlooding(TimeSpan cooldown) => lastMessageSentTimestamp.Add(cooldown) > DateTime.Now;
	}
}
