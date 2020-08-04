using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace QRCodeReader
{
    public class CodeReaderService : ICodeReaderService
    {
        /// <summary>
        /// reads encoded data from the qr code file
        /// </summary>
        public bool ReadCode(IConfigurationRoot configurationRoot, ILogger logger)
        {
            logger.LogInformation($"Request received in Code Reader Service at: {DateTime.UtcNow.ToString()}");
            bool result = false;
            try
            {                
                Console.WriteLine("\n" + "****You have selected QR Code Reader****");
                return SubmitRequest(configurationRoot);
            }
            catch (Exception ex)

            {
                logger.LogError($"Exception Occured at Code Reader Service at: {DateTime.UtcNow.ToString()}: Exception Details{ex.Message}");
                Console.WriteLine("Exception Occured at Code Reader Service: ", ex.Message);
            }
            return result;
        }

        /// <summary>
        /// submits to request to external api
        /// </summary>
        /// <param name="configurationRoot"></param>
        /// <returns></returns>
        private static bool SubmitRequest(IConfigurationRoot configurationRoot)
        {
            Console.WriteLine("\n" + "Enter file path with file name");
            string filePath;
            string extension;
            do
            {
                Console.WriteLine("Supported formats are PNG, GIF or JP(E)G, Please Enter file path with file name ");
                filePath = Console.ReadLine();
                extension = Path.GetExtension(filePath).Replace(".", "");
            }
            while (!Enum.TryParse(typeof(ValidExtension), extension, true, out object enumValue));


            // Handling special characters in file path
            if (filePath.Contains(@"\"))
            {
                filePath = filePath.Replace(@"\", "/");
            }

            // Initialize Rest Client with API Url for Reading QR Code
            var client = new RestClient(configurationRoot.GetConnectionString("QrSiteReaderConnection"))
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.POST);
            request.AddFile("file", filePath);

            //Sending request to Post Method
            IRestResponse response = client.Execute(request);

            if (!(response.StatusCode == HttpStatusCode.OK))
                Console.WriteLine("\n" + "Bad Request: There was problem with the file/request");
            else
            {
                var result = JsonConvert.DeserializeObject<Result[]>(response.Content);
                var qrEncodedData = result != null && result.Length > 0 ? result.FirstOrDefault().Symbol.FirstOrDefault().Data.ToString() : null;
                Console.WriteLine($"Data Encoded in the QR Code: {qrEncodedData}");
                return true;
            }
            return false;
        }
    }
}
