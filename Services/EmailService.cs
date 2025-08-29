using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Illiyeen.Models;

namespace Illiyeen.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendWelcomeEmailAsync(string email, string firstName)
        {
            var subject = "Welcome to ILLIYEEN - Premium Bengali Fashion";
            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; color: #333; }}
                        .header {{ background: linear-gradient(135deg, #d4af37, #f4e6b1); padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; }}
                        .footer {{ background: #1a1a1a; color: #d4af37; padding: 20px; text-align: center; }}
                    </style>
                </head>
                <body>
                    <div class='header'>
                        <h1 style='color: #1a1a1a; margin: 0;'>ILLIYEEN</h1>
                        <p style='color: #1a1a1a; margin: 5px 0 0 0;'>Premium Bengali Fashion</p>
                    </div>
                    <div class='content'>
                        <h2>Welcome, {firstName}!</h2>
                        <p>Thank you for joining ILLIYEEN, the premier destination for authentic Bengali fashion and luxury lifestyle products.</p>
                        <p>Explore our exquisite collection of traditional panjabis, elegant sarees, premium accessories, and much more.</p>
                        <p>Happy shopping!</p>
                    </div>
                    <div class='footer'>
                        <p>&copy; 2025 ILLIYEEN. All rights reserved.</p>
                    </div>
                </body>
                </html>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendOrderConfirmationEmailAsync(string email, Order order)
        {
            var subject = $"Order Confirmation #{order.Id} - ILLIYEEN";
            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; color: #333; }}
                        .header {{ background: linear-gradient(135deg, #d4af37, #f4e6b1); padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; }}
                        .order-details {{ background: #f9f9f9; padding: 15px; margin: 20px 0; border-radius: 5px; }}
                        .footer {{ background: #1a1a1a; color: #d4af37; padding: 20px; text-align: center; }}
                    </style>
                </head>
                <body>
                    <div class='header'>
                        <h1 style='color: #1a1a1a; margin: 0;'>ILLIYEEN</h1>
                        <p style='color: #1a1a1a; margin: 5px 0 0 0;'>Premium Bengali Fashion</p>
                    </div>
                    <div class='content'>
                        <h2>Order Confirmation</h2>
                        <p>Thank you for your order! We've received your order and it's being processed.</p>
                        <div class='order-details'>
                            <h3>Order Details</h3>
                            <p><strong>Order ID:</strong> #{order.Id}</p>
                            <p><strong>Total Amount:</strong> ৳{order.TotalAmount:N2}</p>
                            <p><strong>Payment Method:</strong> {order.PaymentMethod}</p>
                            <p><strong>Order Date:</strong> {order.CreatedAt:dd MMM yyyy}</p>
                        </div>
                        <p>We'll notify you when your order ships.</p>
                    </div>
                    <div class='footer'>
                        <p>&copy; 2025 ILLIYEEN. All rights reserved.</p>
                    </div>
                </body>
                </html>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetToken)
        {
            var subject = "Reset Your Password - ILLIYEEN";
            var resetUrl = $"https://illiyeen.com/account/reset-password?token={resetToken}";
            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; color: #333; }}
                        .header {{ background: linear-gradient(135deg, #d4af37, #f4e6b1); padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; }}
                        .button {{ background: #d4af37; color: #1a1a1a; padding: 12px 24px; text-decoration: none; border-radius: 5px; display: inline-block; margin: 20px 0; }}
                        .footer {{ background: #1a1a1a; color: #d4af37; padding: 20px; text-align: center; }}
                    </style>
                </head>
                <body>
                    <div class='header'>
                        <h1 style='color: #1a1a1a; margin: 0;'>ILLIYEEN</h1>
                        <p style='color: #1a1a1a; margin: 5px 0 0 0;'>Premium Bengali Fashion</p>
                    </div>
                    <div class='content'>
                        <h2>Reset Your Password</h2>
                        <p>We received a request to reset your password. Click the button below to reset it:</p>
                        <a href='{resetUrl}' class='button'>Reset Password</a>
                        <p>If you didn't request this, please ignore this email.</p>
                        <p>This link will expire in 24 hours.</p>
                    </div>
                    <div class='footer'>
                        <p>&copy; 2025 ILLIYEEN. All rights reserved.</p>
                    </div>
                </body>
                </html>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port);
                client.EnableSsl = _smtpSettings.EnableSsl;
                client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);

                var message = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.FromEmail, _smtpSettings.FromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                message.To.Add(to);

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Log error - in production, use proper logging
                Console.WriteLine($"Email send failed: {ex.Message}");
                throw;
            }
        }
    }
}
