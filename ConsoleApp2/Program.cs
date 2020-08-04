using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;


namespace QRCodeReader
{
    class Program
    {
        public static IConfigurationRoot configuration;
        public static ILogger<Program> _logger;

        static void Main(string[] args)
        {
            try
            {
                // Add Service Collection to configure Configuration Services
                ServiceCollection serviceCollection = new ServiceCollection();               
                serviceCollection.AddTransient<ICodeGeneratorService, CodeGeneratorService>();
                serviceCollection.AddTransient<ICodeReaderService, CodeReaderService>();
                ConfigureServices(serviceCollection);    
                IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

                _logger = serviceProvider.GetService<ILogger<Program>>();
                _logger.LogInformation($"Application Started at: {DateTime.UtcNow.ToString()}");

                Console.WriteLine("*****Welcome to QR Code Reader/Generator Appication****");
                Console.WriteLine("Press any key to continue..");
                Console.ReadKey();
                ConsoleKeyInfo userSelection;
                
                var codeReaderService = serviceProvider.GetService<ICodeReaderService>();
                var codeGeneratorService = serviceProvider.GetService<ICodeGeneratorService>();

                userSelection = Helper.GetUserSelection();
                var isContinued = false;
                Helper.ProcessUserSelection(codeReaderService, codeGeneratorService, configuration, userSelection, _logger);
                do
                {
                    Console.WriteLine("\n" + "Do you wish to continue? Please enter " + "Y" + " for Yes " + "N" + " for No");
                    var isContinuedByUser = Console.ReadKey();
                    isContinued = false;
                    if ("y" == isContinuedByUser.KeyChar.ToString().ToLower())
                    {
                        isContinued = true;
                        userSelection = Helper.GetUserSelection();
                        Helper.ProcessUserSelection(codeReaderService, codeGeneratorService, 
                            configuration, userSelection, _logger);
                    }                    
                }
                while (isContinued);                
            }
            catch (Exception ex)
            {
                _logger.LogError("\n" + "Exception Occured: There was problem with the file / request", ex.Message);
                Console.WriteLine("\n" + "Exception Occured: There was problem with the file/request", ex.Message);
            }
        }     

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging(configure => configure.AddConsole())
               .AddTransient<Program>();

            // Build configuration
            configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json", false)
            .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton(configuration);
        }
    }
}

