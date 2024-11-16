using FashionNest.Domain.Common.Entity;

namespace FashionNest.Domain.Entities
{
    public class Role : EntityBase<RoleId>
    {
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;

        public virtual ICollection<User> Users { get; set; } = new List<User>();


        public static Role Create(string name, string description)
        {
            var role = new Role
            {
                Id = new RoleId(Guid.NewGuid()),
                Name = name,
                Description = description
            };

            return role;
        }

        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public record RoleResponse(Guid Id, string Name, string Description);

        public static implicit operator RoleResponse(Role role)
            => new RoleResponse(role.Id.Value, role.Name, role.Description);
    }

   
}
