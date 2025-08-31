using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Deadpan.Models
{
    /// <summary>
    /// View model for the main account management dashboard (Index view).
    /// It provides a summary of the user's current security settings.
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether the user has a local password set for their account.
        /// </summary>
        public bool HasPassword { get; set; }

        /// <summary>
        /// Gets or sets a list of the user's associated external logins (e.g., Google, Facebook).
        /// </summary>
        public IList<UserLoginInfo> Logins { get; set; }

        /// <summary>
        /// Gets or sets the user's phone number, if one has been added.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether two-factor authentication is enabled for the user.
        /// </summary>
        public bool TwoFactor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's browser has been remembered for two-factor authentication.
        /// </summary>
        public bool BrowserRemembered { get; set; }
    }

    /// <summary>
    /// View model for the "Manage Logins" page.
    /// It displays current external logins and other available providers.
    /// </summary>
    public class ManageLoginsViewModel
    {
        /// <summary>
        /// Gets or sets the list of external logins currently associated with the user's account.
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        /// <summary>
        /// Gets or sets the list of other available external authentication providers that the user can link.
        /// </summary>
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    /// <summary>
    /// View model used for handling factors in two-factor authentication.
    /// </summary>
    public class FactorViewModel
    {
        /// <summary>
        /// Gets or sets the purpose of the two-factor authentication factor (e.g., "Phone").
        /// </summary>
        public string Purpose { get; set; }
    }

    /// <summary>
    /// View model for the "Set Password" form.
    /// This is used when a user registered via an external login and does not yet have a local password.
    /// </summary>
    public class SetPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the new password for the user.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirmation for the new password.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// View model for the "Change Password" form.
    /// This is used when a user with an existing local password wants to change it.
    /// </summary>
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// Gets or sets the user's current password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the user's desired new password.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirmation for the new password.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// View model for the "Add Phone Number" form.
    /// </summary>
    public class AddPhoneNumberViewModel
    {
        /// <summary>
        /// Gets or sets the phone number to be added.
        /// </summary>
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    /// <summary>
    /// View model for the "Verify Phone Number" form.
    /// </summary>
    public class VerifyPhoneNumberViewModel
    {
        /// <summary>
        /// Gets or sets the verification code sent to the user's phone.
        /// </summary>
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the phone number being verified.
        /// </summary>
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    /// <summary>
    /// View model for configuring two-factor authentication.
    /// </summary>
    public class ConfigureTwoFactorViewModel
    {
        /// <summary>
        /// Gets or sets the selected two-factor provider.
        /// </summary>
        public string SelectedProvider { get; set; }

        /// <summary>
        /// Gets or sets the collection of available two-factor providers.
        /// </summary>
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}
