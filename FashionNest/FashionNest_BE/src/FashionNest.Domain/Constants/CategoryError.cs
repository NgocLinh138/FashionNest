using FashionNest.Domain.Common;

namespace FashionNest.Domain.Constants
{
    public class CategoryError
    {
        public static readonly Error NotFound = new("Category.NotFound", "Category not found.");

        public static readonly Error DuplicateName = new("Category.DuplicateName", "Category name already exists.");

    }
}
