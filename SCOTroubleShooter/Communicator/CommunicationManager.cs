using System;
using System.Globalization;
using System.IO.Ports;
using System.Text;

namespace SCOTroubleShooter
{
	internal class CommunicationManager : IDisposable
	{

		//.............................................................................................................

		#region MessageType enum

		public enum MessageType
		{
			Incoming,
			Outgoing,
			Normal,
			Warning,
			Error
		} ;

		#endregion

		//.............................................................................................................

		#region Private Fields

		private SerialPort ComPort { get; set; }

		#endregion

		//.............................................................................................................

		#region Constructors

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="CommunicationManager"/> class.
		/// </summary>
		/// <param name="portname">The portname.</param>
		//----------------------------------------------------------------------------------------------------
		public CommunicationManager(string portname)
		{
			BaudRate = "9600";
			Parity = "None";
			StopBits = "One";
			DataBits = "8";
			PortName = portname;
			ComPort = new SerialPort();
		}

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="CommunicationManager"/> class.
		/// </summary>
		/// <param name="baud">Desired BaudRate</param>
		/// <param name="par">Desired Parity</param>
		/// <param name="sBits">Desired StopBits</param>
		/// <param name="dBits">Desired DataBits</param>
		/// <param name="name">Desired PortName</param>
		//----------------------------------------------------------------------------------------------------
		public CommunicationManager(string baud, string par, string sBits, string dBits, string name)
		{
			BaudRate = baud;
			Parity = par;
			StopBits = sBits;
			DataBits = dBits;
			PortName = name;
			ComPort = new SerialPort();
		}

		#endregion

		//.............................................................................................................

		#region IDisposable Members

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
		/// resources.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (ComPort == null) return;

			// Dispose of the SerialPort resource.
			if (ComPort.IsOpen)
				ComPort.Close();
			ComPort.Dispose();
			ComPort = null;
		}

		#endregion

		//.............................................................................................................

		#region Public Properties

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether the port is open.
		/// </summary>
		/// <value><c>true</c> if the port is open; otherwise, <c>false</c>.</value>
		//----------------------------------------------------------------------------------------------------
		public bool IsPortOpen => ComPort.IsOpen;

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whetherthe event is added.
		/// </summary>
		/// <value><c>true</c> if event is added; otherwise, <c>false</c>.</value>
		//----------------------------------------------------------------------------------------------------
		public bool EventAdded { get; set; }

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the baud rate.
		/// </summary>
		/// <value>The baud rate.</value>
		//----------------------------------------------------------------------------------------------------
		public string BaudRate { get; set; }

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the parity.
		/// </summary>
		/// <value>The parity.</value>
		//----------------------------------------------------------------------------------------------------
		public string Parity { get; set; }

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the stop bits.
		/// </summary>
		/// <value>The stop bits.</value>
		//----------------------------------------------------------------------------------------------------
		public string StopBits { get; set; }

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the data bits.
		/// </summary>
		/// <value>The data bits.</value>
		//----------------------------------------------------------------------------------------------------
		public string DataBits { get; set; }

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the name of the port.
		/// </summary>
		/// <value>The name of the port.</value>
		//----------------------------------------------------------------------------------------------------
		public string PortName { get; set; }

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the type of the current transmission.
		/// </summary>
		/// <value>The type of the current transmission.</value>
		//----------------------------------------------------------------------------------------------------
		public TransmissionType CurrentTransmissionType { get; set; }

		#endregion

		//.............................................................................................................

		#region WriteData

		public void WriteData(string msg)
		{
			switch (CurrentTransmissionType)
			{
				case TransmissionType.Text:
					// First make sure the port is open.
					if (!ComPort.IsOpen)
						ComPort.Open();
					// Send the message to the port
					ComPort.Write(msg + "\n");
					DisplayData(MessageType.Outgoing, msg + "\n");
					break;
				case TransmissionType.Hex:
					try
					{
						// Convert the message to byte array
						byte[] newMsg = HexToByte(msg);
						// Send the message to the port
						ComPort.Write(newMsg, 0, newMsg.Length);
						// Convert back to hex and display
						DisplayData(MessageType.Outgoing, ByteToHex(newMsg) + "\n");
					}
					catch (FormatException ex)
					{
						DisplayData(MessageType.Error, ex.Message);
					}
					break;
				default:
					// First make sure the port is open.
					if (!ComPort.IsOpen)
						ComPort.Open();
					// Send the message to the port
					ComPort.Write(msg);
					DisplayData(MessageType.Outgoing, msg + "\n");
					break;
			}
		}

		#endregion

		//.............................................................................................................

		#region HexToByte

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// method to convert hex string into a byte array
		/// </summary>
		/// <param name="msg">string to convert</param>
		/// <returns>a byte array</returns>
		//----------------------------------------------------------------------------------------------------
		private static byte[] HexToByte(string msg)
		{
			// Remove any spaces from the string
			msg = msg.Replace(" ", "");
			// Create a byte array the length of the divided by 2 (Hex is 2 characters in length)
			var comBuffer = new byte[msg.Length / 2];
			// Loop through the length of the provided string
			for (var i = 0; i < msg.Length; i += 2)
				// Convert each set of 2 characters to a byte and add to the array
				comBuffer[i / 2] = Convert.ToByte(msg.Substring(i, 2), 16);
			return comBuffer;
		}

		#endregion

		//.............................................................................................................

		#region ByteToHex

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// method to convert a byte array into a hex string
		/// </summary>
		/// <param name="comByte">byte array to convert</param>
		/// <returns>a hex string</returns>
		//----------------------------------------------------------------------------------------------------
		private static string ByteToHex(byte[] comByte)
		{
			// Create a new StringBuilder object
			var builder = new StringBuilder(comByte.Length * 3);
			// Loop through each byte in the array
			foreach (byte data in comByte)
				// Convert the byte to a string and add to the stringbuilder
				builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));
			return builder.ToString().ToUpper(CultureInfo.CurrentCulture);
		}

		#endregion

		//.............................................................................................................

		#region DisplayData

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Method to display the data to & from the port on the screen
		/// </summary>
		/// <param name="type">MessageType of the message</param>
		/// <param name="msg">Message to display</param>
		//----------------------------------------------------------------------------------------------------
		[STAThread]
		private static void DisplayData(MessageType type, string msg)
		{
			if (type == MessageType.Error)
			{
			}
			if (msg == null)
			{
			}
		}

		#endregion

		//.............................................................................................................

		#region TransmissionType enum

		public enum TransmissionType
		{
			Text,
			Hex
		}

		#endregion

		public void ClosePort()
		{
			if (ComPort.IsOpen)
				ComPort.Close();
		}

		//.............................................................................................................

		#region OpenPort

		public void OpenPort()
		{
			try
			{
				// First check if the port is already open; if its open then close it
				if (ComPort.IsOpen)
					ComPort.Close();

				// Set the properties of our SerialPort Object
				ComPort.BaudRate = int.Parse(BaudRate);
				ComPort.DataBits = int.Parse(DataBits);
				ComPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBits);
				ComPort.Parity = (Parity)Enum.Parse(typeof(Parity), Parity);
				ComPort.PortName = PortName;
				ComPort.ReadTimeout = 500;
				ComPort.WriteTimeout = 500;
				// Now open the port
				ComPort.Open();
				// Display message
				DisplayData(MessageType.Normal, "Port opened at " + DateTime.Now + "\n");
			}
			catch (Exception ex)
			{
				DisplayData(MessageType.Error, ex.Message);
				throw;
			}
		}

		#endregion

		//.............................................................................................................

		#region SetParityValues

		public void SetParityValues(object value)
		{
			Console.WriteLine(value);
			//foreach (var str in Enum.GetNames(typeof(Parity)))
			//{
			//    //  ((ComboBox)value).Items.Add(str);
			//}
		}

		#endregion

		//.............................................................................................................

		#region SetStopBitValues

		public void SetStopBitValues(object value)
		{
			Console.WriteLine(value);
			//foreach (var str in Enum.GetNames(typeof(StopBits)))
			//{
			//    //  ((ComboBox)value).Items.Add(str);
			//}
		}

		#endregion

		//.............................................................................................................

		#region SetPortNameValues

		public void SetPortNameValues(object value)
		{
			Console.WriteLine(value);
			//foreach (var str in SerialPort.GetPortNames())
			//{
			//    // ((ComboBox)value).Items.Add(str);
			//}
		}

		#endregion

		//.............................................................................................................

		#region ComPort_DataReceived

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// method that will be called when theres data waiting in the buffer
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		public string ReadData()
		{
			return ComPort.IsOpen ? ComPort.ReadExisting() : null;
		}

		public void AddEventHandler()
		{
			if (EventAdded) return;
			ComPort.DataReceived += ComPort_DataReceived;
			EventAdded = true;
		}

		public void RemoveEventHandler()
		{
			if (!EventAdded) return;
			ComPort.DataReceived -= ComPort_DataReceived;
			EventAdded = false;
		}

		private void ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			// Determine the mode the user selected (binary/string)
			switch (CurrentTransmissionType)
			{
				case TransmissionType.Text:
					// Read data waiting in the buffer
					string msg = ComPort.ReadExisting();
					UpdateData(new CommEventArgs(CommStatus.Incoming, msg));
					break;
				case TransmissionType.Hex:
					// Retrieve number of bytes in the buffer
					int bytes = ComPort.BytesToRead;
					// Create a byte array to hold the awaiting data
					var comBuffer = new byte[bytes];
					// Read the data and store it
					ComPort.Read(comBuffer, 0, bytes);
					DisplayData(MessageType.Incoming, ByteToHex(comBuffer));
					break;
				default:
					// Read data waiting in the buffer
					string str = ComPort.ReadExisting();
					DisplayData(MessageType.Incoming, str);
					break;
			}
		}

		#endregion

		//.............................................................................................................
		
		#region Configuration Status Event Handling

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Represents the method that will handle the ConfigStatusChanged event raised when certain
		/// processes are initiated by the ConfigurationWizard control.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		public delegate void StatusChangedEventHandler(object sender, CommEventArgs e);

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Event that occurs when the configuration status is changed.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		public event StatusChangedEventHandler DataReceive;

		//----------------------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the status resulting in a StatusChanged event to be raised.
		/// </summary>
		//----------------------------------------------------------------------------------------------------
		protected void UpdateData(CommEventArgs e)
		{
			DataReceive?.Invoke(this, e);
		}

		#endregion

	}
}