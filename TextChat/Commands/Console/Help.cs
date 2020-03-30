using System.Text;
using TextChat.Interfaces;

namespace TextChat.Commands.Console
{
	public class Help : ICommand
	{
		private readonly TextChat pluginInstance;

		public Help(TextChat pluginInstance) => this.pluginInstance = pluginInstance;

		public string Description => "Gets a list of commands or the description of a single command.";

		public string Usage => ".help/.help [Command Name]";

		public (string response, string color) OnCall(ReferenceHub sender, string[] args)
		{
			if (pluginInstance.ConsoleCommands.Count == 0) return ("There are no commands to show!", "red");

			if (args.Length == 0)
			{
				StringBuilder commands = new StringBuilder($"\n\n[LIST OF COMMANDS ({pluginInstance.ConsoleCommands.Count})]");

				foreach (ICommand command in pluginInstance.ConsoleCommands.Values)
				{
					commands.Append($"\n\n{command.Usage}\n\n{command.Description}");
				}

				return (commands.ToString(), "green");
			}
			else if (args.Length == 1)
			{
				if (!pluginInstance.ConsoleCommands.TryGetValue(args[0].Replace(".", ""), out ICommand command)) return ($"Command \"{args[0]}\" was not found!", "red");

				return ($"\n\n{command.Usage}\n\n{command.Description}", "green");
			}

			return ("Too many arguments!", "red");
		}
	}
}
