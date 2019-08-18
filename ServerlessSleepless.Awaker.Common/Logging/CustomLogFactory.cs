using Microsoft.Extensions.Logging;
using System;

namespace ServerlessSleepless.Awaker.Common.Logging
{
    public class CustomLogFactory
    {
        public static ILoggerFactory LoggerFactory = new LoggerFactory();

        public static ILogger CreateLogger<T>()
        {
            return CreateLogger(typeof(T));
        }

        public static ILogger CreateLogger(Type type)
        {
            return LoggerFactory.CreateLogger(type);    
        }
    }
}
