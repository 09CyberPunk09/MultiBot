using System.ComponentModel.DataAnnotations;

namespace LifeTracker.Web.Host.Models.IncomeModels.Account
{
    public class LoginIncomeModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
