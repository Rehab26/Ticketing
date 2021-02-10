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
   public class FileStorage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public AttachmentType Type { get; set; }
        [DataType("VARCHAR")]

        [StringLength(250)]
        public string Path { get; set; }
        public int? Reference { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
