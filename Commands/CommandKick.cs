using System.Threading.Tasks;
using Rocket.API.Commands;
using Rocket.API.Plugins;
using Rocket.API.User;
using Rocket.Core.Commands;

namespace fr34kyn01535.GlobalBan.Commands
{
    public class CommandKick : ICommand
    {
        public string Name => "kick";
        public string[] Aliases => null;
        public string Syntax => "<player> [reason]";
        public string Summary => "Kicks a player.";
        public string Description => null;
        public bool SupportsUser(IUser user) => true;
        public IChildCommand[] ChildCommands => null;

        private readonly GlobalBanPlugin _globalBanPluginPlugin;

        public CommandKick(IPlugin plugin)
        {
            _globalBanPluginPlugin = (GlobalBanPlugin)plugin;
        }

        public async Task ExecuteAsync(ICommandContext context)
        {
            if (context.Parameters.Length == 0 || context.Parameters.Length > 2)
            {
                throw new CommandWrongUsageException();
            }

            var globalUserManager = context.Container.Resolve<IUserManager>();
            IUser userToKick = context.Parameters.Get<IUser>(0);

            string reason = _globalBanPluginPlugin.Translations.Get("command_kick_private_default_reason");
            if (context.Parameters.Length >= 2)
            {
                reason = context.Parameters.GetArgumentLine(1);
                globalUserManager.BroadcastLocalized(_globalBanPluginPlugin.Translations, "command_kick_public_reason", userToKick.Name, reason);
            }
            else
            {
                globalUserManager.BroadcastLocalized(_globalBanPluginPlugin.Translations, "command_kick_public", userToKick.Name);
            }

            userToKick.UserManager.Kick(userToKick, context.User, reason);
        }
    }
}
