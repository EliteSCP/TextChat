namespace TextChat.Commands.RemoteAdmin.Mute
{
    using CommandSystem;
    using Localizations;
    using System;

    public class Mute : ParentCommand
    {
        public Mute() => LoadGeneratedCommands();

        public override string Description { get; } = Language.AddMuteCommandDescription;

        public override string Command { get; } = "mute";

        public override string[] Aliases { get; } = new[] { "m", "mu" };

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(Add.Instance);
            RegisterCommand(Remove.Instance);
            RegisterCommand(Show.Instance);
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = string.Format(Language.CommandSpecifySubCommand, "add, remove, show");
            return false;
        }
    }
}
