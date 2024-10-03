namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public virtual void Disable() => IsActive = false;
    }
}
