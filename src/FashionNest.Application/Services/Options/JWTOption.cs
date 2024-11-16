namespace FashionNest.Application.Services.Options
{
    public class JWTOption
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpireMin { get; set; }
    }
}
