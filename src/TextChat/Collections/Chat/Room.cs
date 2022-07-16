namespace TextChat.Collections.Chat
{
    using Enums;
    using LiteDB;

    public class Room
    {
        public Room()
        {
        }

        public Room(Message message, ChatRoomType type)
        {
            Id = ObjectId.NewObjectId();
            Message = message;
            Type = type;
        }

        public ObjectId Id { get; private set; }

        public Message Message { get; private set; }

        public ChatRoomType Type { get; private set; }

        public void Save() => Database.LiteDatabase.GetCollection<Room>().Insert(this);
    }
}
