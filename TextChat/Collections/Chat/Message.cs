namespace TextChat.Collections.Chat
{
    using System;
    using System.Collections.Generic;

    public class Message
	{
		public Player Sender { get; set; }

		public List<Player> Targets { get; set; }

		public string Content { get; set; }

		public DateTime Timestamp { get; set; }
	}
}
