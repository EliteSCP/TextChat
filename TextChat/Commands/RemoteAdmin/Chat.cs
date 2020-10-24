namespace TextChat.Commands.RemoteAdmin
{
    using CommandSystem;
    using System;
    using Localizations;

    public class Chat : ParentCommand
    {
        public Chat() => LoadGeneratedCommands();

        public override string Command => "chat";

        public override string[] Aliases => new[] { "c" };

        public override string Description => string.Empty;

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Mute.Mute());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = string.Format(Language.CommandSpecifySubCommand, "mute");
            return false;
        }
    }
}
