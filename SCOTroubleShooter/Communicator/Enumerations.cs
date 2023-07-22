namespace SCOTroubleShooter
{
	//.............................................................................................................

	#region Public Enumerations

	//--------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Specifies the current status used in ConfigEventArgs raised during the configuration process.
	/// </summary>
	//--------------------------------------------------------------------------------------------------------
	public enum CommStatus
	{
		Connecting,
		Connected,
		Disconnecting,
		Disconnected,
		Working,
		Warning,
		CommError,
		Exception,
		
	}

	#endregion

}