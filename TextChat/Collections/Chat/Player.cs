namespace TextChat.Collections.Chat
{
    using System;
    using static Database;

    public class Player
    {
        public string Id { get; set; }

        public string Authentication { get; set; }

        public string Name { get; set; }

        public DateTime LastMessageSentTimestamp { get; set; }

        public bool IsFlooding(float cooldown) => LastMessageSentTimestamp.AddMilliseconds(cooldown) > DateTime.Now;

        public bool IsChatMuted() => LiteDatabase.GetCollection<Mute>().Exists(mute => mute.Target.Id == Id && mute.Expire > DateTime.Now);
    }
}
