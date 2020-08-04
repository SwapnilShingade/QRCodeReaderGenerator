using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.IO;
using System.Linq;

namespace QRCodeReader
{
    public class CodeGeneratorService : ICodeGeneratorService
    {

        /// <summary>
        /// generates QR code png file with encoded data
        /// </summary>
        public bool GenerateCode(IConfigurationRoot configurationRoot, ILogger logger)
        {
            logger.LogInformation($"Request received in Code Generator Service at: {DateTime.UtcNow.ToString()}");
            bool result = false;
            try
            {                
                Console.WriteLine("\n" + "****You have selected QR Code Generator****");
                return SubmitRequest(configurationRoot);
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception Occured at Code Generator Service at: {DateTime.UtcNow.ToString()}: Exception Details{ex.Message}");                
                Console.WriteLine("Exception Occured at Code Generator Service: ", ex.Message);
            }
            return result;
        }

        /// <summary>
        /// submits code generation request to external API
        /// </summary>
        /// <param name="configurationRoot"></param>
        /// <returns></returns>
        private static bool SubmitRequest(IConfigurationRoot configurationRoot)
        {
            string dataToBeEncoded;
            do
            {
                Console.WriteLine("\n" + "Please Enter some value in Data");
                dataToBeEncoded = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(dataToBeEncoded));

            var client = new RestClient(configurationRoot.GetConnectionString("QrSiteGeneratorConnection"))
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);

            //Data to be encoded is passed in data as a Key 
            request.AddParameter("data", dataToBeEncoded, ParameterType.QueryString);

            var fileBytes = client.DownloadData(request);
            if (fileBytes.Any())
            {
                File.WriteAllBytes(Path.Combine(@"C:\Users\Public\", "response.png"), fileBytes);
                Console.WriteLine("\n" + "File named response.png is saved at the location: C:/Users/Public");
                return true;
            }
            return false;
        }
    }
}

