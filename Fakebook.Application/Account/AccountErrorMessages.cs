

namespace Fakebook.Application.Account
{
    public class AccountErrorMessages
    {
        public const string AccountNotFound = "Account with ID {0} not found.";
        public const string AccountRemovalNotAuthorized = "Only the account owner can remove it.";
        public const string AccountUpdateNotAuthorized = "Only the account owner can update it.";
        public const string UserNameTaken = "Username / E-mail already taken.";
        public const string WrongCredentials = "Username / E-mail or password is incorrect.";
        public const string AccountCreationFailed = "Account creation failed. Please try again.";
        public const string AccountDeletionFailed = "Account deletion failed. Please try again.";
        public const string AccountUpdateFailed = "Account update failed. Please try again.";
        public const string EmailNotConfirmed = "E-mail address is not confirmed.";
        public const string PasswordResetFailed = "Password reset failed. Please try again.";
        public const string EmailChangeFailed = "E-mail change failed. Please try again.";
        public const string PasswordChangeFailed = "Password change failed. Please try again.";
        public const string InvalidPassword = "The provided password is invalid.";
        public const string AccountLockedOut = "This account has been locked out. Please try again later.";
        public const string AccountDisabled = "This account has been disabled.";
        public const string AccessDenied = "Access denied. You do not have permission to perform this action.";
        public const string InvalidToken = "The provided token is invalid or expired.";
    }

}
