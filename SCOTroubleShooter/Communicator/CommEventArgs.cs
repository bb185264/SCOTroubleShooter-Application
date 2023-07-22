using System;

namespace SCOTroubleShooter
{
	public sealed class CommEventArgs : EventArgs
	{
		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="CommEventArgs"/> class.
		/// </summary>
		/// <param name="status">The status.</param>
		//----------------------------------------------------------------------------------------------------
		public CommEventArgs(CommStatus status)
		{
			Status = status;
			Message = string.Empty;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="CommEventArgs"/> class.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <param name="message">The message.</param>
		//----------------------------------------------------------------------------------------------------
		public CommEventArgs(CommStatus status, string message)
		{
			Status = status;
			Message = message;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="CommEventArgs"/> class.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <param name="message">The message.</param>
		/// <param name="progressPercentage">The value.</param>
		//----------------------------------------------------------------------------------------------------
		public CommEventArgs(CommStatus status, string message, int progressPercentage)
		{
			Status = status;
			Message = message;
			ProgressPercentage = progressPercentage;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current status of the configuration process.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		public CommStatus Status { get; }

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current asynchronous progress percentage of the configuration process.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		public int ProgressPercentage { get; }

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current message of the configuration process.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		public string Message { get; }
	}

}
