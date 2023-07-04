using API.Entities.Enums;

namespace API.Entities.ViewModels
{
    public class NewsViewModel
    {
        public string Title { get; set; }
        public DateTime PublishDate { get; private set; }
        public Status Status { get; set; }
    }
}
