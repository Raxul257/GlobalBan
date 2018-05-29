using Rocket.Core.Plugins;
using System.Collections.Generic;
using Rocket.API.DependencyInjection;
using Rocket.API.Eventing;
using Rocket.Core.Player.Events;

namespace fr34kyn01535.GlobalBan
{
    public class GlobalBan : Plugin<GlobalBanConfiguration>, IEventListener<PlayerPreConnectEvent>
    {
        public GlobalBan(IDependencyContainer container) : base("GlobalBan", container)
        {
        }

        public DatabaseManager Database;

        public static Dictionary<string, string> Players = new Dictionary<string, string>();

        protected override void OnLoad(bool isFromReload)
        {
            Database = new DatabaseManager(this);
        }

        public override Dictionary<string, string> DefaultTranslations => new Dictionary<string, string>
        {
            {"default_banmessage","you were banned by {0} on {1} for {2} seconds, contact the staff if you feel this is a mistake."},
            {"command_generic_invalid_parameter","Invalid parameter"},
            {"command_generic_player_not_found","Player not found"},
            {"command_ban_public_reason", "The player {0} was banned for: {1}"},
            {"command_ban_public","The player {0} was banned"},
            {"command_ban_private_default_reason","you were banned from the server"},
            {"command_kick_public_reason", "The player {0} was kicked for: {1}"},
            {"command_kick_public","The player {0} was kicked"},
            {"command_kick_private_default_reason","you were kicked from the server"},
        };

        public static KeyValuePair<string, string>? GetPlayer(string search)
        {
            foreach (KeyValuePair<string, string> pair in Players)
            {
                if (pair.Key.ToLower().Contains(search.ToLower()) || pair.Value.ToLower().Contains(search.ToLower()))
                {
                    return pair;
                }
            }

            return null;
        }

        public void HandleEvent(IEventEmitter emitter, PlayerPreConnectEvent @event)
        {
            if (Database.IsBanned(@event.Player))
            {
                @event.RejectionReason = "You have been banned.";
                @event.IsCancelled = true;
            }
        }
    }
}
