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

        private bool _isOpening;

        public bool IsOpening
        {
            get { return _isOpening; }
            set { _isOpening = value; OnPropertyChanged(); }
        }

        private bool _usingEnterChb;

        public bool UsingEnterChb
        {
            get { return _usingEnterChb; }
            set { _usingEnterChb = value; OnPropertyChanged(); }
        }


        private bool _isWrite;

        public bool IsWrite
        {
            get { return _isWrite; }
            set { _isWrite = value; OnPropertyChanged(); }
        }

        private string _screenReceiveText;

        public string ScreenReceiveText
        {
            get { return _screenReceiveText; }
            set { _screenReceiveText = value; OnPropertyChanged(); }
        }

        private bool _alwaysUpdate;

        public bool AlwaysUpdateChb
        {
            get { return _alwaysUpdate; }
            set { _alwaysUpdate = value; OnPropertyChanged(); }
        }

        private bool _addToOldDataChb;

        public bool AddToOldDataChb
        {
            get { return _addToOldDataChb; }
            set { _addToOldDataChb = value; OnPropertyChanged(); }
        }


        public ICommand OpenSerialPortCommand { get; set; }
        public ICommand CloseSerialPortCommand { get; set; }
        public ICommand SendDataCommand { get; set; }
        public ICommand ClearScreenCommand { get; set; }
        public ICommand ClearScreenReceiveCommand { get; set; }

        public MainViewModel()
        {
            LoadSerialPort();
            BaudRateSource = new ObservableCollection<int>() { 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200, 128000, 256000 };
            DataBitsSource = new ObservableCollection<int>() { 5, 6, 7, 8 };
            StopBitsSource = new ObservableCollection<string>() { "None", "One", "Two", "OnePointFive" };
            ParityBitsSource = new ObservableCollection<string>() { "None", "Odd", "Even", "Mark", "Space" };

            BaudRateItem = 9600;
            DataBitsItem = 8;
            StopBitsItem = "One";
            ParityBitsItem = "None";
            UsingEnterChb = true;
            
            OpenSerialPortCommand = new RelayCommand(() =>
            {
                OpenPort();
                serialPort.DataReceived += SerialPort_DataReceived;
            });
            CloseSerialPortCommand = new RelayCommand(() =>
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.Close();
                    IsOpening = false;
                }
            });
            SendDataCommand = new RelayCommand(() => { SendData(); ScreenText = ""; });

            ClearScreenCommand = new RelayCommand(() =>
            {
                if (ScreenText != string.Empty && ScreenText != "") ScreenText = "";
            });
            ClearScreenReceiveCommand = new RelayCommand(() =>
            {
                ScreenReceiveText = "";
            });

            


        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (AlwaysUpdateChb)
            {
                ScreenReceiveText = serialPort.ReadExisting();
            }
            else if(AddToOldDataChb)
            {
                ScreenReceiveText += serialPort.ReadExisting();
            }
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
                IsOpening = true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                IsOpening = false;
            }
        }

        public void SendData()
        {
            if (serialPort.IsOpen)
            {
                if (IsWrite)
                {
                    serialPort.Write(ScreenText);
                }
                else
                {
                    serialPort.WriteLine(ScreenText);
                }
            }
        }


        public void TextBox_KeyDown(object sender, KeyEventArgs key)
        {
            if (UsingEnterChb)
            {
                if(key.Key == Key.Enter)
                {
                    if (serialPort.IsOpen)
                    {
                        if (IsWrite)
                        {
                           
                            //serialPort.Write(ScreenText);
                            var b =  serialPort.Encoding.GetBytes(ScreenText);
                            serialPort.Write(b, 0, b.Length);
                        }
                        else
                        {
                            serialPort.WriteLine(ScreenText);
                        }

                        //var temp = ScreenText.Replace(Environment.NewLine, "");
                        //ScreenText = "";
                        //ScreenText = temp;
                        ScreenText = "";
                    }
                }
            }
        }
        #endregion

    }


}
