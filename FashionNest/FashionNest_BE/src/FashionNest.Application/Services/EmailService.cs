using FashionNest.Application.Services.Interface;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Security;

namespace FashionNest.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("FashionNest", configuration["EmailSettings:FromEmail"]));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            try
            {
                using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Kết nối đến SMTP server của Gmail
                    await smtpClient.ConnectAsync(configuration["EmailSettings:SmtpServer"],
                        int.Parse(configuration["EmailSettings:Port"]), SecureSocketOptions.StartTls);

                    // Đăng nhập bằng tài khoản và mật khẩu ứng dụng của Gmail
                    await smtpClient.AuthenticateAsync(configuration["EmailSettings:Username"], configuration["EmailSettings:Password"]);

                    // Gửi email
                    await smtpClient.SendAsync(emailMessage);
                    await smtpClient.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (nếu có)
                throw new InvalidOperationException("Could not send email", ex);
            }
        }
    }
}
