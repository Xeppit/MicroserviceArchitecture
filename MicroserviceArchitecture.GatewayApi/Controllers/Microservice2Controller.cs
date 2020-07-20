using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using MicroserviceArchitecture.GatewayApi;
using MicroserviceArchitecture.GatewayApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroserviceArchitecture.GatewayApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class Microservice2Controller : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenService _tokenService;

        public Microservice2Controller(IHttpClientFactory httpClient, TokenService tokenService)
        {
            _httpClientFactory = httpClient;
            _tokenService = tokenService;
        }
        // GET: api/<MicroserviceController>
        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var client = _httpClientFactory.CreateClient();

            client.SetBearerToken(await _tokenService.GetTokenAsync());

            var response = await client.GetAsync("https://localhost:5004/WeatherForecast");

            if (!response.IsSuccessStatusCode) throw new Exception(response.StatusCode.ToString());

            return JsonConvert.DeserializeObject<List<WeatherForecast>>(await response.Content.ReadAsStringAsync());
        }

        // GET api/<MicroserviceController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MicroserviceController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MicroserviceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MicroserviceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
