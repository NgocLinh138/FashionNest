using FashionNest.Domain.Common;

namespace FashionNest.Domain.Constants
{
    public static class ProductError
    {
        public static Error NotFound = new(
            "Product.NotFound",
            "Product not found!");
    }
}
