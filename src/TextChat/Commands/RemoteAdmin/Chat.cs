﻿namespace TextChat.Commands.RemoteAdmin
{
    using CommandSystem;
    using Localizations;
    using System;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Chat : ParentCommand
    {
        public Chat() => LoadGeneratedCommands();

        public override string Command { get; } = "textchat";

        public override string[] Aliases { get; } = new[] { "chat", "c", "tc" };

        public override string Description { get; } = string.Empty;

        public override void LoadGeneratedCommands() => RegisterCommand(new Mute.Mute());

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = string.Format(Language.CommandSpecifySubCommand, "mute");
            return false;
        }
    }
}
