using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Laboratornay6.Services
{
    internal class RateService : IRateService
    {
        private readonly HttpClient client;

        public RateService(HttpClient http)
        {
            client = http;
        }
        public async Task<IEnumerable<Rate>> GetRates(DateTime date)
        {
            string selectedDate = date.ToString("yyyy-MM-dd");
            var result = await client.GetAsync($"?ondate={selectedDate}&periodicity=0");
            string json = await result.Content.ReadAsStringAsync();
            var rate = JsonSerializer.Deserialize<IEnumerable<Rate>>(json);

            return rate;
        }
    }
}
