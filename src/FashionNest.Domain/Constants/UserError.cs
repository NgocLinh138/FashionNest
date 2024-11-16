using FashionNest.Domain.Common;

namespace FashionNest.Domain.Constants
{
    public static class UserError
    {
        public static Error NotFound = new(
            "User.NotFound",
            "User not found!");

        public static Error UserExisted = new(
            "User.UserExisted",
            "User already exists!");

        public static Error RoleNotFound = new(
            "User.RoleNotFound",
            "Customer role not found.");

        public static Error InvalidPassword = new(
            "User.InvalidPassword",
            "Old password is incorrect.");

        public static Error PasswordMismatch = new(
            "User.PasswordMismatch",
            "Passwords do not match.");

        public static Error InvalidToken = new(
            "User.InvalidToken",
            "Token is invalid.");
    }
}
