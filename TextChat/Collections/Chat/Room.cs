using LiteDB;
using TextChat.Enums;

namespace TextChat.Collections.Chat
{
	public class Room
	{
		public ObjectId Id { get; set; }
		public Message Message { get; set; }
		public ChatRoomType Type { get; set; }
	}
}
