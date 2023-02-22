using System.ComponentModel.DataAnnotations;

namespace LifeTracker.Web.Host.Models.IncomeModels.Account
{
    public class SignUpDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
