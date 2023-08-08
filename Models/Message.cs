namespace FAP_API.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int ExpeditiousID { get; set; }
        public int BeneficiaryID { get; set; }
    }
}
