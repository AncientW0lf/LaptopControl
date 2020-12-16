using System.Text;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using LaptopControlWeb.Stores;

namespace LaptopControlWeb.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public double Volume
        {
            get
            {
                bool canParse = double.TryParse(_client
                    .GetStringAsync($"{Globals.ApiIp}/basics/get/volume")
                    .GetAwaiter()
                    .GetResult(),
                    out double res);

                if (canParse)
                    return res;
                else throw new HttpRequestException($"Fetched volume cannot be parsed to {typeof(double)}.");
            }
            set
            {
                string jsonbody = System.Text.Json.JsonSerializer.Serialize<dynamic>(new { volume = value });

                _client.PostAsync($"{Globals.ApiIp}/basics/post/volume", new StringContent(jsonbody, Encoding.UTF8, "application/json"))
                    .GetAwaiter()
                    .GetResult();
            }
        }

        private readonly ILogger<IndexModel> _logger;

        private readonly HttpClient _client = new HttpClient();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
