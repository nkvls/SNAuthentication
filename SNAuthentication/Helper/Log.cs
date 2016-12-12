using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SNAuthentication.Helper
{
    public interface ILog
    {
        void Exception(Exception exception);
        void Error(string message);
        void Debug(string message);
        void Warn(string message);
        void Warn(string message, Exception exception);
        bool IsDebugEnabled();
    }

    public class Log : ILog, IDisposable
    {
        private readonly log4net.ILog _log4NetLogger;

        public Log(log4net.ILog log4NetLogger)
        {
            _log4NetLogger = log4NetLogger;
        }

        public void Exception(Exception exception)
        {
            _log4NetLogger.Error(exception);
        }

        public void Error(string message)
        {
            _log4NetLogger.Error(message);
        }

        public void Debug(string message)
        {
            if (_log4NetLogger.IsDebugEnabled)
                _log4NetLogger.Debug(message);
        }
        public void Warn(string message)
        {
            _log4NetLogger.Warn(message);
        }

        public void Warn(string message, Exception exception)
        {
            _log4NetLogger.Warn(message, exception);
        }

        public bool IsDebugEnabled()
        {
            return _log4NetLogger.IsDebugEnabled;
        }

        public void Dispose()
        {
            _log4NetLogger.Logger.Repository.Shutdown();
        }
    }
}