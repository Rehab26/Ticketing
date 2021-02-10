
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Logging
{
	/// <summary>
	/// Defines commonly supported types of log entries.
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// Used for tracing code (in Log4Net is maps to <see cref="LogLevel.Debug"/>).
		/// </summary>
		Trace,
		/// <summary>
		/// Diagnostic information used for troubleshooting and debugging purposes.
		/// </summary>
		Debug,
		/// <summary>
		/// Generally useful information tat applies to most log entries.
		/// </summary>
		Info,
		/// <summary>
		/// Information about potentially problematic situations,
		/// such as drive getting full.
		/// </summary>
		Warn,
		/// <summary>
		/// Errors that are not fatal to the service.
		/// </summary>
		Error,
		/// <summary>
		/// Errors that cause a service to shut down.
		/// </summary>
		Fatal
	}
}