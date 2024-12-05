using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;

namespace FashionNest.Application.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _configuration;

        public CloudinaryService(IConfiguration configuration)
        {
            _configuration = configuration;

            // Lấy thông tin cấu hình từ appsettings.json
            var cloudName = _configuration["Cloudinary:CloudName"];
            var apiKey = _configuration["Cloudinary:ApiKey"];
            var apiSecret = _configuration["Cloudinary:ApiSecret"];

            // Log thông tin để kiểm tra cấu hình
            Console.WriteLine($"CloudName: {cloudName}, ApiKey: {apiKey}, ApiSecret: {apiSecret}");

            // Tạo tài khoản Cloudinary và khởi tạo Cloudinary client
            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            // Đặt lại vị trí của stream về đầu trước khi upload
            stream.Position = 0;

            if (stream.Length == 0)
            {
                throw new ArgumentException("Stream is empty after copying.");
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
            };

            try
            {
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult == null)
                {
                    throw new Exception("Upload result is null. Check Cloudinary API status and configuration.");
                }

                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"Upload failed: {uploadResult.Error?.Message}");
                }

                return uploadResult?.SecureUrl?.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading to Cloudinary: {ex.Message}");
                throw new Exception("Upload to Cloudinary failed.");
            }
        }
    }
}
