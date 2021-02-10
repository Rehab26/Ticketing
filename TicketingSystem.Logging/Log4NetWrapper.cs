using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.CompilerServices;
using log4net;
using log4net.Config;

namespace TicketingSystem.Logging
{
    public class Log4NetWrapper : ILogger
    {
        // Logger instance.
        private ILog _log = null;

        // Cller context instance.
        public LogCallerContext CallerContext { get; set; }

        // Initializes a new instance of the 
        public Log4NetWrapper(Type type)
        {
            _log = log4net.LogManager.GetLogger(type);
            XmlConfigurator.Configure();
        }
        public Log4NetWrapper(string name)
        {
            _log = log4net.LogManager.GetLogger(name);
            XmlConfigurator.Configure();
        }

        protected bool IgnoreLogLevel(LogLevel logLevel)
        {
            if ((logLevel == LogLevel.Trace && !_log.IsDebugEnabled) ||
                (logLevel == LogLevel.Debug && !_log.IsDebugEnabled) ||
                (logLevel == LogLevel.Info && !_log.IsInfoEnabled) ||
                (logLevel == LogLevel.Warn && !_log.IsWarnEnabled) ||
                (logLevel == LogLevel.Error && !_log.IsErrorEnabled) ||
                (logLevel == LogLevel.Fatal && !_log.IsFatalEnabled))
                return true;
            return false;
        }

        private void SetCallerContext(string callerMemberName, string callerFilePath, string callerLineNumber)
        {
            //if (CallerContext.HasFlag(LogCallerContext.MethodName))
                log4net.ThreadContext.Properties["callerMemberName"] = callerMemberName;

            //if (CallerContext.HasFlag(LogCallerContext.LineNumber))
                log4net.ThreadContext.Properties["callerLineNumber"] = callerLineNumber;

            //if (CallerContext.HasFlag(LogCallerContext.SourceFilePath))
                log4net.ThreadContext.Properties["callerFilePath"] = callerFilePath;
            //if (CallerContext.HasFlag(LogCallerContext.SourceFileName))
            //{
            //    log4net.ThreadContext.Properties["callerFilePath"] =
            //        String.IsNullOrEmpty(callerFilePath) ?
            //            callerFilePath :
            //            Path.GetFileName(callerFilePath);
            //}
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (_log == null)
                return false;

            switch (logLevel)
            {
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return _log.IsDebugEnabled;

                case LogLevel.Error:
                    return _log.IsErrorEnabled;

                case LogLevel.Fatal:
                    return _log.IsFatalEnabled;

                case LogLevel.Info:
                    return _log.IsInfoEnabled;

                case LogLevel.Warn:
                    return _log.IsWarnEnabled;
            }

            return false;
        }

        public void SetContext(LogContextType contextType, string name, string value)
        {
            switch (contextType)
            {
                case LogContextType.Global:
                    log4net.GlobalContext.Properties[name] = value;
                    break;

                case LogContextType.LogicalThread:
                    log4net.LogicalThreadContext.Properties[name] = value;
                    break;

                case LogContextType.Thread:
                    log4net.ThreadContext.Properties[name] = value;
                    break;
            }
        }
        public string GetContext(LogContextType contextType, string name)
        {
            if (name == null)
                return null;

            switch (contextType)
            {
                case LogContextType.Global:
                    return GlobalContext.Properties[name] == null ?
                        null : GlobalContext.Properties[name].ToString();

                case LogContextType.LogicalThread:
                    return LogicalThreadContext.Properties[name] == null ?
                        null : LogicalThreadContext.Properties[name].ToString();

                case LogContextType.Thread:
                    return ThreadContext.Properties[name] == null ?
                        null : ThreadContext.Properties[name].ToString();
            }

            return null;
        }
        public string GetContext(string name)
        {
            if (name == null)
                return null;

            if (log4net.LogicalThreadContext.Properties[name] != null)
                return log4net.LogicalThreadContext.Properties[name].ToString();

            if (log4net.ThreadContext.Properties[name] != null)
                return log4net.ThreadContext.Properties[name].ToString();

            if (log4net.GlobalContext.Properties[name] != null)
                return log4net.GlobalContext.Properties[name].ToString();

            return null;
        }

        public void Write
        (LogLevel logLevel, string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            if (IgnoreLogLevel(logLevel))
                return;

            if (message == null)
                return;

            SetCallerContext(callerMemberName, callerFilePath, callerLineNumber.ToString());

            if (logLevel == LogLevel.Trace)
                _log.Debug(message);
            else if (logLevel == LogLevel.Debug)
                _log.Debug(message);
            else if (logLevel == LogLevel.Info)
                _log.Info(message);
            else if (logLevel == LogLevel.Warn)
                _log.Warn(message);
            else if (logLevel == LogLevel.Error)
                _log.Error(message);
            else if (logLevel == LogLevel.Fatal)
                _log.Fatal(message);

            SetCallerContext(null, null, null);
        }

        public void Write(LogLevel logLevel, object message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            if (IgnoreLogLevel(logLevel))
                return;

            if (message == null)
                return;

            Write(logLevel, message.ToString(),
                callerMemberName, callerFilePath, callerLineNumber);
        }

        public void Write(LogLevel logLevel, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            if (IgnoreLogLevel(logLevel))
                return;

            if (ex == null)
                return;

            SetCallerContext(callerMemberName, callerFilePath, callerLineNumber.ToString());

            if (logLevel == LogLevel.Trace)
                _log.Debug(ex);
            else if (logLevel == LogLevel.Debug)
                _log.Debug(ex);
            else if (logLevel == LogLevel.Info)
                _log.Info(ex);
            else if (logLevel == LogLevel.Warn)
                _log.Warn(ex);
            else if (logLevel == LogLevel.Error)
                _log.Error(ex);
            else if (logLevel == LogLevel.Fatal)
                _log.Fatal(ex);

            SetCallerContext(null, null, null);
        }

        public void Write(LogLevel logLevel, string message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            if (IgnoreLogLevel(logLevel))
                return;

            if (message == null && ex == null)
                return;

            SetCallerContext(callerMemberName, callerFilePath, callerLineNumber.ToString());

            if (ex == null)
            {
                if (logLevel == LogLevel.Trace)
                    _log.Debug(message);
                else if (logLevel == LogLevel.Debug)
                    _log.Debug(message);
                else if (logLevel == LogLevel.Info)
                    _log.Info(message);
                else if (logLevel == LogLevel.Warn)
                    _log.Warn(message);
                else if (logLevel == LogLevel.Error)
                    _log.Error(message);
                else if (logLevel == LogLevel.Fatal)
                    _log.Fatal(message);
            }
            else
            {
                if (logLevel == LogLevel.Trace)
                    _log.Debug(message, ex);
                else if (logLevel == LogLevel.Debug)
                    _log.Debug(message, ex);
                else if (logLevel == LogLevel.Info)
                    _log.Info(message, ex);
                else if (logLevel == LogLevel.Warn)
                    _log.Warn(message, ex);
                else if (logLevel == LogLevel.Error)
                    _log.Error(message, ex);
                else if (logLevel == LogLevel.Fatal)
                    _log.Fatal(message, ex);
            }

            SetCallerContext(null, null, null);
        }

        public void Write(LogLevel logLevel, object message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            if (IgnoreLogLevel(logLevel))
                return;

            if (message == null && ex == null)
                return;

            Write(logLevel, message.ToString(), ex,
                callerMemberName, callerFilePath, callerLineNumber);
        }

        public void Debug(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Debug(message);
        }

        public void Debug(object message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Debug(message);
        }

        public void Debug(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Debug(ex);
        }

        public void Debug(string message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Debug(message);
        }

        public void Debug(object message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Debug(message);
        }

        public void Error(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Error(message);
        }

        public void Error(object message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Error(message);
        }

        public void Error(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Error(ex);
        }

        public void Error(string message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Error(message);
        }

        public void Error(object message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Error(message);
        }

        public void Fatal(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Fatal(message);
        }

        public void Fatal(object message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Fatal(message);
        }

        public void Fatal(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Fatal(ex);
        }

        public void Fatal(string message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Fatal(message);
        }

        public void Fatal(object message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Fatal(message);
        }

        public void Info(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Info(message);
        }

        public void Info(object message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Info(message);
        }

        public void Info(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Info(ex);
        }

        public void Info(string message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Info(message);
        }

        public void Info(object message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Info(message);
        }

        public void Trace(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Info(message);
        }

        public void Trace(object message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Info(message);
        }

        public void Trace(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            throw new NotImplementedException();
        }

        public void Trace(object message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Warn(message);
        }

        public void Warn(object message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Warn(message);
        }

        public void Warn(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Warn(ex);
        }

        public void Warn(string message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Warn(message);
        }

        public void Warn(object message, Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            _log.Warn(message);
        }

        public void SetContext<TKey, TValue>(LogContextType contextType, KeyValuePair<TKey, TValue> property)
        {
            throw new NotImplementedException();
        }

        public void SetContext(LogContextType contextType, Dictionary<string, string> properties)
        {
            throw new NotImplementedException();
        }

        public void SetContext(LogContextType contextType, Dictionary<string, object> properties)
        {
            throw new NotImplementedException();
        }

        public void SetContext(LogContextType contextType, NameValueCollection properties)
        {
            throw new NotImplementedException();
        }

        public string GetContext(LogContextType contextType, object name)
        {
            throw new NotImplementedException();
        }

        public string GetContext(object name)
        {
            throw new NotImplementedException();
        }

        public void SetContext(LogContextType contextType, string name, object value)
        {
            throw new NotImplementedException();
        }
    }
}