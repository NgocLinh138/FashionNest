using FashionNest.Domain.Common.Entity;

namespace FashionNest.Domain.Entities
{
    public class User : EntityBase<UserId>
    {
        public User()
        {
        }

        public User(string name, string email, string password, string phoneNumber, string address, string? avatar, bool isActive, RoleId roleId)
        {
            Id = UserId.Of(Guid.NewGuid());
            Name = name;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            Address = address;
            Avatar = avatar;
            IsActive = isActive;
            RoleId = roleId;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string? Avatar { get; set; }
        public bool IsActive { get; set; }
        public RoleId RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public void Update(string? name, string? password, string? phoneNumber, string? address)
        {
            if (!string.IsNullOrEmpty(name))
                Name = name;

            if (!string.IsNullOrEmpty(password))
                Password = password;

            if (!string.IsNullOrEmpty(phoneNumber))
                PhoneNumber = phoneNumber;

            if (!string.IsNullOrEmpty(address))
                Address = address;
        }

    }
}