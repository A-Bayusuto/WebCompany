using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KnifeCompany.Utility
{
    public class PayPalAPI
    {
        public IConfiguration _configuration { get; }

        public PayPalAPI(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> getRedirectURLToPayPal(double total_input, string currency_input)
        {
            try
            {
                return Task.Run(async () =>
                {
                    HttpClient http = GetPayPalHttpClient();
                    PayPalAccessToken accessToken = await GetPayPalAccessTokenAsync(http);
                    PayPalPaymentCreatedResponse createdPayment = await CreatePayPalPaymentAsync(http, accessToken, 
                        total_input, currency_input);
                    return createdPayment.links.First(x => x.rel == "approval_url").href;
                }).Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Failed to login to PayPal");
                return null;
            }
        }

        public async Task<PayPalPaymentExecutedResponse> executedPayment(string paymentId, string payerId)
        {
            try
            {
                HttpClient http = GetPayPalHttpClient();
                PayPalAccessToken accessToken = await GetPayPalAccessTokenAsync(http);
                return await ExecutedPayPalPaymentAsync(http, accessToken, paymentId, payerId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Failed to login to PayPal");
                return null;
            }
        }

        private HttpClient GetPayPalHttpClient()
        {
            string sandbox = _configuration["Paypal:urlAPI"];
            var http = new HttpClient
            {
                BaseAddress = new Uri(sandbox),
                Timeout = TimeSpan.FromSeconds(90),
            };
            return http;
        }

        private async Task<PayPalAccessToken> GetPayPalAccessTokenAsync(HttpClient http)
        {
            byte[] bytes = Encoding.GetEncoding("iso-8859-1").
                GetBytes($"{_configuration["Paypal:clientId"]}:{_configuration["Paypal:secret"]}");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));

            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            };

            request.Content = new FormUrlEncodedContent(form);
            HttpResponseMessage response = await http.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
            PayPalAccessToken accessToken = JsonConvert.DeserializeObject<PayPalAccessToken>(content);
            return accessToken;

        }

        private async Task<PayPalPaymentCreatedResponse> CreatePayPalPaymentAsync(HttpClient http,
            PayPalAccessToken accessToken, double total_input, string currency_input)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "v1/payments/payment");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

            var payment = JObject.FromObject(new
            {
                intent = "sale",
                redirect_urls = new
                {
                    return_url = _configuration["Paypal:returnUrl"],
                    cancel_url = _configuration["Paypal:cancelUrl"]
                },
                payer = new { payment_method = "paypal" },
                transactions = JArray.FromObject(new[]
                {
                    new
                    {
                        amount = new
                        {
                            total = total_input,
                            currency =currency_input
                        }
                    }
                })
            });

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await http.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
            PayPalPaymentCreatedResponse payPalPaymentCreated =
                JsonConvert.DeserializeObject<PayPalPaymentCreatedResponse>(content);
            return payPalPaymentCreated;
        }

        private async Task<PayPalPaymentExecutedResponse> ExecutedPayPalPaymentAsync(HttpClient http,
            PayPalAccessToken accessToken, string paymentId, string payerId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"v1/payments/payment/{paymentId}/execute");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

            var payment = JObject.FromObject(new
            {
                payer_id = payerId
            });

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await http.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();
            PayPalPaymentExecutedResponse executedPayment =
                JsonConvert.DeserializeObject<PayPalPaymentExecutedResponse>(content);
            return executedPayment;

        }
    }
}
