namespace MessagesExchange.Infrastructure.Logging
{
    public class ProblemInfo
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string Exception { get; set; }
        public Dictionary<string, string> Details { get; set; }

        public ProblemInfo()
        {
            Details = new Dictionary<string, string>();
        }
    }
}
