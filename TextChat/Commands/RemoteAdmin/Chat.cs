namespace TextChat.Commands.RemoteAdmin
{
    using CommandSystem;
    using Localizations;
    using System;

    public class Chat : ParentCommand
    {
        public Chat() => LoadGeneratedCommands();

        public override string Command { get; } = "chat";

        public override string[] Aliases { get; } = new[] { "c" };

        public override string Description { get; } = string.Empty;

        public override void LoadGeneratedCommands() => RegisterCommand(new Mute.Mute());

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = string.Format(Language.CommandSpecifySubCommand, "mute");
            return false;
        }
    }
}
