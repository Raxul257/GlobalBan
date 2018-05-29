using System;
using Rocket.API.Commands;
using Rocket.API.Plugins;
using Rocket.API.User;
using Rocket.Core.Commands;
using Rocket.Core.I18N;
using Rocket.Core.User;

namespace fr34kyn01535.GlobalBan
{
    public class CommandUnban : ICommand
    {
        private readonly GlobalBan _globalBanPlugin;

        public CommandUnban(IPlugin plugin)
        {
            _globalBanPlugin = (GlobalBan)plugin;
        }

        public string Name => "unban";
        public string[] Aliases => null;
        public string Permission => null;
        public string Syntax => "<player>";
        public IChildCommand[] ChildCommands => null;
        public string Summary => "Unbans a player.";
        public string Description => null;

        public bool SupportsUser(Type user)
        {
            return true;
        }

        public void Execute(ICommandContext context)
        {
            if (context.Parameters.Length != 1)
            {
                throw new CommandWrongUsageException();
            }

            IUserManager globalUserManager = context.Container.Resolve<IUserManager>();
            IUserInfo target = context.Parameters.Get<IUserInfo>(0);

            DatabaseManager.UnbanResult name = _globalBanPlugin.Database.UnbanPlayer(target);


            if (!target.UserManager.Unban(target, context.User) && String.IsNullOrEmpty(name.Name))
            {
                context.User.SendLocalizedMessage(_globalBanPlugin.Translations, "command_generic_player_not_found");
                return;
            }

            globalUserManager.Broadcast(null, "The player " + name.Name + " was unbanned");
        }
    }
}
