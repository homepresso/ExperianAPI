using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
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


    public static class Bankruptcies
    {
        [FunctionName("Bankruptcies")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "Bin", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Bin")]
        [OpenApiParameter(name: "Subcode", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Subcode")]
        [OpenApiParameter(name: "BankruptcySummary", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "BankruptcySummary")]
        [OpenApiParameter(name: "BankruptcyDetail", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "BankruptcyDetail")]
        [OpenApiParameter(name: "clientID", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "Experian Client ID")]
        [OpenApiParameter(name: "clientSecret", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "Experian Client Secret")]
        [OpenApiParameter(name: "userName", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "Experian User Name")]
        [OpenApiParameter(name: "passWord", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "Experian Password")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(object), Description = "Bankruptcy Detail")]
        public static async Task<dynamic> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string Bin = req.Query["Bin"];
            string Subcode = req.Query["Subcode"];
        //    string BankruptcySummary = req.Query["BankruptcySummary"];
         //   string BankruptcyDetail = req.Query["BankruptcyDetail"];
            string Token = req.Headers["Token"];
            string clientID = req.Headers["clientID"];
            string clientSecret = req.Headers["clientSecret"];
            string userName = req.Headers["userName"];
            string passWord = req.Headers["passWord"];

            var request = new BankruptcyRequest()
            {
                Bin               = Bin,
                Subcode           = Subcode,
                BankruptcySummary = true,
                BankruptcyDetail  = true,
            };

            var serviceClient = new ServiceClient();
            var authResponse    = serviceClient.SendAuthenticationRequestAsync(new AuthRequest(userName, passWord), clientID, clientSecret, ServiceClient.OAuthSandboxUrl).Result;
            var response = serviceClient.PostBankruptcyAsync(Environ.Sandbox, authResponse, request);




            return response.Result;
        }
    }
}

