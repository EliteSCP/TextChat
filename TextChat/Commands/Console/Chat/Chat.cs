namespace TextChat.Commands.Console.Chat
{
    using CommandSystem;
    using Localizations;
    using System;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class Chat : ParentCommand
    {
        public Chat() => LoadGeneratedCommands();

        public override string Command { get; } = "chat";

        public override string[] Aliases { get; } = new[] { "c" };

        public override string Description { get; } = string.Empty;

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Public());
            RegisterCommand(new Private());
            RegisterCommand(new Team());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = string.Format(Language.CommandSpecifySubCommand, "public, private, team, help");
            return false;
        }
    }
}
