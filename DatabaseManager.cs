using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System;
using System.Text.RegularExpressions;
using Rocket.API.User;

namespace fr34kyn01535.GlobalBan
{
    public class DatabaseManager
    {
        private readonly GlobalBan _globalBanPlugin;

        public DatabaseManager(GlobalBan globalBanPlugin)
        {
            _globalBanPlugin = globalBanPlugin;
            new I18N.West.CP1250();
            CheckSchema();
        }

        private MySqlConnection createConnection()
        {
            MySqlConnection connection = null;

            if (_globalBanPlugin.ConfigurationInstance.DatabasePort == 0)

                _globalBanPlugin.ConfigurationInstance.DatabasePort = 3306;
            connection = new MySqlConnection(
                $"SERVER={_globalBanPlugin.ConfigurationInstance.DatabaseAddress};DATABASE={_globalBanPlugin.ConfigurationInstance.DatabaseName};UID={_globalBanPlugin.ConfigurationInstance.DatabaseUsername};PASSWORD={_globalBanPlugin.ConfigurationInstance.DatabasePassword};PORT={_globalBanPlugin.ConfigurationInstance.DatabasePort};");

            return connection;
        }

        public bool IsBanned(IIdentity player)
        {

            MySqlConnection connection = createConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select 1 from `" + _globalBanPlugin.ConfigurationInstance.DatabaseTableName + "` where `player_id` = '" + player.Id + "' and (banDuration is null or ((banDuration + UNIX_TIMESTAMP(banTime)) > UNIX_TIMESTAMP()));";
            connection.Open();
            object result = command.ExecuteScalar();
            if (result != null) return true;
            connection.Close();

            return false;
        }

        public class Ban
        {
            public int Duration;
            public DateTime Time;
            public string Admin;
        }


        public Ban GetBan(string playerId)
        {
            MySqlConnection connection = createConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select  `banDuration`,`banTime`,`admin` from `" + _globalBanPlugin.ConfigurationInstance.DatabaseTableName + "` where `player_id` = '" + playerId + "' and (banDuration is null or ((banDuration + UNIX_TIMESTAMP(banTime)) > UNIX_TIMESTAMP()));";
            connection.Open();
            MySqlDataReader result = command.ExecuteReader(System.Data.CommandBehavior.SingleRow);
            if (result != null && result.Read() && result.HasRows) return new Ban()
            {
                Duration = result["banDuration"] == DBNull.Value ? -1 : result.GetInt32("banDuration"),
                Time = (DateTime)result["banTime"],
                Admin = (result["admin"] == DBNull.Value || result["admin"].ToString() == "Console") ? "Console" : (string)result["admin"]
            };
            connection.Close();
            return null;
        }

        public void CheckSchema()
        {
            MySqlConnection connection = createConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "show tables like '" + _globalBanPlugin.ConfigurationInstance.DatabaseTableName + "'";
            connection.Open();
            object test = command.ExecuteScalar();

            if (test == null)
            {
                command.CommandText = "CREATE TABLE `" + _globalBanPlugin.ConfigurationInstance.DatabaseTableName + "` (`id` int(11) NOT NULL AUTO_INCREMENT,`player_id` varchar(32) NOT NULL,`admin` varchar(32) NOT NULL,`banMessage` varchar(512) DEFAULT NULL,`name` varchar(255) DEFAULT NULL,`banDuration` int NULL,`banTime` timestamp NULL ON UPDATE CURRENT_TIMESTAMP,PRIMARY KEY (`id`));";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public void BanPlayer(IIdentity toBan, IIdentity bannedBy, string banMessage, int duration)
        {
            MySqlConnection connection = createConnection();
            MySqlCommand command = connection.CreateCommand();
            if (banMessage == null) banMessage = "";
            command.Parameters.AddWithValue("@id", toBan.Id);
            command.Parameters.AddWithValue("@admin", bannedBy.Name);
            command.Parameters.AddWithValue("@name", toBan.Name);
            command.Parameters.AddWithValue("@banMessage", banMessage);
            if (duration == 0)
            {
                command.Parameters.AddWithValue("@banDuration", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@banDuration", duration);
            }
            command.CommandText = "insert into `" + _globalBanPlugin.ConfigurationInstance.DatabaseTableName + "` (`player_id`,`admin`,`banMessage`,`name`,`banTime`,`banDuration`) values(@id,@admin,@banMessage,@name,now(),@banDuration);";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public class UnbanResult
        {
            public string Id;
            public string Name;
        }

        public UnbanResult UnbanPlayer(IIdentity player)
        {
            MySqlConnection connection = createConnection();

            MySqlCommand command = connection.CreateCommand();
            command.Parameters.AddWithValue("@player", "%" + player.Id + "%");
            command.CommandText = "select id,name from `" + _globalBanPlugin.ConfigurationInstance.DatabaseTableName + "` where `player_id` like @player or `name` like @player limit 1;";
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                string playerId = reader.GetString(0);
                string playerName = reader.GetString(1);
                connection.Close();
                command = connection.CreateCommand();
                command.Parameters.AddWithValue("@id", playerId);
                command.CommandText = "delete from `" + _globalBanPlugin.ConfigurationInstance.DatabaseTableName + "` where `player_id` = @id;";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return new UnbanResult { Id = playerId, Name = playerName };
            }

            return null;
        }
    }
}
