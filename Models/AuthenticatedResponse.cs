namespace FAP_API.Models
{
    public class AuthenticatedResponse
    {
        public string Access_token { get; set; }
        public User User { get; set; }
    }
}
