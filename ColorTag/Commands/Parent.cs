using CommandSystem;
using System;

namespace ColorTag.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class Parent : ParentCommand
    {
        public Parent() => LoadGeneratedCommands();

        public override string Command { get; } = "colortag";
        public override string[] Aliases { get; } = { };
        public override string Description { get; } = "Add colors to tag";

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new ColorAdd());
            RegisterCommand(new ColorCheck());
            RegisterCommand(new ColorDelete());
            RegisterCommand(new ColorRemove());
            RegisterCommand(new ColorSet());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Usage: colortag (set/add/remove/check/delete)";
            return false;
        }
    }
}
