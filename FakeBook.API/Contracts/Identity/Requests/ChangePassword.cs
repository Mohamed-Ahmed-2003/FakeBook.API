namespace FakeBook.API.Contracts.Identity.Requests
{
    public class ChangePassword
    {
            public required string CurrentPassword { get; set; }
            public required string NewPassword { get; set; }
        
    }
}
