
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Experian.Api.Client;
using Experian.Api.Client.Bis;

namespace nintex.function.experian
{

    public class token
    
     {
        public long issued_at { get; set;}
        public long expires_in { get; set;}
        public string token_type { get; set;}
        public string access_token { get; set;}
    }
    public static class experianauthToken
    {
        [FunctionName("token")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Experian Auth Token"})]
        [OpenApiParameter(name: "clientID", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "Experian Client ID")]
        [OpenApiParameter(name: "clientSecret", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "Experian Client Secret")]
        [OpenApiParameter(name: "userName", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "Experian User Name")]
        [OpenApiParameter(name: "passWord", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "Experian Password")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "Bearer - Token")]
        public static async Task<token> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

         string clientID = req.Headers["clientID"];
         string clientSecret = req.Headers["clientSecret"];
         string userName = req.Headers["userName"];
         string passWord = req.Headers["passWord"];

            var serviceClient = new ServiceClient();
            var authResult =  serviceClient.SendAuthenticationRequestAsync(new AuthRequest(userName, passWord), clientID, clientSecret, ServiceClient.OAuthSandboxUrl).Result;

var token = new token
{
issued_at = authResult.IssuedAt,
expires_in = authResult.ExpiresIn,
token_type = authResult.TokenType,
access_token = authResult.AccessToken

};
            return token;
        }
    }
}

