using System.Text;
using System.Text.Json;
using Illiyeen.Models;

namespace Illiyeen.Services
{
//    public class PaymentService : IPaymentService
//    {
//        private readonly IConfiguration _configuration;
//        private readonly HttpClient _httpClient;

//        public PaymentService(IConfiguration configuration, HttpClient httpClient)
//        {
//            _configuration = configuration;
//            _httpClient = httpClient;
//        }

//        public async Task<PaymentResult> ProcessPaymentAsync(Order order, string paymentMethod)
//        {
//            return paymentMethod.ToLower() switch
//            {
//                "bkash" => await InitiatebKashPaymentAsync(order),
//                "nagad" => await InitiateNagadPaymentAsync(order),
//                _ => new PaymentResult { Success = false, ErrorMessage = "Unsupported payment method" }
//            };
//        }

//        public async Task<PaymentResult> InitiatebKashPaymentAsync(Order order)
//        {
//            try
//            {
//                var bkashConfig = _configuration.GetSection("PaymentSettings:bKash");
                
//                // First, get authentication token
//                var authToken = await GetbKashAuthTokenAsync();
//                if (string.IsNullOrEmpty(authToken))
//                {
//                    return new PaymentResult { Success = false, ErrorMessage = "Failed to authenticate with bKash" };
//                }

//                // Create payment request
//                var paymentRequest = new
//                {
//                    mode = "0011",
//                    payerReference = order.PhoneNumber,
//                    callbackURL = $"{GetBaseUrl()}/payment/bkash-callback",
//                    amount = order.TotalAmount.ToString("F2"),
//                    currency = "BDT",
//                    intent = "sale",
//                    merchantInvoiceNumber = $"INV-{order.Id:D6}"
//                };

//                _httpClient.DefaultRequestHeaders.Clear();
//                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
//                _httpClient.DefaultRequestHeaders.Add("X-APP-Key", bkashConfig["AppKey"]);

//                var json = JsonSerializer.Serialize(paymentRequest);
//                var content = new StringContent(json, Encoding.UTF8, "application/json");

//                var response = await _httpClient.PostAsync($"{bkashConfig["ApiUrl"]}/checkout/payment/create", content);
//                var responseContent = await response.Content.ReadAsStringAsync();

//                if (response.IsSuccessStatusCode)
//                {
//                    var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
//                    var paymentId = result.GetProperty("paymentID").GetString();
//                    var redirectUrl = result.GetProperty("bkashURL").GetString();

//                    return new PaymentResult
//                    {
//                        Success = true,
//                        TransactionId = paymentId,
//                        RedirectUrl = redirectUrl,
//                        Amount = order.TotalAmount
//                    };
//                }

//                return new PaymentResult { Success = false, ErrorMessage = "bKash payment creation failed" };
//            }
//            catch (Exception ex)
//            {
//                return new PaymentResult { Success = false, ErrorMessage = $"bKash payment error: {ex.Message}" };
//            }
//        }

//        public async Task<PaymentResult> InitiateNagadPaymentAsync(Order order)
//        {
//            try
//            {
//                var nagadConfig = _configuration.GetSection("PaymentSettings:Nagad");
                
//                // Nagad payment initialization
//                var paymentRequest = new
//                {
//                    merchantId = nagadConfig["MerchantId"],
//                    orderId = $"ORDER-{order.Id:D6}",
//                    amount = order.TotalAmount.ToString("F2"),
//                    currencyCode = "050", // BDT
//                    challenge = GenerateNagadChallenge()
//                };

//                var json = JsonSerializer.Serialize(paymentRequest);
//                var content = new StringContent(json, Encoding.UTF8, "application/json");

//                var response = await _httpClient.PostAsync($"{nagadConfig["ApiUrl"]}/check-out/initialize/{nagadConfig["MerchantId"]}/{order.Id}", content);
//                var responseContent = await response.Content.ReadAsStringAsync();

//                if (response.IsSuccessStatusCode)
//                {
//                    var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
//                    var sensitiveData = result.GetProperty("sensitiveData").GetString();
//                    var signature = result.GetProperty("signature").GetString();

//                    // Complete payment process (simplified)
//                    var redirectUrl = $"{nagadConfig["ApiUrl"]}/check-out/complete/{order.Id}";

//                    return new PaymentResult
//                    {
//                        Success = true,
//                        TransactionId = $"NAGAD-{order.Id}",
//                        RedirectUrl = redirectUrl,
//                        Amount = order.TotalAmount
//                    };
//                }

//                return new PaymentResult { Success = false, ErrorMessage = "Nagad payment creation failed" };
//            }
//            catch (Exception ex)
//            {
//                return new PaymentResult { Success = false, ErrorMessage = $"Nagad payment error: {ex.Message}" };
//            }
//        }

//        public async Task<PaymentResult> VerifybKashPaymentAsync(string paymentId)
//        {
//            try
//            {
//                var bkashConfig = _configuration.GetSection("PaymentSettings:bKash");
//                var authToken = await GetbKashAuthTokenAsync();

//                _httpClient.DefaultRequestHeaders.Clear();
//                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
//                _httpClient.DefaultRequestHeaders.Add("X-APP-Key", bkashConfig["AppKey"]);

//                var executeRequest = new { paymentID = paymentId };
//                var json = JsonSerializer.Serialize(executeRequest);
//                var content = new StringContent(json, Encoding.UTF8, "application/json");

//                var response = await _httpClient.PostAsync($"{bkashConfig["ApiUrl"]}/checkout/payment/execute", content);
//                var responseContent = await response.Content.ReadAsStringAsync();

//                if (response.IsSuccessStatusCode)
//                {
//                    var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
//                    var transactionStatus = result.GetProperty("transactionStatus").GetString();

//                    return new PaymentResult
//                    {
//                        Success = transactionStatus == "Completed",
//                        TransactionId = paymentId
//                    };
//                }

//                return new PaymentResult { Success = false, ErrorMessage = "Payment verification failed" };
//            }
//            catch (Exception ex)
//            {
//                return new PaymentResult { Success = false, ErrorMessage = $"Payment verification error: {ex.Message}" };
//            }
//        }

//        public async Task<PaymentResult> VerifyNagadPaymentAsync(string paymentRefId)
//        {
//            try
//            {
//                // Simplified Nagad verification
//                await Task.Delay(100); // Simulate API call
                
//                return new PaymentResult
//                {
//                    Success = true,
//                    TransactionId = paymentRefId
//                };
//            }
//            catch (Exception ex)
//            {
//                return new PaymentResult { Success = false, ErrorMessage = $"Nagad verification error: {ex.Message}" };
//            }
//        }

//        private async Task<string?> GetbKashAuthTokenAsync()
//        {
//            try
//            {
//                var bkashConfig = _configuration.GetSection("PaymentSettings:bKash");
                
//                var authRequest = new
//                {
//                    app_key = bkashConfig["AppKey"],
//                    app_secret = bkashConfig["AppSecret"]
//                };

//                _httpClient.DefaultRequestHeaders.Clear();
//                _httpClient.DefaultRequestHeaders.Add("Username", bkashConfig["Username"]);
//                _httpClient.DefaultRequestHeaders.Add("Password", bkashConfig["Password"]);

//                var json = JsonSerializer.Serialize(authRequest);
//                var content = new StringContent(json, Encoding.UTF8, "application/json");

//                var response = await _httpClient.PostAsync($"{bkashConfig["ApiUrl"]}/checkout/token/grant", content);
//                var responseContent = await response.Content.ReadAsStringAsync();

//                if (response.IsSuccessStatusCode)
//                {
//                    var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
//                    return result.GetProperty("id_token").GetString();
//                }

//                return null;
//            }
//            catch
//            {
//                return null;
//            }
//        }

//        private string GenerateNagadChallenge()
//        {
//            return Guid.NewGuid().ToString("N");
//        }

//        private string GetBaseUrl()
//        {
//            return "https://illiyeen.com"; // Replace with actual base URL
//        }
//    }
}
