namespace Task.CQRS.Messages.Commands
{
    using System.ComponentModel.DataAnnotations;
    using Task.CQRS.Impl;

    public class SignInCmd : CommandBase
    {
        [Required(ErrorMessage = "Email field is required")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        [MaxLength(50, ErrorMessage = "Max Length for Email field is 50")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password field is required")]
        [MinLength(5, ErrorMessage = "Min Length for Password field is 5")]
        [MaxLength(50, ErrorMessage = "Max Length for Password field is 50")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}