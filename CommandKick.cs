using System;
using Rocket.API.Commands;
using Rocket.API.Plugins;
using Rocket.API.User;
using Rocket.Core.Commands;
using Rocket.Core.I18N;

namespace fr34kyn01535.GlobalBan
{
    public class CommandKick : ICommand
    {
        private readonly GlobalBan _globalBanPlugin;

        public CommandKick(IPlugin plugin)
        {
            _globalBanPlugin = (GlobalBan)plugin;
        }

        public string Name => "kick";
        public string[] Aliases => null;
        public string Summary => "Kicks a player.";
        public string Description => null;
        public string Permission => null;
        public string Syntax => "<player> [reason]";
        public IChildCommand[] ChildCommands => null;

        public bool SupportsUser(Type user)
        {
            return true;
        }

        public void Execute(ICommandContext context)
        {
            if (context.Parameters.Length == 0 || context.Parameters.Length > 2)
            {
                throw new CommandWrongUsageException();
            }

            var globalUserManager = context.Container.Resolve<IUserManager>();
            IUser userToKick = context.Parameters.Get<IUser>(0);

            string reason = _globalBanPlugin.Translations.Get("command_kick_private_default_reason");
            if (context.Parameters.Length >= 2)
            {
                reason = context.Parameters.GetArgumentLine(1);
                globalUserManager.BroadcastLocalized(_globalBanPlugin.Translations, "command_kick_public_reason", userToKick.Name, reason);
            }
            else
            {
                globalUserManager.BroadcastLocalized(_globalBanPlugin.Translations, "command_kick_public", userToKick.Name);
            }

            userToKick.UserManager.Kick(userToKick, context.User, reason);
        }
    }
}
