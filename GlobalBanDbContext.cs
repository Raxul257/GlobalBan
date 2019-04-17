using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fr34kyn01535.GlobalBan.Models;
using Microsoft.EntityFrameworkCore;

namespace fr34kyn01535.GlobalBan
{
    public class GlobalBanDbContext : DbContext
    {
        public DbSet<Ban> Bans { get; set; }
        private readonly string _connectionString;

        public GlobalBanDbContext()
        {
            _connectionString = "SERVER=localhost;DATABASE=unturned;UID=root;PASSWORD=;PORT=3306;charset=utf8";
        }

        public GlobalBanDbContext(GlobalBanPlugin plugin)
        {
            _connectionString = plugin.ConfigurationInstance.MySqlConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString);
        }
    }
}
