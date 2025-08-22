using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;

namespace TodoApp.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; } = true;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public ICollection<Todo> Todos { get; set; } = new Collection<Todo>();
    }
}
