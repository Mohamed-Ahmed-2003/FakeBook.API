namespace Fakebook.Application.Generics.Enums
{
    public enum StatusCode
    {
        // HTTP Status Codes
        NotFound = 404,
        ServerError = 500,

        // Validation errors should be in the range 400 - 499
        ValidationError = 400, // Generic validation error
        FriendRequestValidationError = 401, // Specific validation error for friend requests

        // Infrastructure errors should be in the range 500 - 599
        IdentityCreationFailed = 500, // Identity-related infrastructure error
        DatabaseOperationException = 501, // Database-related infrastructure error

        // Application errors should be in the range 600 - 699
        PostUpdateNotPossible = 600,
        PostDeleteNotPossible = 601,
        InteractionRemovalNotAuthorized = 602,
        IdentityUserAlreadyExists = 603,
        IdentityUserDoesNotExist = 604,
        IncorrectPassword = 605,
        UnauthorizedAccountRemoval = 606,
        CommentRemovalNotAuthorized = 607,
        FriendRequestAcceptNotPossible = 608,
        FriendRequestRejectNotPossible = 609,

        // General or Unknown errors
        UnknownError = 999
    }

}
