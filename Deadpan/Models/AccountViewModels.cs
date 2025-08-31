using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Deadpan.Models
{
    /// <summary>
    /// View model for confirming an external login for the first time.
    /// It's used when a user authenticates via a third-party provider (e.g., Google)
    /// and needs to associate that login with a local account email.
    /// </summary>
    public class ExternalLoginConfirmationViewModel
    {
        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    /// <summary>
    /// View model for the list of available external login providers.
    /// </summary>
    public class ExternalLoginListViewModel
    {
        /// <summary>
        /// Gets or sets the URL to redirect to after a successful login.
        /// </summary>
        public string ReturnUrl { get; set; }
    }

    /// <summary>
    /// View model for the "Send Code" screen in two-factor authentication.
    /// </summary>
    public class SendCodeViewModel
    {
        /// <summary>
        /// Gets or sets the two-factor authentication provider selected by the user (e.g., "Email" or "SMS").
        /// </summary>
        public string SelectedProvider { get; set; }

        /// <summary>
        /// Gets or sets the collection of available two-factor authentication providers for the user.
        /// </summary>
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }

        /// <summary>
        /// Gets or sets the URL to redirect to after a successful login.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's "Remember Me" choice should be persisted.
        /// </summary>
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// View model for the "Verify Code" screen in two-factor authentication.
    /// </summary>
    public class VerifyCodeViewModel
    {
        /// <summary>
        /// Gets or sets the two-factor authentication provider being used.
        /// </summary>
        [Required]
        public string Provider { get; set; }

        /// <summary>
        /// Gets or sets the verification code entered by the user.
        /// </summary>
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the URL to redirect to after a successful login.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the browser should be remembered to bypass two-factor authentication in the future.
        /// </summary>
        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's "Remember Me" choice should be persisted.
        /// </summary>
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// View model for the initial "Forgot Password" screen.
    /// </summary>
    public class ForgotViewModel
    {
        /// <summary>
        /// Gets or sets the user's email address to which the password reset link will be sent.
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    /// <summary>
    /// View model for the login form.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's login session should be persisted.
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// View model for the registration form.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets the user's chosen nickname or display name.
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Nickname")]
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets the user's email address, which will also serve as their username.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's chosen password.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password confirmation field, used to verify the chosen password.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// View model for the "Reset Password" form, used after a user has clicked a password reset link.
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the user's email address for account confirmation.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's new password.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirmation field for the new password.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the password reset token from the URL.
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// View model for the "Forgot Password" confirmation screen.
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
