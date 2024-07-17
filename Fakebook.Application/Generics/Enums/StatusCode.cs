namespace Fakebook.Application.Generics.Enums
{
    public enum StatusCode
    {
        NotFound = 404 ,
        Succeed = 200,
        ServerError = 500,

        ValidationError = 444,

        UserAlreadyExists  = 220,
        UserCreationFailed = 221,
        UserNotFound= 222,
        ProfileNotFound = 223,

        PostUpdateNotAuthorized  = 601,
        PostRemovalNotAuthorized = 602 , 
        CommentUpdateNotAuthorized = 603 ,
        CommentRemovalNotAuthorized = 604 ,
        InteractionRemovalNotAuthorized = 605 ,

        Unknown = 999,
    }
}
