namespace TextChat.Collections.Chat
{
    using LiteDB;
    using System;

    public class Player
    {
        [BsonCtor]
        public Player(string id, string authentication, string name, DateTime lastMessageSentTimestamp)
        {
            Id = id;
            Authentication = authentication;
            Name = name;
            LastMessageSentTimestamp = lastMessageSentTimestamp;
        }

        public string Id { get; }

        public string Authentication { get; }

        public string Name { get; }

        public DateTime LastMessageSentTimestamp { get; set; }

        public bool IsFlooding(float cooldown) => LastMessageSentTimestamp.AddSeconds(cooldown) > DateTime.Now;

        public bool IsChatMuted() => Database.LiteDatabase.GetCollection<Mute>().Exists(mute => mute.Target.Id == Id && mute.Expire > DateTime.Now);

        public void Save() => Database.LiteDatabase.GetCollection<Player>().Upsert(this);
    }
}
