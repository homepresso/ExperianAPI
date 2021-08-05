using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Experian.Api.Client;
using Experian.Api.Client.Bis;


namespace nintex.function.experian
{
    public static class Search
    {
        [FunctionName("Search")]
        public static async Task<dynamic> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string Name = req.Query["Name"];
            string City = req.Query["City"];
            string State = req.Query["State"];
            string Subcode = req.Query["Subcode"];
            string Street = req.Query["Street"];
            string Zip = req.Query["Zip"];
            string Phone = req.Query["Phone"];
            string TaxId = req.Query["TaxId"];
            string Comments = req.Query["Comments"];
            string Token = req.Headers["Token"];
            string clientID = req.Headers["clientID"];
            string clientSecret = req.Headers["clientSecret"];
            string userName = req.Headers["userName"];
            string passWord = req.Headers["passWord"];

            var request = new SearchRequest()
            {
                Name     = Name,
                City     = City,
                State    = State,
                Subcode  = Subcode,
                Street   = Street,
                Zip      = Zip,
                Phone    = Phone,
                TaxId    = TaxId,
                Geo      = true,
                Comments = Comments,
            };

            var serviceClient = new ServiceClient();
            var authResponse    = serviceClient.SendAuthenticationRequestAsync(new AuthRequest(userName, passWord), clientID, clientSecret, ServiceClient.OAuthSandboxUrl).Result;
            var response = serviceClient.PostSearchAsync(Environ.Sandbox, authResponse, request);




            return response.Result;
        }



        }
}


