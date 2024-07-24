namespace Fakebook.Application.CQRS.Chat
{
    public class ChatErrorMessages
    {
        public const string ChatRoomNotFound = "Chat room with ID {0} not found.";
        public const string ChatRoomCreationFailed = "Failed to create the chat room.";
        public const string ChatRoomAlreadyExists = "Chat room with ID {0} already exists.";
        public const string ChatRoomAccessDenied = "You do not have access to this chat room.";
        public const string ChatMessageNotFound = "Chat message with ID {0} not found.";
        public const string ChatMessageSendingFailed = "Failed to send the chat message.";
        public const string ChatMessageUpdateFailed = "Failed to update the chat message.";
        public const string ChatMessageDeletionFailed = "Failed to delete the chat message.";
        public const string ChatMessageEditNotAllowed = "You are not allowed to edit this chat message.";
        public const string ChatMessageDeleteNotAllowed = "You are not allowed to delete this chat message.";
        public const string UserNotFriends = "User is not friends with the specified friend ID.";

    }
}
