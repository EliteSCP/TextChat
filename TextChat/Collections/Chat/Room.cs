namespace TextChat.Collections.Chat
{
	using LiteDB;
	using Enums;
	public class Room
	{
		public ObjectId Id { get; set; }
		public Message Message { get; set; }
		public ChatRoomType Type { get; set; }
	}
}
