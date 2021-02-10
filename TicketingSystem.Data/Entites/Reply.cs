using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Data.Entites
{
    public class Reply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TicketId { get; set; }
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Ticket Ticket { get; set; }

    }
}
