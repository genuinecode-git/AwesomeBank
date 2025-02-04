namespace AwesomeBank.API.Application.Models.Requests
{
    public class StatementRequest
    {
        [FromRoute]
        public string AccountNumber { get; set; }

        [FromRoute]
        public string Year { get; set; }

        [FromRoute]
        public string Month { get; set; }
    }
}