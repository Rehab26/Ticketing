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
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
        [DataType("NVARCHAR")]
        [StringLength(20)]
        public string FirstName { get; set; }
        [DataType("NVARCHAR")]
        [StringLength(20)]
        public string LastName { get; set; }
        public UserType Type { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        [DataType("NVARCHAR")]
        public UserCountry Country { get; set; }
        public DateTime CreatedDate { get; set; }
        public LoginApproach LoginType { get; set; }
        [Required]
        public UserStatus UserStatus { get; set; }
        public ICollection<Reply> Replies { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        [DataType("NVARCHAR")]
        public string ResetPasswordCode { get; set; } // Will be used when a client ask to change the password
    }
}
