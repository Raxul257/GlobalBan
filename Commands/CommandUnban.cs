using System;
using System.Threading.Tasks;
using Rocket.API.Commands;
using Rocket.API.Plugins;
using Rocket.API.User;
using Rocket.Core.Commands;

namespace fr34kyn01535.GlobalBan.Commands
{
    public class CommandUnban : ICommand
    {
        public string Name => "unban";
        public string[] Aliases => null;
        public string Syntax => "<player>";
        public string Summary => "Unbans a player.";
        public string Description => null;
        public bool SupportsUser(IUser user) => true;
        public IChildCommand[] ChildCommands => null;

        private readonly GlobalBanPlugin _globalBanPluginPlugin;

        public CommandUnban(IPlugin plugin)
        {
            _globalBanPluginPlugin = (GlobalBanPlugin)plugin;
        }

        public async Task ExecuteAsync(ICommandContext context)
        {
            //if (context.Parameters.Length != 1)
            //{
            //    throw new CommandWrongUsageException();
            //}

            //IUserManager globalUserManager = context.Container.Resolve<IUserManager>();
            //IUserInfo target = context.Parameters.Get<IUserInfo>(0);

            //DatabaseManager.UnbanResult name = _globalBanPluginPlugin.Database.UnbanPlayer(target);


            //if (!target.UserManager.Unban(target, context.User) && String.IsNullOrEmpty(name.Name))
            //{
            //    context.User.SendLocalizedMessage(_globalBanPluginPlugin.Translations, "command_generic_player_not_found");
            //    return;
            //}

            //globalUserManager.Broadcast(null, "The player " + name.Name + " was unbanned");
        }
    }
}
