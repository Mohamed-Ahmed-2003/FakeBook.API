namespace FakeBook.API.Contracts.Chat.Requests
{
    public class SearchMessages
    {
        public required string SearchTerm { get; set; }
        public DateTime StartDate {get;set;}
        public DateTime EndDate  {get;set;}
    }
}
