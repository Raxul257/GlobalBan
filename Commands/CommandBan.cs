using System;
using System.Threading.Tasks;
using Rocket.API.Commands;
using Rocket.API.Plugins;
using Rocket.API.User;
using Rocket.Core.Commands;

namespace fr34kyn01535.GlobalBan.Commands
{
    public class CommandBan : ICommand
    {
        public string Name => "ban";
        public string[] Aliases => null;
        public string Syntax => "<player> [reason] [duration]";
        public string Summary => "Bans a player.";
        public string Description => null;
        public bool SupportsUser(IUser user) => true;
        public IChildCommand[] ChildCommands => null;

        private readonly GlobalBanPlugin _plugin;

        public CommandBan(IPlugin plugin)
        {
            _plugin = (GlobalBanPlugin) plugin;
        }

        public async Task ExecuteAsync(ICommandContext context)
        {
            if (context.Parameters.Length == 0 || context.Parameters.Length > 3)
            {
                throw new CommandWrongUsageException();
            }

            IUserManager globalUserManager = context.Container.Resolve<IUserManager>();

            IUserInfo toBan = context.Parameters.Get<IUserInfo>(0);
            IUser toBanUser = toBan.UserManager.OnlineUsers.FirstOrDefault(c =>
                string.Equals(c.Id, toBan.Id, StringComparison.OrdinalIgnoreCase));

            string reason = _plugin.Translations.Get("command_ban_private_default_reason");
            bool hasPublicReason = false;
            if (context.Parameters.Length > 1 )
            {
                reason = context.Parameters.Get<string>(1);
                hasPublicReason = true;
            }

            int duration = 0;
            if (context.Parameters.Length > 2)
            {
                duration = context.Parameters.Get<int>(2);
            }

            if (hasPublicReason)
            {
                globalUserManager.BroadcastLocalized(_plugin.Translations, "command_ban_public_reason", toBan.Name, reason);

            }
            else
            {
                globalUserManager.BroadcastLocalized(_plugin.Translations, "command_ban_public", toBan.Name);
            }

            _plugin.Database.BanPlayer(toBan, context.User, reason, duration);
            toBanUser?.UserManager.Kick(toBanUser, context.User, reason);
        }
    }
}
