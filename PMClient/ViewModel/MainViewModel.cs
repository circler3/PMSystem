using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PMClient.Model;
using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using OfficeOpenXml;
using Microsoft.Win32;

namespace PMClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set("WelcomeTitle", ref _welcomeTitle, value);
            }
        }
        /// <summary>
        /// The <see cref="UserDict" /> property's name.
        /// </summary>
        public const string UserDictPropertyName = "UserDict";

        private Dictionary<string, string> _userDict = new Dictionary<string, string>();

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Dictionary<string, string> UserDict
        {
            get
            {
                return _userDict;
            }
            set
            {
                Set("UserDict", ref _userDict, value);
            }
        }

        /// <summary>
        /// The <see cref="Previleged" /> property's name.
        /// </summary>
        public const string PrevilegedPropertyName = "Previleged";

        private bool _previleged = false;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool Previleged
        {
            get
            {
                return _previleged;
            }
            set
            {
                Set("Previleged", ref _previleged, value);
            }
        }

        private RelayCommand _addCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand
                    ?? (_addCommand = new RelayCommand(
                    () =>
                    {
                        WorkItems.Add(new WorkItemViewModel() { Name = "<输入标题>", Description = "<输入描述>" });
                    }));
            }
        }

        private RelayCommand _exportCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand ExportCommand
        {
            get
            {
                return _exportCommand
                    ?? (_exportCommand = new RelayCommand(ExportWorkItemsCommand));
            }
        }

        private void ExportWorkItemsCommand()
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.Filter = "Excel File|*.xlsx";
            diag.ShowDialog();
            if (string.IsNullOrEmpty(diag.FileName))
            {
                return;
            }
            else
            {
                using (var p = new ExcelPackage())
                {
                    var sheet = p.Workbook.Worksheets.Add("任务列表");

                    //Cells的起始索引是1
                    sheet.Cells[1, 1].Value = "任务标题";
                    sheet.Cells[1, 2].Value = "任务描述";
                    sheet.Cells[1, 3].Value = "执行人";
                    sheet.Cells[1, 4].Value = "进度";
                    sheet.Cells[1, 5].Value = "截止时间";
                    int row = 1;
                    foreach (var n in WorkItems)
                    {
                        row += 1;
                        sheet.Cells[row, 1].Value = n.Name;
                        sheet.Cells[row, 2].Value = n.Description;
                        sheet.Cells[row, 3].Value = n.Username;
                        sheet.Cells[row, 4].Value = n.Percentage;
                        sheet.Cells[row, 5].Value = n.Deadline;
                    }

                    p.SaveAs(new System.IO.FileInfo(diag.FileName));
                }

            }
        }

        private RelayCommand<object> _updateCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand<object> UpdateCommand
        {
            get
            {
                return _updateCommand
                    ?? (_updateCommand = new RelayCommand<object>(UpdateWorkItem));
            }
        }

        private void UpdateWorkItem(object o)
        {
            WorkItemViewModel w = (WorkItemViewModel)o;
            var mes = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}\r\n", "Update", w.Guid, w.Name.Replace(" ", "&nbsp;"), w.Description.Replace(" ", "&nbsp;"), w.Percentage, w.Deadline.ToShortDateString(), w.Priority, w.Username ?? "自己");
            Send(mes);
        }

        private RelayCommand<object> _deleteCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand<object> DeleteCommand
        {
            get
            {
                return _deleteCommand
                    ?? (_deleteCommand = new RelayCommand<object>(DeleteWorkItem));
            }
        }

        private void DeleteWorkItem(object parameter)
        {
            WorkItemViewModel w = (WorkItemViewModel)parameter;
            var mes = string.Format("{0} {1} {2} {3} {4} {5} {6}\r\n", "Remove", w.Guid, w.Name.Replace(" ", "&nbsp;"), w.Description.Replace(" ", "&nbsp;"), w.Percentage, w.Deadline.ToShortDateString(), w.Priority);
            Send(mes);
            WorkItems.Remove(w);
        }
        /// <summary>
        /// The <see cref="WorkItems" /> property's name.
        /// </summary>
        public const string WorkItemsPropertyName = "WorkItems";

        private ObservableCollection<WorkItemViewModel> _workItems = new ObservableCollection<WorkItemViewModel>();

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<WorkItemViewModel> WorkItems
        {
            get
            {
                return _workItems;
            }
            set
            {
                Set("WorkItems", ref _workItems, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });
            //_workItems.Add(new WorkItemViewModel() { Name = "工作标题", Percentage = 30, Description = "完成一定精度的任务完成一定精度的任务完成一定精度的任务" });
            //_workItems.Add(new WorkItemViewModel() { Name = "B work item", Percentage = 40 });

            tcpClient = new TcpClient();
            tcpClient.Connect(Properties.Settings.Default.ServerIP, Properties.Settings.Default.ServerPort);
            stream = tcpClient.GetStream();
            Thread th = new Thread(new ThreadStart(ReadData));
            th.IsBackground = true;
            th.Start();
        }

        private TcpClient tcpClient;
        private NetworkStream stream;


        public void Send(string message)
        {
            byte[] data = Encoding.GetEncoding("gb2312").GetBytes(message);
            try
            {
                if (stream.CanWrite)
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception)
            {
                stream.Close();
                tcpClient.Close();
                tcpClient.Connect(Properties.Settings.Default.ServerIP, Properties.Settings.Default.ServerPort);
            }

        }

        private void ReadData()
        {
            while (true)
            {
                if (stream.CanRead)
                {
                    try
                    {
                        byte[] readBuffer = new byte[1024];
                        //stream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback((x) => { }), stream);
                        int count = stream.Read(readBuffer, 0, readBuffer.Length);
                        //App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new ParameterizedThreadStart(ProcessMessage), mes)
                        string message = Encoding.GetEncoding("gb2312").GetString(readBuffer, 0, count);
                        //ProcessMessage(message);
                        App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new ParameterizedThreadStart(ProcessMessage), message);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }

                }
            }

        }

        private void ProcessMessage(object mes)
        {
            var message = (string)mes;
            string[] splitted = message.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var n in splitted)
            {
                ProcessMessage(n);
            }
        }

        private void ProcessMessage(string message)
        {
            //string message = Encoding.GetEncoding("gb2312").GetString(buffer, 0, count);
            string[] mesParts = message.Split(' ');
            if (mesParts[0] == "Update")
            {
                //var mes = string.Format("{0} {1} {2} {3} {4} {5} {6}\r\n", "Remove", w.Guid, w.Name.Replace(" ", "&nbsp;"), w.Description.Replace(" ", "&nbsp;"), w.Percentage, w.Deadline.ToShortDateString(), w.Priority);
                var insp = (from c in WorkItems
                            where c.Guid == mesParts[1]
                            select c).FirstOrDefault();
                if (insp == null)
                {
                    WorkItemViewModel item = new WorkItemViewModel
                    {
                        Guid = mesParts[1],
                        Name = mesParts[2].Replace("&nbsp;", " "),
                        Description = mesParts[3].Replace("&nbsp;", " "),
                        Percentage = Convert.ToInt32(mesParts[4]),
                        Deadline = Convert.ToDateTime(mesParts[5]),
                        Priority = Convert.ToInt32(mesParts[6]),
                        Username = mesParts[7]
                    };
                    //App.Current.MainWindow.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                    //new ParameterizedThreadStart((x) => WorkItems.Add(x as WorkItemViewModel)), item);
                    WorkItems.Add(item);
                }
                else
                {
                    insp.Name = mesParts[2].Replace("&nbsp;", " ");
                    insp.Description = mesParts[3].Replace("&nbsp;", " ");
                    insp.Percentage = Convert.ToInt32(mesParts[4]);
                    insp.Deadline = Convert.ToDateTime(mesParts[5]);
                    insp.Priority = Convert.ToInt32(mesParts[6]);
                    insp.Username = mesParts[7];
                }
            }
            else if (mesParts[0] == ("Token"))
            {
                if (mesParts[1] == "Previleged")
                {
                    Previleged = true;
                }
            }
            else if (mesParts[0] == "Userlist")
            {
                for (int i = 1; i < mesParts.Count() - 2; i += 2)
                {
                    UserDict[mesParts[i]] = mesParts[i + 1];
                }
            }
        }

        public override void Cleanup()
        {
            // Clean up if needed
            if (stream != null)
            {
                stream.Close();
            }
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
            base.Cleanup();
        }

        public void Dispose()
        {
            Cleanup();
        }
    }
}