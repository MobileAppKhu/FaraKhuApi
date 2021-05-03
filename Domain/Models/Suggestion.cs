using Domain.BaseModels;

namespace Domain.Models
{
    public class Suggestion : BaseEntity
    {
        public int SuggestionId { get; set; }
        public string Detail { get; set; }
        public BaseUser Sender { get; set; }
        public string SenderId { get; set; }
    }
}