using Domain.Enums;

namespace Domain.Entities
{
    public class Chore : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public EChoreStatus Status { get; set; }

        public Chore(string tittle,
            string description,
            EChoreStatus status = EChoreStatus.ToDo)
        {
            Title = tittle;
            Description = description;
            Status = status;
        }
    }
}
