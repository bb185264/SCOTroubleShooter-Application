using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace SCOTroubleShooter
{
	public class ComSrvCommunication : IDisposable
	{

		private bool _bWatchdogTimeoutFlag;
		private bool _bConnected;

		public string RemoteIPAddress { get; set; }
		public string PartNumber { get; set; }
		public string FirmwareVersion { get; set; }
		public string BootRevision { get; set; }
		public bool IsAllowDownload { get; set; }
		public string OS_Filename_Token_1 { get; set; }
		public string OS_Filename_Token_2 { get; set; }
		public bool WatchDogTimeoutFlag => _bWatchdogTimeoutFlag;

		public delegate void StatusChangedEventHandler(object sender, ComEventArgs e);

		public event StatusChangedEventHandler StatusChanged;

		

		public bool IsConnected
		{
			get
			{
				
					return false;
				try
				{
					if (_bWatchdogTimeoutFlag) return false;
					return _bConnected;// _scoControl.TestConnect();
				}
				catch (Exception)
				{
					return false;
				}
			}
		}

		public bool TestConnection()
		{
			try
			{
				if (_bConnected == true)
				{
					_bConnected = false;
					return false;
				}
				
				return _bConnected;
			}
			catch
			{
				_bConnected = false;
				return false;
			}
		}

		public ComSrvCommunication(string remoteIPAddress)
		{
			RemoteIPAddress = remoteIPAddress;
			PartNumber = string.Empty;
			FirmwareVersion = string.Empty;
			BootRevision = string.Empty;
			IsAllowDownload = false;
			OS_Filename_Token_1 = string.Empty;
			OS_Filename_Token_2 = string.Empty;
			Initialize();

			// Subcribe to COM events
			Subscribe();
		}

		~ComSrvCommunication()
		{
			
			_bWatchdogTimeoutFlag = false;
			_bConnected = false;
			
		}
		//..................................................................................................................................

		#region Public Methods

		public /* async */ void Connect()
		{
			try
			{
				
				if (_scoControl.TestConnect())
				{
					OnStatusChange(new ComEventArgs(ComEventArgs.ComStatus.Connected));
				}
			}
			catch (Exception)
			{
				_bConnected = false;
				_scoControl.Disconnect();
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Sends the specified command line.
		/// </summary>
		/// <param name="commandLine">The command line.</param>
		/// <param name="useEnding">if set to <c>true</c> [the default] then use ending.</param>
		//------------------------------------------------------------------------------------------------------------------------
		public void Send(string commandLine, bool useEnding = true)
		{
			if (!IsConnected)
				return;
			if (useEnding)
				commandLine += "\r";
			try
			{
				_Terminal.Write(commandLine);
			}
			catch (Exception Ex)
			{
				Console.WriteLine("Exception in Send()" + Ex.Message);
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Read data from terminal.
		/// Use only for Download logic purposes.
		/// For other reasons use <see cref="ComEventArgs.ComStatus.Data"/> event.
		/// </summary>
		//------------------------------------------------------------------------------------------------------------------------
		public string Read()
		{
			return !IsConnected ? "" : _Terminal.Read();
		}

		
		
		private void OnWatchdogTimeout()
		{
			_bWatchdogTimeoutFlag = true;
			OnStatusChange(new ComEventArgs(ComEventArgs.ComStatus.WatchdogTimeout));
		}

		private void OnStatusChange(ComEventArgs e)
		{
			if (e.Status == ComEventArgs.ComStatus.Connected) _bConnected = true;
			if (e.Status == ComEventArgs.ComStatus.Disconnected) _bConnected = false;
			StatusChanged?.Invoke(this, e);
		}

		private void OnControllerInfoUpdate()
		{
			OnStatusChange(new ComEventArgs(ComEventArgs.ComStatus.ControllerInfo));
		}

		public void Dispose()
		{
			Unsubscribe();
			try
			{
			
			}
			catch (COMException)
			{
				Console.WriteLine("ComServer is not running!");
			}
			catch (Exception)
			{
				Console.WriteLine("ComServer is not running!");
			}
		}

		private void Subscribe()
		{
			
		}

		private void Unsubscribe()
		{
			
		}

		#endregion

		public void DownloadComplete()
		{
			OnStatusChange(new ComEventArgs(ComEventArgs.ComStatus.DownloadComplete));
		}
	}
}
