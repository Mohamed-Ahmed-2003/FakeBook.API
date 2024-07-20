namespace FakeBook.API
{
    public static class ApiRoutes
    {
        public const string BaseRoute = "Api/v{version:apiVersion}/[controller]";

        public static class UserProfile
        {
            public const string RouteId = "{id}";

        }
        public static class Post
        {
            public const string RouteId = "{id}";

            public static class Comments
            {
                public const string All = "{postId}/comments";
                public const string Single = All + "{commentId}";
            }
            public static class Interactions
            {
                public const string All = "{postId}/interactions";
                public const string Single = All + "{interactionId}";
            }
        }
        public static class Identity
        {
            public const string Login = "login";
            public const string Register = "register";
            public const string CurrentUser = "currentUser";
            public const string ChangePassword = "changePassword";
            public const string ForgotPassword = "forgotPassword";
            public const string ConfirmEmail = "confirmEmail";
            public const string ConfirmPhone = "confirmPhone";
            public const string ResetPassword = "resetPassword";

        }

        public static class Friendships
        {
            public const string FriendRequestCreate = "friendRequest";
            public const string FriendRequestAccept = "friendRequest/{friendRequestId}/accept";
            public const string FriendRequestReject = "friendRequest/{friendRequestId}/reject";
            public const string GetFriendRequests = "friendRequests";
            public const string ListFriends = "friends";
            public const string RemoveFriend = "removeFriend";
            public const string GetFriendDetails = "FriendDetails";
            public const string SearchFriends = "searchFriends";

        }

    }
}
