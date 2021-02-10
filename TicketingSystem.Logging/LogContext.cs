using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace TicketingSystem.Logging
{/// <summary>
 /// Implements helper methods useful for getting caller context information.
 /// </summary>
	public sealed class LogContext
	{
		/// <summary>
		/// Prevents a default instance of the <see cref="LogContext"/> 
		/// class from being created.
		/// </summary>
		private LogContext()
		{
		}

		/// <summary>
		/// Returns the name of the calling method.
		/// </summary>
		/// <param name="callerMemberName">
		/// Must be omitted.
		/// </param>
		/// <returns>
		/// Name of the calling method.
		/// </returns>
		public static string GetMethodName
		(
			[CallerMemberName]
			string callerMemberName = null
		)
		{
			return callerMemberName;
		}

		/// <summary>
		/// Returns the file path of the source code.
		/// </summary>
		/// <param name="callerFilePath">
		/// Must be omitted.
		/// </param>
		/// <returns>
		/// The caller file path.
		/// </returns>
		public static string GetFilePath
		(
			[CallerFilePath]
			string callerFilePath = null
		)
		{
			return callerFilePath;
		}

		/// <summary>
		/// Returns the line number of the caller in the source code.
		/// </summary>
		/// <param name="callerLineNumber">
		/// Must be omitted.
		/// </param>
		/// <returns>
		/// Line number of the caller in the source code.
		/// </returns>
		public static int GetLineNumber
		(
			[CallerLineNumber]
			int callerLineNumber = 0
		)
		{
			return callerLineNumber;
		}
	}
}