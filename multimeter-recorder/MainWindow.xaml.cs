using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Timers;
using System.Globalization;
using System.Threading;

namespace multimeter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>

	public class Entity
	{
		public ObjectId Id { get; set; }
		public DateTime Date { get; set; }
		public Object[] Values = new Object[4];
	}
	public class Port
	{
		public Port(SerialPort name, string description, int index)
		{
			this.name = name;
			this.description = description;
			this.index = index;
			this.response = "Error";
		}
		public SerialPort name {get; set;}
		public string description {get; set;}
		public int index {get; set;}
		public string response {get; set;}
	}
	public partial class MainWindow : Window
	{
		private const string ConnectionString = "mongodb://elrond:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false";
		private List<Port> ports = new List<Port>();
		private static System.Timers.Timer DurationTimer;
		private int DurationTime = 0;
		private int SampleRateTime = 0;
		public MainWindow()
		{
			InitializeComponent();
			RefreshCollections();

			ComPort1.DataName = "Multimeter1";
			ComPort2.DataName = "Multimeter2";
			ComPort3.DataName = "Multimeter3";
			ComPort4.DataName = "Multimeter4";

			DurationType.Items.Add("Days");
			DurationType.Items.Add("Hours");
			DurationType.Items.Add("Mins");
			DurationType.SelectedItem = "Hours";

			SampleRateType.Items.Add("Mins");
			SampleRateType.Items.Add("Secs");
			SampleRateType.SelectedItem = "Secs";
		}
		private void NumberValidationTextBox(object sender = null, TextCompositionEventArgs e = null)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}
		private async void AddNewDataSet(object sender, RoutedEventArgs e)
		{
			var client = new MongoClient(ConnectionString);
			var database = client.GetDatabase("MultimeterOutput");
			if (!DataSetsComboBox.Items.Contains(DataNameInput.Text))
			{
				await database.CreateCollectionAsync(DataNameInput.Text, new CreateCollectionOptions
				{
					Capped = false,
				});
				DataSetsComboBox.Items.Add(DataNameInput.Text);
			}
			else
			{
				MessageBox.Show("Data Set already exists! Selecting this will add data to the Data Set!", "Warning");
			}
			DataSetsComboBox.SelectedItem = DataNameInput.Text;
			DataNameInput.Text = "";
		}
		private void RefreshCollections(object sender = null, RoutedEventArgs e = null)
		{
			var client = new MongoClient(ConnectionString);
			var database = client.GetDatabase("MultimeterOutput");
			DataSetsComboBox.Items.Clear();
			foreach (var item in database.ListCollectionsAsync().Result.ToListAsync<BsonDocument>().Result)
			{
				DataSetsComboBox.Items.Add(item["name"].ToString());
			}
		}

		private void ViewDataButton(object sender, RoutedEventArgs e)
		{
			var psi = new ProcessStartInfo
			{
				FileName = "http://multimer:81/",
				UseShellExecute = true
			};
			Process.Start(psi);
		}

		private void StopButton_Click(object sender, RoutedEventArgs e)
		{
			DurationEnd();
		}
		private async void DurationEnd(Object source = null, ElapsedEventArgs e = null)
		{
			foreach (Port port in ports)
			{
				
				port.name.Dispose();
				await Task.Delay(500);
				port.name.Close();
			}
			ports = new List<Port>();
			this.Dispatcher.Invoke(() => 
			{
				DurationTimer.Enabled = false;
				DataSetsComboBox.IsEnabled = !DataSetsComboBox.IsEnabled;
				DurationValue.IsEnabled = !DurationValue.IsEnabled;
				SampleRateValue.IsEnabled = !SampleRateValue.IsEnabled;
				StartButton.IsEnabled = !StartButton.IsEnabled;
				DurationValue.IsReadOnly = !DurationValue.IsReadOnly;
				DurationType.IsEnabled = !DurationType.IsEnabled;
				SampleRateType.IsEnabled = !SampleRateType.IsEnabled;
				SampleRateValue.IsReadOnly = !SampleRateValue.IsReadOnly;
				ComPort1.IsEnabled = true;
				ComPort2.IsEnabled = true;
				ComPort3.IsEnabled = true;
				ComPort4.IsEnabled = true;
				StartButton.Content = "Start";
			});
		}
		void SerialPort_DataReceived(object s, SerialDataReceivedEventArgs e)
		{
			//response += port.ReadExisting();
			try
			{
				SerialPort p = (SerialPort)s;
				foreach(Port port in ports)
				{
					if(p.PortName == port.name.PortName)
					{
						port.response += p.ReadExisting();
					}
				}
			}
			catch
			{

			}
			//Trace.WriteLine();
		}
		private void StartRecording(object sender = null, RoutedEventArgs e = null)
		{
			bool allPortsOpen = true;
			if (DurationValue.Text != "" && SampleRateValue.Text != "" && DataSetsComboBox.Text != "")
			{
				SerialPort p;
				if(ComPort1.ComPort != "" && ComPort1.DataName != "")
				{
					p = new SerialPort(ComPort1.ComPort, 9600, Parity.None, 8, StopBits.One);
					p.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
					ports.Add(new Port(
						p,
						ComPort1.DataName,
						0
					));
				}
				if (ComPort2.ComPort != "" && ComPort2.DataName != "")
				{
					p = new SerialPort(ComPort2.ComPort, 9600, Parity.None, 8, StopBits.One);
					p.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
					ports.Add(new Port(
						p,
						ComPort2.DataName,
						1
					));
				}
				if (ComPort3.ComPort != "" && ComPort3.DataName != "")
				{
					p = new SerialPort(ComPort3.ComPort, 9600, Parity.None, 8, StopBits.One);
					p.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
					ports.Add(new Port(
						p,
						ComPort3.DataName,
						2
					));
				}
				if (ComPort4.ComPort != "" && ComPort4.DataName != "")
				{
					p = new SerialPort(ComPort4.ComPort, 9600, Parity.None, 8, StopBits.One);
					p.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
					ports.Add(new Port(
						p,
						ComPort4.DataName,
						3
					));
				}

				foreach(Port port in ports)
				{
					try
					{
						port.name.Open();
					}
					catch
					{

					}
				}
				if(ports.Count != 0)
				{
					if (allPortsOpen)
					{
						DurationTime = Int32.Parse(DurationValue.Text) * 1000;
						SampleRateTime = Int32.Parse(SampleRateValue.Text) * 1000;
						if (DurationType.Text == "Days")
						{
							DurationTime *= 60 * 60 * 24;
						}
						else if (DurationType.Text == "Hours")
						{
							DurationTime *= 60 * 60;
						}
						else if (DurationType.Text == "Mins")
						{
							DurationTime *= 60;
						}

						if (SampleRateType.Text == "Mins")
						{
							SampleRateTime *= 60;
						}
						DurationTimer = new System.Timers.Timer(DurationTime);
						DurationTimer.Elapsed += DurationEnd;
						DurationTimer.AutoReset = false;
						DurationTimer.Enabled = true;
						ToggleStartButton();
						SampleRateTimer();
					}
				}
				else
				{
					MessageBox.Show("Check Com Ports!", "Warning");
				}
			}
			else
			{
				MessageBox.Show("Check File Name, Duration, and Sample Rate!", "Warning");
			}
		}
		private void ToggleStartButton()
		{
			DataSetsComboBox.IsEnabled = !DataSetsComboBox.IsEnabled;
			DurationValue.IsEnabled = !DurationValue.IsEnabled;
			SampleRateValue.IsEnabled = !SampleRateValue.IsEnabled;
			StartButton.IsEnabled = !StartButton.IsEnabled;
			DurationValue.IsReadOnly = !DurationValue.IsReadOnly;
			DurationType.IsEnabled = !DurationType.IsEnabled;
			SampleRateType.IsEnabled = !SampleRateType.IsEnabled;
			SampleRateValue.IsReadOnly = !SampleRateValue.IsReadOnly;
			ComPort1.IsEnabled = false;
			ComPort2.IsEnabled = false;
			ComPort3.IsEnabled = false;
			ComPort4.IsEnabled = false;
			StartButton.Content = "In Progress...";
		}
		private async void SampleRateTimer()
		{
			if (DurationTimer.Enabled == true)
			{
				foreach (Port p in ports)
				{
					try
					{
						p.response = "";
						p.name.Write(":FETC?\r\n");
					}
					catch
					{
					}
				}
				Thread thread = new Thread(getMultimeterData);
				thread.Start();
				await Task.Delay(SampleRateTime);
				SampleRateTimer();
			}
			else
			{
			}
		}
		private void getMultimeterData()
		{
			Thread.Sleep(TimeSpan.FromSeconds(0.5));
			Entity e = new Entity();
			this.Dispatcher.BeginInvoke(new Action(() =>
			{
				e.Date = DateTime.Now;
				double valDbl;
				string valString;
				foreach(Port port in ports)
				{
					valString = port.response;
					try
					{
						valDbl = double.Parse(valString.Replace(":FETC?", "").Replace("\n", "").Replace("e", "E"), CultureInfo.InvariantCulture);
						e.Values[port.index] = new { Name = port.description, Value = valDbl };
					}
					catch
					{
						
					}
				}
				addToMongo(e);
			}));
		}
		async void addToMongo(Entity e)
		{
			var client = new MongoClient(ConnectionString);
			var database = client.GetDatabase("MultimeterOutput");
			string colName = DataSetsComboBox.Text;
			IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(colName);
			await collection.InsertOneAsync(e.ToBsonDocument());
		}
	}
}

