using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace QRCodeReader
{
    public interface ICodeGeneratorService
    {
        public bool GenerateCode(IConfigurationRoot configurationRoot, ILogger logger);
        
    }
}
