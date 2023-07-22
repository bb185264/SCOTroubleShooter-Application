using System;
using System.Collections.Generic;

namespace SCOTroubleShooter
{
	//----------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Class used for creating and maintaining connections between the  Server and the PMM application.
	/// </summary>
	//----------------------------------------------------------------------------------------------------------------------------
	public class ControllerConnectionManager
	{
		private readonly Dictionary<string, ComSrvCommunication> _connectionPool = new Dictionary<string, ComSrvCommunication>();

		private static ControllerConnectionManager _instance;

		private const string DEFAULT_IP = "192.168.100.1";
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		//------------------------------------------------------------------------------------------------------------------------
		public static ControllerConnectionManager Instance => _instance ?? (_instance = new ControllerConnectionManager());

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether this instance contains one or more connections currently connected to a device.
		/// </summary>
		/// <value><c>true</c> if this instance contains one or more connections; otherwise, <c>false</c>.</value>
		//------------------------------------------------------------------------------------------------------------------------
		public bool IsConnections
		{
			get
			{
				foreach (KeyValuePair<string, ComSrvCommunication> connection in _connectionPool)
				{
					if (connection.Value.IsConnected)
						return true;
				}
				return false;
			}
		}

		public bool CheckMultipleConnections_ConnectedDefaultIP
		{
			get
			{
				int count = 0;
				bool IsControllerConnectedDefaultIP = false;
				bool status = false;
				foreach (KeyValuePair<string, ComSrvCommunication> connection in _connectionPool)
				{
					if (connection.Value.IsConnected)
					{
						count++;
						if (connection.Value.RemoteIPAddress == DEFAULT_IP)
						{
							IsControllerConnectedDefaultIP = true;
						}
					}
					
					if (count > 1 && IsControllerConnectedDefaultIP == true)
                    {
						status = true;
						break;
                    }
				}
				return status;
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Prevents a default instance of the <see cref="ControllerConnectionManager"/> class from being created.
		/// </summary>
		//------------------------------------------------------------------------------------------------------------------------
		private ControllerConnectionManager()
		{
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a connection to the  server and adds it to the connection pool.
		/// </summary>
		/// <param name="uid">The uid.</param>
		/// <param name="remoteIpAddress">The remote ip address.</param>
		/// <returns></returns>
		//------------------------------------------------------------------------------------------------------------------------
		public ComSrvCommunication CreateConnection(string uid, string remoteIpAddress)
		{
			try
			{
				var comm = new ComSrvCommunication(remoteIpAddress);
				_connectionPool.Add(uid, comm);
			}
			catch (ArgumentException)
			{
			}
			return _connectionPool[uid];
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets an existing connection to the  server from the connection pool corresponding to the specified identifier. If 
		/// the connection doesn't exist in the pool then a new one is created and added to the pool.
		/// </summary>
		/// <param name="uid">The uid.</param>
		/// <returns></returns>
		//------------------------------------------------------------------------------------------------------------------------
		public ComSrvCommunication GetConnectionByID(string uid)
		{
			ComSrvCommunication comm;
			_connectionPool.TryGetValue(uid, out comm);
			return comm;
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Removes an existing connection (It is disconnected and diposed).
		/// </summary>
		/// <param name="uid">The uid.</param>
		//------------------------------------------------------------------------------------------------------------------------
		public void RemoveConnection(string uid)
		{
			ComSrvCommunication connection;
			_connectionPool.TryGetValue(uid, out connection);
			if (connection == null)
				return;
			connection.Disconnect();
			connection.Dispose();
			_connectionPool.Remove(uid);
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Resets this instance by removing any and all connections to the  Server, disposing of the connection object, and 
		/// clearing the connection pool.
		/// </summary>
		//------------------------------------------------------------------------------------------------------------------------
		public void Clean()
		{
			foreach (KeyValuePair<string, ComSrvCommunication> connection in _connectionPool)
			{
				connection.Value.Disconnect();
				connection.Value.Dispose();
			}
			_connectionPool.Clear();
		}
	}
}
