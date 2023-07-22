using System;

namespace SCOTroubleShooter
{
	public sealed class ComEventArgs : EventArgs
	{
		public enum ComStatus
		{
			Alarm,
			AboutToDisconnect,
			Connected,
			Data,
			Disconnected,
			DownloadComplete,
			Error,
			Status,
			StatusError,
			WatchdogReconnect,
			WatchdogTimeout,
			ControllerInfo
		}
		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current status of the configuration process.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		public ComStatus Status { get; }

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current message of the configuration process.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		public string Message { get; }

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current status message ID for the status event.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		public int StatusID { get; }

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current message error ID for the status event.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		public int ErrorID { get; }

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current message data the status event.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		public Array Data { get; }

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ComStatus"/> class.
		/// </summary>
		/// <param name="status">The status.</param>
		//----------------------------------------------------------------------------------------------------
		public ComEventArgs(ComStatus status)
		{
			Status = status;
			Message = string.Empty;
			Data = null;
			StatusID = -1;
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ComStatus"/> class.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <param name="message">The message.</param>
		//----------------------------------------------------------------------------------------------------
		public ComEventArgs(ComStatus status, string message)
		{
			Status = status;
			Message = message;
			Data = null;
			StatusID = -1;
			ErrorID = 0;
		}


		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ComStatus"/> class.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <param name="statusId">The Status ID.</param>
		/// <param name="errorId">The Status Error ID.</param>
		//----------------------------------------------------------------------------------------------------
		public ComEventArgs(ComStatus status, int statusId, int errorId)
		{
			Status = status;
			Message = string.Empty;
			Data = null;
			StatusID = statusId;
			ErrorID = errorId;
		}
		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ComStatus"/> class.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <param name="statusId">The Status ID.</param>
		/// <param name="data">The Status Data object.</param>
		//----------------------------------------------------------------------------------------------------
		public ComEventArgs(ComStatus status, int statusId, Array data)
		{
			Status = status;
			Message = string.Empty;
			StatusID = statusId;
			Data = data;
			ErrorID = 0;
		}
	}
}
