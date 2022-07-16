﻿namespace TextChat.Commands.RemoteAdmin.Mute
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using Localizations;
    using System;
    using System.Linq;

    public class Add : ICommand
    {
        private Add()
        {
        }

        public static Add Instance { get; } = new Add();

        public string Command { get; } = "add";

        public string[] Aliases { get; } = new[] { "a" };

        public string Description { get; } = Language.AddMuteCommandDescription;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("tc.mute.add"))
            {
                response = Language.CommandNotEnoughPermissionsError;
                return false;
            }

            if (arguments.Count < 2)
            {
                response = string.Format(Language.CommandNotEnoughParametersError, 3, Language.AddMuteCommandUsage);
                return false;
            }

            Player target = Player.Get(arguments.At(0));
            Collections.Chat.Player chatPlayer = arguments.At(0).GetChatPlayer();
            Collections.Chat.Player issuer = ((CommandSender)sender).GetStaffer();

            if (chatPlayer == null)
            {
                response = string.Format(Language.PlayerNotFoundError, arguments.At(0));
                return false;
            }

            if (!double.TryParse(arguments.At(1), out double duration) || duration < 1)
            {
                response = string.Format(Language.InvalidDurationError, arguments.At(1));
                return false;
            }

            string reason = string.Join(" ", arguments.Skip(2).Take(arguments.Count - 2));

            if (string.IsNullOrEmpty(reason))
            {
                response = Language.ReasonCannotBeEmptyError;
                return false;
            }

            if (chatPlayer.IsChatMuted())
            {
                response = string.Format(Language.PlayerIsAlreadyMutedError, chatPlayer.Name);
                return false;
            }

            new Collections.Chat.Mute(chatPlayer, issuer, reason, duration, DateTime.Now, DateTime.Now.AddMinutes(duration)).Save();

            if (TextChat.Instance.Config.ChatMutedBroadcast.Show)
            {
                target?.ClearBroadcasts();
                target?.Broadcast(TextChat.Instance.Config.ChatMutedBroadcast.Duration, string.Format(TextChat.Instance.Config.ChatMutedBroadcast.Content, duration, reason), TextChat.Instance.Config.ChatMutedBroadcast.Type);
            }

            target?.SendConsoleMessage(string.Format(Language.AddMuteCommandSuccessPlayer, duration, reason), "red");

            response = string.Format(Language.AddMuteCommandSuccessModerator, chatPlayer.Name, duration, reason);
            return true;
        }
    }
}
