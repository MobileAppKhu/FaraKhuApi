namespace Domain.Models
{
    public class News : BaseEntity
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}