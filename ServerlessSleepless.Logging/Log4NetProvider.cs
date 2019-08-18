using System.IO;
using Microsoft.Extensions.Logging;

namespace ServerlessSleepless.Logging
{
    public class Log4netProvider : ILoggerProvider
    {
        protected readonly FileInfo m_fileInfo;

        public Log4netProvider(string log4netConfigFile)
        {
            m_fileInfo = new FileInfo(log4netConfigFile);
        }

        public ILogger CreateLogger(string name)
        {
            return new Log4netAdapter(name, m_fileInfo);
        }

        public void Dispose()
        {
        }
    }
}
