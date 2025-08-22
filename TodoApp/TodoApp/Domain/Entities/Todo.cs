namespace TodoApp.Domain.Entities
{
    public class Todo : BaseEntity
    {
        public required string Title { get; set; }
        public required string Description { get; set; }

        // Foreign key property
        public required string UserId { get; set; }

        // Navigation property
        public User User { get; set; } = null!;
    }
}
