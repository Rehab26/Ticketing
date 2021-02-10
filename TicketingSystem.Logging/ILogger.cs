using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace TicketingSystem.Logging
{
    /// <summary>
    /// Defines common log methods.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets or sets the details about the caller context 
        /// (such as calling method or line number)
        /// that will be exposed to the logging provider
        /// and may be written to the logs.
        /// </summary>
        /// <value>
        /// Details about the caller context that must be captured.
        /// </value>
        /// <remarks>
        /// <para>
        /// While this setting dictates what information about 
        /// the caller must be made available to the log formatter,
        /// whether this information is included in the log
        /// depends on the log configuration.
        /// </para>
        /// <para>
        /// For security reasons, it is recommended to only
        /// capture the source file name (not the full path)
        /// in the default logger configuration.
        /// </para>
        /// </remarks>
        LogCallerContext CallerContext { get; set; }

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes a debug text message to the log.
        /// </summary>
#pragma warning restore 1573
        void Debug
        (
            string message,
            [CallerMemberName]
            string  callerMemberName    = "",
            [CallerFilePath]
            string  callerFilePath      = "",
            [CallerLineNumber]
            int     callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,object,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Converts the specified <paramref name="message"/> to a string
        /// and writes it to the log as a debug message.
        /// </summary>
#pragma warning restore 1573
        void Debug
        (
            object message,
            [CallerMemberName]
            string  callerMemberName    = "",
            [CallerFilePath]
            string  callerFilePath      = "",
            [CallerLineNumber]
            int     callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,Exception,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes an exception to the log as a debug message.
        /// </summary>
#pragma warning restore 1573
        void Debug
        (
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,Exception,string,string,int)" select="param|remarks"/>
        /// <summary>
        /// Writes text and exception to the log as a debug message.
        /// </summary>
#pragma warning restore 1573
        void Debug
        (
            string message,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,object,Exception,string,string,int)" select="param|remarks"/> 
        /// <overloads>
        /// Writes a debug record to the log.
        /// These methods are simple shortcuts to the 
        /// <see cref="O:BasicLogging.ILogger.Write"/> method called
        /// with the <see cref="BasicLogging.LogLevel.Debug"/> flag.
        /// </overloads>
        /// <summary>
        /// Converts the specified <paramref name="message"/> to a string
        /// and writes it along with an exception to the log as a debug message.
        /// </summary>
#pragma warning restore 1573
        void Debug
        (
            object message,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes a non-fatal error text message to the log.
        /// </summary>
#pragma warning restore 1573
        void Error
        (
            string message,
            [CallerMemberName]
            string  callerMemberName    = "",
            [CallerFilePath]
            string  callerFilePath      = "",
            [CallerLineNumber]
            int     callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,object,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Converts the specified <paramref name="message"/> to a string
        /// and writes it to the log as a non-fatal error message.
        /// </summary>
#pragma warning restore 1573
        void Error
        (
            object message,
            [CallerMemberName]
            string  callerMemberName    = "",
            [CallerFilePath]
            string  callerFilePath      = "",
            [CallerLineNumber]
            int     callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,Exception,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes an exception to the log as a non-fatal error message.
        /// </summary>
#pragma warning restore 1573
        void Error
        (
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,Exception,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes text and exception to the log as a non-fatal error message.
        /// </summary>
#pragma warning restore 1573
        void Error
        (
            string message,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,object,Exception,string,string,int)" select="param|remarks"/> 
        /// <overloads>
        /// Writes a non-fatal error record to the log.
        /// These methods are simple shortcuts to the 
        /// <see cref="O:BasicLogging.ILogger.Write"/> method called
        /// with the <see cref="BasicLogging.LogLevel.Error"/> flag.
        /// </overloads>
        /// <summary>
        /// Converts the specified <paramref name="message"/> to a string
        /// and writes it along with an exception to the log as a non-fatal error message.
        /// </summary>	
#pragma warning restore 1573
        void Error
        (
            object message,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes a fatal error text message to the log.
        /// </summary>
#pragma warning restore 1573
        void Fatal
        (
            string message,
            [CallerMemberName]
            string  callerMemberName    = "",
            [CallerFilePath]
            string  callerFilePath      = "",
            [CallerLineNumber]
            int     callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,object,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Converts the specified <paramref name="message"/> to a string
        /// and writes it to the log as a fatal error message.
        /// </summary>
#pragma warning restore 1573
        void Fatal
        (
            object message,
            [CallerMemberName]
            string  callerMemberName    = "",
            [CallerFilePath]
            string  callerFilePath      = "",
            [CallerLineNumber]
            int     callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,Exception,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes an exception to the log as a fatal error message.
        /// </summary>
#pragma warning restore 1573
        void Fatal
        (
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,Exception,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes text and exception to the log as a fatal error message.
        /// </summary>
#pragma warning restore 1573
        void Fatal
        (
            string message,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,object,Exception,string,string,int)" select="param|remarks"/> 
        /// <overloads>
        /// Writes a fatal error record to the log.
        /// These methods are simple shortcuts to the 
        /// <see cref="O:BasicLogging.ILogger.Write"/> method called
        /// with the <see cref="BasicLogging.LogLevel.Fatal"/> flag.
        /// </overloads>
        /// <summary>
        /// Converts the specified <paramref name="message"/> to a string
        /// and writes it along with an exception to the log as a fatal error message.
        /// </summary>
#pragma warning restore 1573
        void Fatal
        (
            object message,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes an informational text message to the log.
        /// </summary>
#pragma warning restore 1573
        void Info
        (
            string message,
            [CallerMemberName]
            string  callerMemberName    = "",
            [CallerFilePath]
            string  callerFilePath      = "",
            [CallerLineNumber]
            int     callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,object,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Converts the specified <paramref name="message"/> to a string
        /// and writes it to the log as an informational message.
        /// </summary>
#pragma warning restore 1573
        void Info
        (
            object message,
            [CallerMemberName]
            string  callerMemberName    = "",
            [CallerFilePath]
            string  callerFilePath      = "",
            [CallerLineNumber]
            int     callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,Exception,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes an exception to the log as an informational message.
        /// </summary>
#pragma warning restore 1573
        void Info
        (
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,Exception,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes text and exception to the log as an informational message.
        /// </summary>
#pragma warning restore 1573
        void Info
        (
            string message,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,object,Exception,string,string,int)" select="param|remarks"/> 
        /// <overloads>
        /// Writes an informational record to the log.
        /// These methods are simple shortcuts to the 
        /// <see cref="O:BasicLogging.ILogger.Write"/> method called
        /// with the <see cref="BasicLogging.LogLevel.Info"/> flag.
        /// </overloads>
        /// <summary>
        /// Converts the specified <paramref name="message"/> to a string
        /// and writes it along with an exception to the log as an informational message.
        /// </summary>
#pragma warning restore 1573
        void Info
        (
            object message,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes a trace text message to the log.
        /// </summary>
#pragma warning restore 1573
        void Trace
        (
            string message,
            [CallerMemberName]
            string  callerMemberName    = "",
            [CallerFilePath]
            string  callerFilePath      = "",
            [CallerLineNumber]
            int     callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,object,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Converts the specified <paramref name="message"/> to a string
        /// and writes it to the log as a trace message.
        /// </summary>
#pragma warning restore 1573
        void Trace
        (
            object message,
            [CallerMemberName]
            string  callerMemberName    = "",
            [CallerFilePath]
            string  callerFilePath      = "",
            [CallerLineNumber]
            int     callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,Exception,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes an exception to the log as a trace message.
        /// </summary>
#pragma warning restore 1573
        void Trace
        (
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,Exception,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes text and exception to the log as a trace message.
        /// </summary>
#pragma warning restore 1573
        void Trace
        (
            string message,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,object,Exception,string,string,int)" select="param|remarks"/> 
        /// <overloads>
        /// Writes a trace record to the log.
        /// These methods are simple shortcuts to the 
        /// <see cref="O:BasicLogging.ILogger.Write"/> method called
        /// with the <see cref="BasicLogging.LogLevel.Trace"/> flag.
        /// </overloads>
        /// <summary>
        /// Converts the specified <paramref name="message"/> to a string
        /// and writes it along with an exception to the log as a trace message.
        /// </summary>
#pragma warning restore 1573
        void Trace
        (
            object message,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes a warning text message to the log.
        /// </summary>
#pragma warning restore 1573
        void Warn
        (
            string message,
            [CallerMemberName]
            string  callerMemberName    = "",
            [CallerFilePath]
            string  callerFilePath      = "",
            [CallerLineNumber]
            int     callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,object,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Converts the specified <paramref name="message"/> to a string
        /// and writes it to the log as a warning message.
        /// </summary>
#pragma warning restore 1573
        void Warn
        (
            object message,
            [CallerMemberName]
            string  callerMemberName    = "",
            [CallerFilePath]
            string  callerFilePath      = "",
            [CallerLineNumber]
            int     callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,Exception,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes an exception to the log as a warning message.
        /// </summary>
#pragma warning restore 1573
        void Warn
        (
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,Exception,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes text and exception to the log as a warning message.
        /// </summary>
#pragma warning restore 1573
        void Warn
        (
            string message,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,object,Exception,string,string,int)" select="param|remarks"/> 
        /// <overloads>
        /// Writes a warning record to the log.
        /// These methods are simple shortcuts to the 
        /// <see cref="O:BasicLogging.ILogger.Write"/> method called
        /// with the <see cref="BasicLogging.LogLevel.Warn"/> flag.
        /// </overloads>
        /// <summary>
        /// Converts the specified <paramref name="message"/> to a string
        /// and writes it along with an exception to the log as a warning message.
        /// </summary>
#pragma warning restore 1573
        void Warn
        (
            object message,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

        /// <summary>
        /// Determines whether the specified log level is enabled.
        /// </summary>
        /// <param name="logLevel">
        /// The log level to be checked.
        /// </param>
        /// <returns>
        ///   <c>true</c> if the specified log level is enabled; otherwise, <c>false</c>.
        /// </returns>
        bool IsEnabled
        (
            LogLevel logLevel
        );

        /// <overloads>
        /// Writes a record to the log.
        /// </overloads>
        /// <summary>
        /// Writes a text message to the log.
        /// </summary>
        /// <param name="logLevel">
        /// Log level of the log record.
        /// </param>
        /// <param name="message">
        /// Log message.
        /// </param>
        /// <param name="callerMemberName">
        /// Name of the caller method.
        /// Omit to use the default.
        /// </param>
        /// <param name="callerFilePath">
        /// File path of the caller.
        /// Omit to use the default.
        /// </param>
        /// <param name="callerLineNumber">
        /// Line number of the statement making the call.
        /// Omit to use the default.
        /// </param>
        /// <remarks>
        /// <para>
        /// To add custom properties to a log record, use a 
        /// <see cref="O:BasicLogging.ILogger.SetContext"/> method before making this call.
        /// </para>
        /// <para>
        /// When not specified <paramref name="callerMemberName"/>,
        /// <paramref name="callerFilePath"/>,
        /// <paramref name="callerLineNumber"/> values will
        /// reflect the caller information.
        /// Generally, you should not override those.
        /// When using log4net, you can reference them in the log template
        /// via the <c>%property{name}</c> placeholder,
        /// where <c>name</c> is either <c>callerMemberName</c>,
        /// <c>callerFilePath</c>, or <c>callerLineNumber</c>.
        /// For more information about these properties, 
        /// see
        /// <see cref="System.Runtime.CompilerServices.CallerMemberNameAttribute"/>,
        /// <see cref="System.Runtime.CompilerServices.CallerFilePathAttribute"/>, and
        /// <see cref="System.Runtime.CompilerServices.CallerLineNumberAttribute"/>
        /// </para>
        /// </remarks>
        void Write
        (
            LogLevel logLevel,
            string message,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Converts the message object to a string and writes it to the log.
        /// </summary>
        /// <param name="message">
        /// Object to be converted to a log message.
        /// </param>
        void Write
        (
            LogLevel logLevel,
            object message,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );
#pragma warning restore 1573

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes an exception to the log.
        /// </summary>
        /// <param name="ex">
        /// Error information.
        /// </param>
        void Write
        (
            LogLevel logLevel,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );
#pragma warning restore 1573

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Writes text and exception to the log.
        /// </summary>
        /// <param name="ex">
        /// Error information.
        /// </param>
        void Write
        (
            LogLevel logLevel,
            string message,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );
#pragma warning restore 1573

#pragma warning disable 1573
        /// <inheritdoc cref="Write(LogLevel,string,Exception,string,string,int)" select="param|remarks"/> 
        /// <summary>
        /// Converts the message object to a string and writes it along with an exception to the log.
        /// </summary>
        void Write
        (
            LogLevel logLevel,
            object message,
            Exception ex,
            [CallerMemberName]
            string      callerMemberName    = "",
            [CallerFilePath]
            string      callerFilePath      = "",
            [CallerLineNumber]
            int         callerLineNumber    = 0
        );
#pragma warning restore 1573

        /// <overloads>
        /// Sets values of custom properties that can be logged implicitly.
        /// </overloads>
        /// <summary>
        /// Sets the string value of a custom property that can be logged implicitly.
        /// </summary>
        /// <param name="contextType">
        /// Property accessibility scope.
        /// </param>
        /// <param name="name">
        /// Property name.
        /// </param>
        /// <param name="value">
        /// Property value.
        /// </param>
        /// <remarks>
        /// Custom properties can be referenced in the log templates using
        /// standard logger syntax. For example, for log4net loggers,
        /// they can be referenced as <c>%property{name}</c>, 
        /// where <c>name</c> is the name of the property.
        /// </remarks>
        void SetContext
        (
            LogContextType contextType,
            string name,
            string value
        );

#pragma warning disable 1573
        /// <inheritdoc cref="SetContext(LogContextType,string,string)" select="param|remarks"/> 
        /// <summary>
        /// Sets the value of a custom string property that can be logged implicitly.
        /// </summary>
        void SetContext
        (
            LogContextType contextType,
            string name,
            object value
        );
#pragma warning restore 1573

#pragma warning disable 1573
        /// <inheritdoc cref="SetContext(LogContextType,string,string)" select="param|remarks"/> 
        /// <summary>
        /// Sets the values of the custom string properties that can be logged implicitly
        /// using a generic key-value pair array.
        /// </summary>
        /// <typeparam name="TKey">
        /// Type of the property name.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of the property value.
        /// </typeparam>
        /// <param name="property">
        /// Property name value pair.
        /// </param>
        void SetContext<TKey, TValue>
        (
            LogContextType contextType,
            KeyValuePair<TKey, TValue> property
        );
#pragma warning restore 1573

#pragma warning disable 1573
        /// <inheritdoc cref="SetContext(LogContextType,string,string)" select="param|remarks"/>
        /// <summary>
        /// Sets the values of the custom string properties that can be logged implicitly
        /// using a string dictionary.
        /// </summary>
        /// <param name="properties">
        /// Collection of name/value pairs holding custom properties.
        /// </param>
        void SetContext
        (
            LogContextType contextType,
            Dictionary<string, string> properties
        );
#pragma warning restore 1573

#pragma warning disable 1573
        /// <inheritdoc cref="SetContext(LogContextType,Dictionary{string,string})" select="param|remarks"/>
        /// <summary>
        /// Sets the values of the custom string properties that can be logged implicitly
        /// using an object dictionary.
        /// </summary>
        void SetContext
        (
            LogContextType contextType,
            Dictionary<string, object> properties
        );
#pragma warning restore 1573

#pragma warning disable 1573
        /// <inheritdoc cref="SetContext(LogContextType,Dictionary{string,string})" select="param|remarks"/>
        /// <summary>
        /// Sets the values of the custom string properties that can be logged implicitly
        /// using a name-value collection.
        /// </summary>
        void SetContext
        (
            LogContextType contextType,
            NameValueCollection properties
        );
#pragma warning restore 1573

        /// <summary>
        /// Gets the value of the custom context property 
        /// defined in the specified context scope.
        /// </summary>
        /// <overloads>
        /// Gets the value of a custom context property.
        /// </overloads>
        /// <param name="contextType">
        /// Property accessibility scope.
        /// </param>
        /// <param name="name">
        /// Property name.
        /// </param>
        /// <returns>
        /// Property value.
        /// </returns>
        string GetContext
        (
            LogContextType contextType,
            string name
        );

#pragma warning disable 1573
        /// <inheritdoc cref="GetContext(LogContextType,string)"/>
        string GetContext
        (
            LogContextType contextType,
            object name
        );
#pragma warning restore 1573

#pragma warning disable 1573
        /// <inheritdoc cref="GetContext(string)"/>
        string GetContext
        (
            object name
        );
#pragma warning restore 1573

#pragma warning disable 1573
        /// <inheritdoc cref="GetContext(LogContextType,string)" select="param|returns"/>
        /// <summary>
        /// Gets the value of the custom context property 
        /// defined in any context scope.
        /// </summary>
        string GetContext
        (
            string name
        );
#pragma warning restore 1573
    }
}