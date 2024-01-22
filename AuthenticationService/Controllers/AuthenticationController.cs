using AuthenticationService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IConfiguration _configuration;

        public AuthenticationController(ILogger<AuthenticationController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("GetAuthenticationToken")]
        public async Task<ActionResult<AuthenticationDto>> GetAuthenticationToken([FromBody] UserCredentialsDto userCredentials)
        {
            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, $"https://{_configuration["Auth0:Domain"]}/oauth/token");

            req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "content-type", "application/json" },
                { "client_id", _configuration["Auth0:ClientId"] },
                { "client_secret", _configuration["Auth0:ClientSecret"] },
                { "audience", "https://localhost" },
                { "scope", "manager:catalog buyer:catalog" },
                { "username", userCredentials.UserName },
                { "password", userCredentials.Password },
                { "grant_type", "password" }
            });

            #region Client Credentials Authentication

            // Client credentials authentication.
            // req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            // {
            //    { "content-type", "application/json" },
            //    { "client_id", "SdM28vqgNcEPRMOFXifqBSySZfvoJiob" },
            //    { "client_secret", "GOXd_LN-Hb7_BrXRLdM2NfzJRGBLfHsnlfwIAO-n_KaoiP4PFYmF-4cf7DqxGF50" },
            //    { "audience", "https://localhost" },
            //    { "grant_type", "client_credentials" }
            // });

            #endregion

            var response = await client.SendAsync(req);
            string reponseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation(reponseContent);

            if (response.IsSuccessStatusCode)
            {   
                AuthenticationDto? tokenResponse = JsonConvert.DeserializeObject<AuthenticationDto>(reponseContent);
                return Ok(tokenResponse);
            }
            else
            {
                return BadRequest( 
                    new { 
                        Error = $"{(int)response.StatusCode} {response.ReasonPhrase}",
                        Message = $"{reponseContent}"
                    }
                );
            }

        }

        /// <summary>
        /// Use this endpoint in case of implicit flow, since there are only APIs client credentials flow is used
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAuthorizationToken")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> GetAuthorizationToken()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://{_configuration["Auth0:Domain"]}/authorize?client_id={_configuration["Auth0:ClientId"]}&redirect_uri=https://localhost");
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("content-type", "application/json"));
            collection.Add(new("client_id", _configuration["Auth0:ClientId"]));
            collection.Add(new("client_secret", _configuration["Auth0:ClientSecret"]));
            collection.Add(new("grant_type", "client_credentials"));
            collection.Add(new("response_type", "code"));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return Ok(response);
        }
    }
}
