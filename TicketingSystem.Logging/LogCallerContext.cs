using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Logging
{
	/// <summary>
	/// Flags indicating the type of info about the caller
	/// that must be automatically captured by the logger.
	/// </summary>
	[Flags]
	public enum LogCallerContext : int
	{
		/// <summary>
		/// Name of the method writing to the log 
		/// (corresponds to <see cref="System.Runtime.CompilerServices.CallerMemberNameAttribute"/>).
		/// For log4net providers, use the following field in the layout's 
		/// <c>conversionPattern</c> value:
		/// <c>%property{callerMemberName}</c>.
		/// </summary>
		MethodName = 0x1,
		/// <summary>
		/// Line number on which a log record was written
		/// (corresponds to <see cref="System.Runtime.CompilerServices.CallerLineNumberAttribute"/>).
		/// For log4net providers, use the following field in the layout's 
		/// <c>conversionPattern</c> value:
		/// <c>%property{callerLineNumber}</c>.
		/// </summary>
		LineNumber = 0x2,
		/// <summary>
		/// Name of the source file writing to the log without the path info
		/// (obtained from <see cref="System.Runtime.CompilerServices.CallerFilePathAttribute"/>).
		/// For log4net providers, use the following field in the layout's 
		/// <c>conversionPattern</c> value:
		/// <c>%property{callerFilePath}</c>.
		/// When <see cref="SourceFilePath"/> is set, this flag will be ignored.
		/// </summary>
		SourceFileName = 0x4,
		/// <summary>
		/// Full path to the source file
		/// (corresponds to <see cref="System.Runtime.CompilerServices.CallerFilePathAttribute"/>).
		/// For log4net providers, use the following field in the layout's 
		/// <c>conversionPattern</c> value:
		/// <c>%property{callerFilePath}</c>.
		/// When this flag is set, <see cref="SourceFileName"/> will be ignored.
		/// </summary>
		SourceFilePath = 0x8
	}
}