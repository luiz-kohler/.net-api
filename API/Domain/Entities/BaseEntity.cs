namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public virtual void Disable() 
        {
            IsActive = false;
            UpdateBaseEntity();
        } 
        
        public virtual void UpdateBaseEntity() => UpdatedAt = DateTime.UtcNow;
    }
}
