using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace dailyQuotes
{
    class Response
    {
        public Contents contents { get; set; }
    }

    class Contents
    {
        public List<Quote> quotes { get; set; }
    }
    class Quote
    {
        public string quote { get; set; }
        public string author { get; set; }
        public string category { get; set; }
        public string background { get; set; }
    }

    public class Functions
    {

        // Run once a day at 6AM PST
        public static void SendQuote(
            [TimerTrigger("0/1 0 14/24 * * *", RunOnStartup = true)] TimerInfo timerInfo,
            ILogger log,
            [SendGrid(
            From = "FROM_EMAIL",
            To = "TO_EMAIL")]
            out SendGridMessage message
            )
        {
            var quoteObj = GetQuote().Result;
            log.LogInformation($"QUOTE BY {quoteObj.author}: \"{quoteObj.quote}\"");

            message = new SendGridMessage();
            message.Subject = $"[{quoteObj.category}] Today's Quote";
            message.HtmlContent =
                $@"
                   <img src={quoteObj.background}>
                   <div>
                     Today's quote is brought to you by <b>{quoteObj.author}</b>:
                     <div>
                       <q>{quoteObj.quote}</q>
                     </div>
                   </div>";
        }

        static async Task<Quote> GetQuote()
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new System.Uri("https://quotes.rest/qod"),
                Method = HttpMethod.Get,
            };
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var resp = await new HttpClient().SendAsync(request);
            var stream = await resp.Content.ReadAsStreamAsync();
            var response = await JsonSerializer.DeserializeAsync<Response>(stream);
            return response.contents.quotes[0];
        }
    }
}
