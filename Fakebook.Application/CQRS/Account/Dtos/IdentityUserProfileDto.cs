namespace Fakebook.Application.CQRS.Account.Dtos
{
    public class IdentityUserProfileDto
    {
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public required string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string CurrentCity { get; set; }
        public required string Token { get; set; }
    }
}
