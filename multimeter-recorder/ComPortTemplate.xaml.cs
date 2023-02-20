using System;
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
using System.IO.Ports;

namespace multimeter_recorder
{
	/// <summary>
	/// Interaction logic for ComPortTemplate.xaml
	/// </summary>
	public partial class ComPortTemplate : UserControl
	{
		public ComPortTemplate()
		{
			InitializeComponent();
      RefreshPorts();
    }
    public string ComPort
    {
      get { return ComPortComboBox.Text; }
      set { ComPortComboBox.SelectedItem = value; }
    }
    public string DataName
    {
      get { return DataNameTextBox.Text; }
      set { DataNameTextBox.Text = value; }
    }
    public bool isEnabled
    {
      set { 
        ComPortComboBox.IsEnabled = value; 
        RefreshBtn.IsEnabled = value;
        DataNameTextBox.IsEnabled = value;
      }
    }
    private void RefreshPorts(object sender = null, RoutedEventArgs e = null)
    {
      ComPortComboBox.Items.Clear();
      string[] ports = SerialPort.GetPortNames();
      foreach (string port in ports)
      {
        ComPortComboBox.Items.Add(port);
      }
    }
  }
}
