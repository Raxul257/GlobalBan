using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fr34kyn01535.GlobalBan.Models
{
    [Table("bans")]
    public class Ban
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(32)")]
        public string PlayerId { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string PlayerName { get; set; }

        [Column(TypeName = "varchar(32)")]
        public string AdminId { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string AdminName { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Reason { get; set; }

        public int Duration { get; set; }

        public DateTime BanTime { get; set; } = DateTime.Now;

        public Ban(string playerId, string playerName, string adminId, string adminName, string reason, int duration)
        {
            PlayerId = playerId;
            PlayerName = playerName;

            AdminId = adminId;
            AdminName = adminName;

            Reason = reason;
            Duration = duration;
        }
    }
}
