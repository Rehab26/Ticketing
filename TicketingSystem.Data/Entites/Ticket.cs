using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Common.Enums;

namespace TicketingSystem.Data.Entites
{
  public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public TicketStatus Status { get; set; }
        public TicketCategory Category { get; set; }
        public TicketPriority Priority { get; set; }
        [DataType("NVARCHAR")]
        [StringLength(50)]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int? ClosedBy { get; set; }
        public int ClientId { get; set; } // this is the ForeignKey
        public int? EmployeeId { get; set; } // allow null , then manager fill this column
        public virtual User User { get; set; }
        public virtual ICollection<Reply> Replys { get; set; }
    }
}

