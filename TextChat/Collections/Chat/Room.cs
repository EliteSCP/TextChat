namespace TextChat.Collections.Chat
{
    using Enums;
    using LiteDB;

    public class Room
    {
        [BsonCtor]
        public Room(ObjectId id, Message message, ChatRoomType type)
        {
            Id = id;
            Message = message;
            Type = type;
        }

        public Room(Message message, ChatRoomType type)
        {
            Id = ObjectId.NewObjectId();
            Message = message;
            Type = type;
        }

        public ObjectId Id { get; }

        public Message Message { get; }

        public ChatRoomType Type { get; }

        public void Save() => Database.LiteDatabase.GetCollection<Room>().Insert(this);
    }
}
