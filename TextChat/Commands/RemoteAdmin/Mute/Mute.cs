namespace TextChat.Commands.RemoteAdmin.Mute
{
    using CommandSystem;
    using Localizations;
    using System;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Mute : ParentCommand
	{
		public Mute() => LoadGeneratedCommands();

		public override string Description => Language.AddMuteCommandDescription;

		public override string Command => "mute";

		public override string[] Aliases => new[] { "mu" };

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Add());
            RegisterCommand(new Remove());
            RegisterCommand(new Show());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = string.Format(Language.CommandSpecifySubCommand, "mute");
            return false;
		}
    }
}
