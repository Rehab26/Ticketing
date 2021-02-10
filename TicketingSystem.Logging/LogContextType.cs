using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Logging
{
	/// <summary>
	/// Defines the accessibility scope for custom properties that can be written to the logs.
	/// </summary>
	/// <remarks>
	/// For additional information, see
	/// <see href="https://logging.apache.org/log4net/release/manual/contexts.html">Contexts</see>
	/// in Apache log4net Manual.
	/// </remarks>
	public enum LogContextType
	{
		/// <summary>
		/// Accessible from all threads.
		/// </summary>
		Global,
		/// <summary>
		/// Accessible from the current physical thread only.
		/// </summary>
		/// <remarks>
		/// This scope is most appropriate for properties that
		/// can be changed during the thread execution.
		/// </remarks>
		Thread,
		/// <summary>
		/// Accessible from the primary thread and all child threads.
		/// </summary>
		/// <remarks>
		/// Because other threads can modify these properties
		/// before they get written to a log,
		/// this context is best suited for properties
		/// that do not change within the execution scope.
		/// </remarks>
		LogicalThread
	}
}