using System.Text;
using TextChat.Interfaces;
using TextChat.Localizations;

namespace TextChat.Commands.Console
{
	public class Help : ICommand
	{
		private readonly TextChat pluginInstance;

		public Help(TextChat pluginInstance) => this.pluginInstance = pluginInstance;

		public string Description => Language.HelpCommandDescription;

		public string Usage => Language.HelpCommandUsage;

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (pluginInstance.ConsoleCommands.Count == 0) return (Language.NoCommandsToShowError, "red");

			if (args.Length == 0)
			{
				StringBuilder commands = new StringBuilder($"\n\n[{string.Format(Language.ListOfCommands, pluginInstance.ConsoleCommands.Count)}]");

				foreach (ICommand command in pluginInstance.ConsoleCommands.Values)
				{
					commands.Append($"\n\n{command.Usage}\n\n{command.Description}");
				}

				return (commands.ToString(), "green");
			}
			else if (args.Length == 1)
			{
				if (!pluginInstance.ConsoleCommands.TryGetValue(args[0].Replace(".", ""), out ICommand command)) return (string.Format(Language.CommandNotFoundError, args[0]), "red");

				return ($"\n\n{command.Usage}\n\n{command.Description}", "green");
			}

			return (Language.CommandTooManyArgumentsError, "red");
		}
	}
}
