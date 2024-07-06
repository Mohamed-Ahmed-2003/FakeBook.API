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
        }

    }
}
