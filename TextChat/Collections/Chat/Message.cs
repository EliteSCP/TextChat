namespace TextChat.Collections.Chat
{
    using Enums;
    using LiteDB;
    using Localizations;
    using System;
    using System.Collections.Generic;
    using static TextChat;

    public class Message
    {
        private string content;

        [BsonCtor]
        public Message(Player sender, List<Player> targets, string content, DateTime timestamp)
        {
            Sender = sender;
            Targets = targets;
            Content = content;
            Timestamp = timestamp;
        }

        public Message(Player sender, Player target, string content, DateTime timestamp)
        {
            Sender = sender;
            Targets = new List<Player>() { target };
            Content = content;
            Timestamp = timestamp;
        }

        public Player Sender { get; }

        public List<Player> Targets { get; }

        public string Content
        {
            get => content;
            set
            {
                content = Instance.Config.ShouldCensorBadWords ? value.Sanitize(Instance.Config.BadWords, Instance.Config.CensorBadWordsChar) : value;
            }
        }

        public DateTime Timestamp { get; }

        public bool IsValid(out string response)
        {
            if (string.IsNullOrEmpty(Content.Trim()))
            {
                response = Language.ChatMessageCannotBeEmptyError;
                return false;
            }
            else if (Sender.IsChatMuted())
            {
                response = Language.PlayerIsMutedError;
                return false;
            }
            else if (Sender.IsFlooding(Instance.Config.SlowModeCooldown))
            {
                response = Language.PlayerIsFloodingError;
                return false;
            }
            else if (Content.Length > Instance.Config.MaxMessageLength)
            {
                response = string.Format(Language.ChatMessageTooLongError, Instance.Config.MaxMessageLength);
                return false;
            }

            response = Content;
            return true;
        }

        public void Send(IEnumerable<Exiled.API.Features.Player> targets, string color)
        {
            targets.SendConsoleMessage(Content, color);

            Sender.LastMessageSentTimestamp = DateTime.Now;
        }

        public void Send(Exiled.API.Features.Player target, string color)
        {
            target.SendConsoleMessage(Content, color);

            Sender.LastMessageSentTimestamp = DateTime.Now;
        }

        public void Save(ChatRoomType type) => new Room(this, type).Save();
    }
}
