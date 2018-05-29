using System;
using Rocket.API;

namespace fr34kyn01535.GlobalBan
{
    public class GlobalBanConfiguration
    {
        public string DatabaseAddress { get; set; } = "localhost";
        public string DatabaseUsername { get; set; } = "unturned";
        public string DatabasePassword { get; set; } = "password";
        public string DatabaseName { get; set; } = "unturned";
        public string DatabaseTableName { get; set; } = "banlist";
        public int DatabasePort { get; set; } = 3306;
    }
}
