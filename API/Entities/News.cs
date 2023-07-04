using API.Entities.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace API.Entities
{
    public class News : BaseEntity
    {
        public News()
        {
        }

        public string Title { get; set; }

        public News(string title, Status status)
        {
            Id = Guid.NewGuid().ToString();
            PublishDate = DateTime.Now;
            Title = title;
            Status = status;
            ValidateEntity();

        }

        public void ValidateEntity()
        {
            AssertionConcern.AssertArgumentNotEmpty(Title, "O título não pode estar vazio!");
            AssertionConcern.AssertArgumentLength(Title, 90, "O título deve ter até 90 caracteres!");
        }
    }
}
