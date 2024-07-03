namespace FakeBook.API.Contracts.Others
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }

        public string StatusName { get; set; }

        public List<string> Errors { get; set; } = new List<string>();

        public DateTime Timestamp { get; set; }
    }
}
