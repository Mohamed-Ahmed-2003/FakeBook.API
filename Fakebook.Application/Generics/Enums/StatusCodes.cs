namespace Fakebook.Application.Generics.Enums
{
    public enum StatusCodes
    {
        // HTTP Status Codes
        NotFound = 404,
        ServerError = 500,

        // Validation errors should be in the range 400 - 499
        ValidationError = 400, // Generic validation error
        FriendRequestValidationError = 401, // Specific validation error for friend requests
        ChatRoomCreationValidationError = 402, // Validation error for chat room creation
        ChatMessageValidationError = 403, // Validation error for chat messages

        // Infrastructure errors should be in the range 500 - 599
        IdentityCreationFailed = 500, // Identity-related infrastructure error
        DatabaseOperationException = 501, // Database-related infrastructure error
        ChatRoomCreationFailed = 502, // Failure in chat room creation
        ChatMessageSendingFailed = 503, // Failure in sending a chat message
        ChatMessageUpdateFailed = 504, // Failure in updating a chat message
        ChatMessageDeletionFailed = 505, // Failure in deleting a chat message
        ImageUploadFailed = 506,
        ImageDeletionFailed = 507,

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

        // Chat-specific application errors
        ChatRoomNotFound = 610, // The chat room was not found
        ChatRoomNotAccessible = 611, // User does not have access to the chat room
        ChatMessageNotFound = 612, // The chat message was not found
        ChatMessageEditNotAllowed = 613, // User is not allowed to edit the message
        ChatMessageDeleteNotAllowed = 614, // User is not allowed to delete the message
        ChatRoomAlreadyExists = 615, // The chat room already exists

        // General or Unknown errors
        UnknownError = 999
    }


}
