using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace QRCodeReader
{
    public static class Helper
    {
        /// <summary>
        /// handles the flow of user selection for code generator/reader
        /// </summary>
        /// <param name="codeReaderService"></param>
        /// <param name="codeGeneratorService"></param>
        /// <param name="configuration"></param>
        /// <param name="userSelection"></param>

        public static void ProcessUserSelection(ICodeReaderService codeReaderService, ICodeGeneratorService codeGeneratorService,
             IConfigurationRoot configuration, ConsoleKeyInfo userSelection, ILogger logger)
        {
            switch (userSelection.KeyChar)
            {
                case '1':                    
                    codeReaderService.ReadCode(configuration, logger);
                    break;
                case '2':                    
                    codeGeneratorService.GenerateCode(configuration, logger);
                    break;
                default:
                    break;
            }            
        }

        /// <summary>
        /// limites user selection to the options available
        /// </summary>
        /// <returns></returns>
        public static ConsoleKeyInfo GetUserSelection()
        {
            ConsoleKeyInfo userSelection;
            do
            {
                Console.WriteLine("\n" + "Please Press 1 for QR Code Reader OR 2 for QR Code Generator");
                userSelection = Console.ReadKey();
            }
            while (!(userSelection.KeyChar == '1' || userSelection.KeyChar == '2'));
            return userSelection;
        }       

    }
}
