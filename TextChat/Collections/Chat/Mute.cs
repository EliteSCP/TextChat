namespace TextChat.Collections.Chat
{
	using LiteDB;
	using System;

	public class Mute
	{
		public ObjectId Id { get; set; }
		public Player Target { get; set; }
		public Player Issuer { get; set; }
		public string Reason { get; set; }
		public double Duration { get; set; }
		public DateTime Timestamp { get; set; }
		public DateTime Expire { get; set; }
	}
}
