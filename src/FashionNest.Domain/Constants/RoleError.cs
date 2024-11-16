using FashionNest.Domain.Common;

namespace FashionNest.Domain.Constants
{
    public static class RoleError
    {
        public static Error NotFound = new(
            "Role.NotFound",
            "Role not found!");
    }
}
