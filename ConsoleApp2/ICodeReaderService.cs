using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace QRCodeReader
{
    public interface ICodeReaderService
    {
        public bool ReadCode(IConfigurationRoot configurationRoot, ILogger logger);
    }
}
