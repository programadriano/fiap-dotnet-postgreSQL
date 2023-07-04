using API.Entities.Enums;

namespace API.Entities
{
    public abstract class BaseEntity
    {
        
        public string Id { get; protected set; }
        public int Deleted { get; set; }        
        public DateTime PublishDate { get; protected set; }
        public Status Status { get;  set; }
    }
}
