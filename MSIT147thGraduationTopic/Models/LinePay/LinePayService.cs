using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Dtos.LinePay;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;

namespace MSIT147thGraduationTopic.Models.LinePay
{
    public class LinePayService
    {

        private readonly JsonProvider _jsonProvider;
        private readonly string channelId = "2000393663";
        private readonly string channelSecretKey = "4770917b92fee21968eb314291424734";
        private readonly string linePayBaseApiUrl = "https://sandbox-api-pay.line.me";

        private static HttpClient client;

        public LinePayService()
        {
            client = new HttpClient();
            _jsonProvider = new JsonProvider();
        }

        public PaymentRequestDto GetPaymentRequestDto(
            int orderId,
            int totalPaymentAmount,
            string baseUrl,
            IEnumerable<CartItemCheckoutDto> checkoutDtos)
        {
            var req = new PaymentRequestDto  // +UserFee
            {
                Packages = new List<PackageDto>(),
                Currency = "TWD",
                OrderId = orderId.ToString(), //orderid
                Amount = totalPaymentAmount, //PaymentAmount                
            };

            //redirecturl
            var redirectUrl = new RedirectUrlsDto
            {
                ConfirmUrl = baseUrl + "/Buy/LinePayConfirm/",
                CancelUrl = baseUrl + "/Buy/Failed/",
            };
            req.RedirectUrls = redirectUrl;

            var package = new PackageDto
            {
                Id = orderId.ToString(),
                Amount = totalPaymentAmount,
                Name = "福祿獸寵物商城線上訂單",
                Products = new List<LinePayProductDto>(),
            };
            req.Packages.Add(package);

            //list =>
            var products = checkoutDtos.Select(o => new LinePayProductDto
            {
                Name = o.ItemName,
                Quantity = o.Quantity,
                Price = o.Price * o.Quantity * o.DiscountPercentage / 100,
                Id = o.SpecId.ToString(),
                OriginalPrice = o.DiscountPercentage == 100 ? null : o.Price * o.Quantity,
            });
            package.Products.AddRange(products);
            package.Amount = products.Sum(o => o.Price * o.Quantity);
            req.Amount = package.Amount;

            

            return req;
        }



        public async Task<PaymentResponseDto> SendPaymentRequest(PaymentRequestDto dto)
        {
            var json = _jsonProvider.Serialize(dto);
            // 產生 GUID Nonce
            var nonce = Guid.NewGuid().ToString();
            // 要放入 signature 中的 requestUrl
            var requestUrl = "/v3/payments/request";

            //使用 channelSecretKey & requestUrl & jsonBody & nonce 做簽章
            var signature = SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + json + nonce);

            var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + requestUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            // 帶入 Headers
            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);

            var response = await client.SendAsync(request);
            var linePayResponse = _jsonProvider.Deserialize<PaymentResponseDto>(await response.Content.ReadAsStringAsync());

            Console.WriteLine(nonce);
            Console.WriteLine(signature);

            return linePayResponse;
        }


        // 取得 transactionId 後進行確認交易
        public async Task<PaymentConfirmResponseDto> ConfirmPayment(string transactionId, string orderId, PaymentConfirmDto dto) //加上 OrderId 去找資料
        {
            var json = _jsonProvider.Serialize(dto);

            var nonce = Guid.NewGuid().ToString();
            var requestUrl = string.Format("/v3/payments/{0}/confirm", transactionId);
            var signature = SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + json + nonce);

            var request = new HttpRequestMessage(HttpMethod.Post, String.Format(linePayBaseApiUrl + requestUrl, transactionId))
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);

            var response = await client.SendAsync(request);
            var responseDto = _jsonProvider.Deserialize<PaymentConfirmResponseDto>(await response.Content.ReadAsStringAsync());
            return responseDto;
        }


        public async void TransactionCancel(string transactionId)
        {
            //使用者取消交易則會到這裏。
            Console.WriteLine($"訂單 {transactionId} 已取消");
        }

    }
}
