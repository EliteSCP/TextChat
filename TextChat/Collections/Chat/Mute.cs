namespace TextChat.Collections.Chat
{
    using LiteDB;
    using System;

    public class Mute
    {
        public Mute()
        {
        }

        public Mute(Player target, Player issuer, string reason, double duration, DateTime timestamp, DateTime expire) 
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            Duration = duration;
            Timestamp = timestamp;
            Expire = expire;
        }

        public ObjectId Id { get; private set; }

        public Player Target { get; private set; }

        public Player Issuer { get; private set; }

        public string Reason { get; private set; }

        public double Duration { get; private set; }

        public DateTime Timestamp { get; private set; }

        public DateTime Expire { get; private set; }

        public void Save() => Database.LiteDatabase.GetCollection<Mute>().Insert(this);
    }
}
