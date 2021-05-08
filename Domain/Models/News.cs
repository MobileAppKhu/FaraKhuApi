namespace Domain.Models
{
    public class News : BaseEntity
    {
        public string NewsId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}