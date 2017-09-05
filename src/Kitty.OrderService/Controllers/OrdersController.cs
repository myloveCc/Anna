using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Kitty.ConsulService.Models;
using Kitty.ConsulService.ServiceProvider;
using Kitty.ServiceConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace Kitty.OrderService.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController:Controller
    {
        private readonly HttpClient _apiClient;
        private RetryPolicy _serverRetryPolicy;
        private int _currentConfigIndex;
        private List<ServiceApi> _userApis;
        private readonly IKittyServiceProvider _conuseServiceProvider;
        private readonly ILogger<OrdersController> _logger;
        private readonly IOptions<ConsulConfig> _consulConfig;

        public OrdersController(IKittyServiceProvider consulServiceProvider, IOptions<ConsulConfig> consulConfig, ILogger<OrdersController> logger)
        {
            _conuseServiceProvider = consulServiceProvider;
            _consulConfig = consulConfig;
            _logger = logger;

            _apiClient = new HttpClient();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        // GET api/orders
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "order1", "order2" };
        }

        // GET api/orders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userServiceDiscovery = _consulConfig.Value.ServiceDiscoveryConfig.Services.FirstOrDefault(m => m.ServiceName == "kitty-user-service");
            _userApis = await _conuseServiceProvider.HealthApisAsync(userServiceDiscovery);

            var retries = _userApis.Count * 2 - 1;
            _logger.LogInformation($"Retry count set to {retries}");

            _serverRetryPolicy = Policy.Handle<HttpRequestException>()
               .RetryAsync(retries, (exception, retryCount) =>
               {
                   ChooseNextServer(retryCount);
               });

            var result = await _serverRetryPolicy.ExecuteAsync(async () =>
             {
                 var serverUrl = _userApis[_currentConfigIndex].ToString();

                 _logger.LogInformation($"user api url is {serverUrl}");

                 var requestPath = $"{serverUrl}api/users/1";

                 _logger.LogInformation($"Making request to {requestPath}");
                 var response = await _apiClient.GetAsync(requestPath).ConfigureAwait(false);
                 var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                 return content;
                 //return JsonConvert.DeserializeObject<IEnumerable<string>>(content);
             });

            return Json(result);
        }

        private void ChooseNextServer(int retryCount)
        {
            if (retryCount % 2 == 0)
            {
                _logger.LogWarning("Trying next server... \n");
                _currentConfigIndex++;

                if (_currentConfigIndex > _userApis.Count - 1)
                    _currentConfigIndex = 0;
            }
        }

        // POST api/orders
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/orders/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/orders/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
