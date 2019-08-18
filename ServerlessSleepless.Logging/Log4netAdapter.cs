using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Logging;

namespace ServerlessSleepless.Logging
{
    public class Log4netAdapter : ILogger
    {
        protected readonly ILog _logger;

        public Log4netAdapter(string name, FileInfo fileInfo)
        {
            var repository = LogManager.CreateRepository(
                    Assembly.GetEntryAssembly(),
                    typeof(log4net.Repository.Hierarchy.Hierarchy)
                );
            XmlConfigurator.Configure(repository, fileInfo);
            _logger = LogManager.GetLogger(repository.Name, name);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical: return _logger.IsFatalEnabled;
                case LogLevel.Debug:
                case LogLevel.Trace: return _logger.IsDebugEnabled;
                case LogLevel.Error: return _logger.IsErrorEnabled;
                case LogLevel.Information: return _logger.IsInfoEnabled;
                case LogLevel.Warning: return _logger.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
        {
            if (false == IsEnabled(logLevel))
            {
                return;
            }

            if (null == formatter)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = null;

            if (null != formatter)
            {
                message = formatter(state, exception);
            }

            if (false == string.IsNullOrEmpty(message) || null != exception)
            {
                switch (logLevel)
                {
                    case LogLevel.Critical: _logger.Fatal(message); break;
                    case LogLevel.Debug:
                    case LogLevel.Trace: _logger.Debug(message); break;
                    case LogLevel.Error: _logger.Error(message); break;
                    case LogLevel.Information: _logger.Info(message); break;
                    case LogLevel.Warning: _logger.Warn(message); break;
                    default:
                        _logger.Warn($"Unknown log level {logLevel}.{Environment.NewLine}{message}");
                        break;
                }
            }
        }
    }
}
