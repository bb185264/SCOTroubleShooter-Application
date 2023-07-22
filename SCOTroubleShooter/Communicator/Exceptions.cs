using System;

namespace SCOTroubleShooter
{
	//..................................................................................................................................

	#region Public Exceptions

	//----------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Represents the errors that occur when an instance of communication manager has not been created.
	/// </summary>
	//----------------------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NullReferenceException : Exception
	{
		public override string Message => "Communication Manager has not been not created.";
	}

	//----------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Represents the errors that occur when a port was not previously opened.
	/// </summary>
	//----------------------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class PortNotOpenException : Exception
	{
		public override string Message => "Port has not been opened.";
	}

	//----------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Represents the errors that occur when a connection to the drive could not be performed.
	/// </summary>
	//----------------------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class CouldNotConnectException : Exception
	{
		public CouldNotConnectException()
		{
			Message = "Could not connect to the drive.";
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a message that describes the current exception.
		/// </summary>
		/// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
		//------------------------------------------------------------------------------------------------------------------------
		public override string Message { get; }

		//public override void GetObjectData(SerializationInfo info, StreamingContext context)
		//{
		//    base.GetObjectData(info, context);
		//}
	}

	//----------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Represents errors that occur when the drive does not respond in the alloted time.
	/// </summary>
	//----------------------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class TimeoutException : Exception
	{
		//private readonly string _message;
		public override string Message => "Drive did not respond in the alloted time.";
	}

	//----------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Represents errors that occur when a specific drive based on the drive type cannot be found connected to the com port.
	/// </summary>
	//----------------------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class DriveNotFoundException : Exception
	{
		public DriveNotFoundException()
		{
			Message = "Drive could not be found.";
		}

		public DriveNotFoundException(string message)
		{
			Message = message;
		}

		public override string Message { get; }
	}

	[Serializable]
	public class AddressNotFoundException : Exception
	{
		public AddressNotFoundException()
		{
			Message = "Address could not be found.";
		}

		public AddressNotFoundException(string message)
		{
			Message = message;
		}

		public override string Message { get; }
	}

	//----------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Represents errors that occur when the communications drive type does not match the project drive
	/// type specified within the client application.
	/// </summary>
	//----------------------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class DriveTypeMismatchException : Exception
	{
		public DriveTypeMismatchException()
		{
			Message = "The Project Drive Type does not match the Drive Type it is currently connected to.";
		}

		public DriveTypeMismatchException(string message)
		{
			Message = message;
		}

		public override string Message { get; }
	}

	#endregion

}