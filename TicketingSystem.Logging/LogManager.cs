using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Logging
{
	/// <summary>
	/// Implements a log factory responsible for generating a logger instance.
	/// </summary>
	public static class LogManager
	{
		/// <summary>
		/// The default logging provider.
		/// </summary>
		private static LogProvider _defaultProvider = LogProvider.Log4Net;

		/// <summary>
		/// Gets or sets the default logging provider.
		/// </summary>
		/// <remarks>
		/// Only <c>log4net</c> is supported at this time.
		/// </remarks>
		public static LogProvider DefaultProvider
		{
			get
			{
				return _defaultProvider;
			}

			set
			{
				_defaultProvider = value;
			}
		}

		/// <overloads>
		/// Generates a logger instance.
		/// </overloads>
		/// <summary>
		/// Returns a logger instance of the specified logging provider 
		/// matching a given name.
		/// </summary>
		/// <param name="provider">
		/// Name of the logging provider.
		/// </param>
		/// <param name="name">
		/// Logger name.
		/// When not specified, the full name of the caller class will be
		/// used (it's a bit slow and therefore not recommended).
		/// </param>
		/// <returns>
		/// Logger instance.
		/// </returns>
		/// <remarks>
		/// Currently, only an instance of
		/// <see cref="BasicLogging.Log4NetWrapper"/>
		/// can be returned.
		/// </remarks>
		public static ILogger GetLogger(LogProvider provider, string name = null)
		{
			if (String.IsNullOrEmpty(name))
				return GetLogger(provider, (new StackFrame(1)).GetMethod().DeclaringType);

			switch (provider)
			{
				case LogProvider.Log4Net: return new Log4NetWrapper(name);
			}

			return null;
		}

#pragma warning disable 1573
		/// <inheritdoc cref="GetLogger(LogProvider,string)" select="param|returns|remarks"/> 
		/// <summary>
		/// Returns a logger instance of the default logging provider 
		/// matching a given name.
		/// </summary>		
		public static ILogger GetLogger
		(
			string name = null
		)
		{
			return GetLogger(DefaultProvider, name);
		}
#pragma warning restore 1573

#pragma warning disable 1573
		/// <inheritdoc cref="GetLogger(LogProvider,string)" select="param|returns|remarks"/> 
		/// <summary>
		/// Returns a logger instance of the specified logging provider 
		/// matching a given type.
		/// </summary>
		/// <param name="type">
		/// Logger type.
		/// </param>
		public static ILogger GetLogger
		(LogProvider provider,
			Type type
		)
		{
			switch (provider)
			{
				case LogProvider.Log4Net:
					return new Log4NetWrapper(type);
			}

			return null;
		}
#pragma warning restore 1573

#pragma warning disable 1573
		/// <inheritdoc cref="GetLogger(LogProvider,Type)" select="param|returns|remarks"/> 
		/// <summary>
		/// Returns a logger instance of the default logging provider 
		/// matching a given type.
		/// </summary>		
		public static ILogger GetLogger
		(
			Type type
		)
		{
			return GetLogger(DefaultProvider, type);
		}
#pragma warning restore 1573
	}
}