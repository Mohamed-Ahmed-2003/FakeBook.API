namespace FakeBook.API
{
    public class ApiRoutes
    {
        public const string BaseRoute = "Api/v{version:apiVersion}/[controller]";

        public class UserProfile
        {
            public const string RouteId = "{id}";

        }
        public class Post
        {
            public const string RouteId = "{id}";

            public class Comments
            {
                public const string All = "{postId}/comments";
                public const string Single = All + "{commentId}";
            }
            public class Interactions
            {
                public const string All = "{postId}/interactions";
                public const string Single = All + "{interactionId}";
            }
        }
        public class Identity
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

    }
}
