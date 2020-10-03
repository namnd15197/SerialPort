using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SerialPort
{
    public class MainViewModel : BaseViewModel
    {

        private ObservableCollection<string> _serialPortSource;

        public ObservableCollection<string> SerialPortSource
        {
            get { return _serialPortSource; }
            set { _serialPortSource = value; OnPropertyChanged(); }
        }

        private ObservableCollection<int> _baudrateSource;

        public ObservableCollection<int> BaudRateSource
        {
            get { return _baudrateSource; }
            set { _baudrateSource = value; OnPropertyChanged(); }
        }

        private ObservableCollection<int> _databitsSource;

        public ObservableCollection<int> DataBitsSource
        {
            get { return _databitsSource; }
            set { _databitsSource = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _stopbitsSource;

        public ObservableCollection<string> StopBitsSource
        {
            get { return _stopbitsSource; }
            set { _stopbitsSource = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _parityBitsSource;

        public ObservableCollection<string> ParityBitsSource
        {
            get { return _parityBitsSource; }
            set { _parityBitsSource = value; OnPropertyChanged(); }
        }

        private string _serialPortItem;

        public string SerialPortItem
        {
            get { return _serialPortItem; }
            set { _serialPortItem = value; OnPropertyChanged(); }
        }

        private int _baudrateItem;

        public int BaudRateItem
        {
            get { return _baudrateItem; }
            set { _baudrateItem = value; OnPropertyChanged(); }
        }

        private int _databitsItem;

        public int DataBitsItem
        {
            get { return _databitsItem; }
            set { _databitsItem = value; OnPropertyChanged(); }
        }

        private string _stopbitsItem;

        public string StopBitsItem
        {
            get { return _stopbitsItem; }
            set { _stopbitsItem = value; OnPropertyChanged(); }
        }

        private string _parityBitsItem;

        public string ParityBitsItem
        {
            get { return _parityBitsItem; }
            set { _parityBitsItem = value; OnPropertyChanged(); }
        }

        private System.IO.Ports.SerialPort serialPort;

        private string _screenText;

        public string ScreenText
        {
            get { return _screenText; }
            set { _screenText = value; OnPropertyChanged(); }
        }


        private bool _dtrBoolean;

        public bool DTRBoolean
        {
            get { return _dtrBoolean; }
            set { _dtrBoolean = value; OnPropertyChanged(); }
        }

        private bool _rstBoolean;

        public bool RSTBoolean
        {
            get { return _rstBoolean; }
            set { _rstBoolean = value; OnPropertyChanged(); }
        }


        public ICommand OpenSerialPortCommand { get; set; }
        public ICommand CloseSerialPortCommand { get; set; }
        public ICommand SendDataCommand { get; set; }


        bool _continue;
        System.IO.Ports.SerialPort _serialPort;

        public MainViewModel()
        {
            LoadSerialPort();
            BaudRateSource = new ObservableCollection<int>() { 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200, 128000, 256000 };
            DataBitsSource = new ObservableCollection<int>() { 5, 6, 7, 8 };
            StopBitsSource = new ObservableCollection<string>() { "None", "One", "Two", "OnePointFive" };
            ParityBitsSource = new ObservableCollection<string>() { "None", "Odd", "Even", "Mark", "Space" };

            OpenSerialPortCommand = new RelayCommand(() =>
            {
                OpenPort();
            });
            CloseSerialPortCommand = new RelayCommand(() =>
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.Close();
                }
            });
            SendDataCommand = new RelayCommand(() => { SendData(); });




        }

        #region public methods
        public void LoadSerialPort()
        {
            SerialPortSource = new ObservableCollection<string>();
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            foreach (var item in ports)
            {
                SerialPortSource.Add(item);
            }
        }

        public void OpenPort()
        {
            try
            {
                serialPort = new System.IO.Ports.SerialPort();
                serialPort.PortName = SerialPortItem;
                serialPort.BaudRate = BaudRateItem;
                serialPort.DataBits = DataBitsItem;
                serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBitsItem);
                serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), ParityBitsItem);
                serialPort.Open();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        public void SendData()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Write(ScreenText);
            }
        }
        #endregion

    }
        

}
