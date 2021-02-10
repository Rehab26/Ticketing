using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace TicketingSystem.Web.Employee.Models
{
    public class AuthenticationModel
    {


        [Required]
        [IntegerValidator]
        [MaxLength(4, ErrorMessage = "Code must be at least 4 digit")]
        public string InputCode { get; set; }
    }
}