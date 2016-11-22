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
using System.Collections.ObjectModel;
using System.Timers;
using System.Threading;
using System.Windows.Threading;
using System.Reflection;

namespace dispatch
{
    class Task
    {
        string _state;
        string _name;
        int _totalLong;
        int _doneLong;
        int _priority;
        public int _responseRatio;
        public int waitTime;

        public Task(string state, string name, int totalLong,int priority = 1)
        {
            _state = state;
            _name = name;
            _totalLong = totalLong;
            _doneLong = 0;

            waitTime = 0;
            _priority = priority;
            _responseRatio = 0;
        }

        public int progress
        {
            get { return _doneLong * 200 / _totalLong; }
        }

        public int priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public string state
        {
            get { return _state; }
            set { _state = value; }
        }
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string totalLong
        {
            get { return _totalLong.ToString(); }
            set { _totalLong = int.Parse(value); }
        }
        public string compeleteRate
        {
            get { return _doneLong.ToString() + "/" + _totalLong.ToString(); }
        }
        public bool run()
        {
            if (_doneLong < _totalLong)
            {
                _doneLong++;
                state = "CPU执行";
                return true;
            }else
            {
                state = "完成";
                return false;
            }
        }
        public void stop()
        {
            if(_doneLong < _totalLong)
                state = "就绪";
            else
                state = "完成";
        }
        public string responseRatio
        {
            get { return _responseRatio.ToString()+"%"; }
        }

        public void calResRatio()
        {
            _responseRatio = 100 + waitTime*100 / (_totalLong );
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Task> taskList = new ObservableCollection<Task>();
        public MainWindow()
        {
            InitializeComponent();
        }
        DispatcherTimer timer = new DispatcherTimer();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            taskList.Add(new Task("就绪", "1321", 10));
            dataGrid.ItemsSource = taskList;
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += responseScheduling;
            timer.Tick += timer_Tick;
            timer.Start();

        }

        void roundRobin(object sender, EventArgs e)
        {
            lock(taskList)
            {
                int index = 0;
                int bias = 0;
                foreach (var i in taskList)
                {
                    if (i.state == "CPU执行")
                    {
                        index = taskList.IndexOf(i);
                        i.stop();
                        bias = 1;
                        break;
                    }
                }
                for(int i = bias; i<taskList.Count;i++)
                {
                    if(taskList[(index+i)% taskList.Count].state =="就绪")
                    {
                        taskList[(index + i) % taskList.Count].run();
                        break;
                    } 
                }
                
            }
        }
        void priorityScheduling(object sender, EventArgs e)
        {
            lock(taskList)
            {
                foreach (var i in taskList)
                {
                    if (i.state == "CPU执行")
                    {
                        if (i.run())
                            return;
                        i.stop();
                    }
                }
                int p = 0;
                int index = 0;
                foreach (var i in taskList)
                {
                    if (i.priority > p&&i.state == "就绪")
                    {
                        p = i.priority;
                        index = taskList.IndexOf(i);
                    }
                }
                taskList[index].run();
            }
        }
        void responseScheduling(object sender, EventArgs e)
        {
            lock(taskList)
            {
                foreach (var i in taskList)
                {
                    i.calResRatio();
                }

                foreach (var i in taskList)
                {
                    if(i.state=="CPU执行")
                    {
                        if (i.run())
                        {
                            foreach (var j in taskList)
                            {
                                if (j.state == "就绪")
                                {
                                    j.waitTime++;
                                }
                            }
                            return;
                        }

                        else
                            i.stop();
                    }
                }

                int r = 0;
                int index = 0;


                foreach (var i in taskList)
                {
                    if(r<i._responseRatio&& i.state=="就绪")
                    {
                        r = i._responseRatio;
                        index = taskList.IndexOf(i);
                    }
                }
                taskList[index].run();
                foreach (var i in taskList)
                {
                    if (i.state == "就绪")
                    {
                        i.waitTime++;
                    }
                }


            }
        }

        void timer_Tick(object sender,EventArgs e)
        {
            lock(taskList)
            {
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = taskList;
            }
        }

        private void btn_NewTask_Click(object sender, RoutedEventArgs e)
        {
            lock (taskList)
            {
                Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
                taskList.Add(new Task("就绪", "1321", ra.Next(50, 100), ra.Next(1,10)));
            }
        }
        private void RemoveHandlers(DispatcherTimer dispatchTimer)
        {
            var eventField = dispatchTimer.GetType().GetField("Tick",
                    BindingFlags.NonPublic | BindingFlags.Instance);
            var eventDelegate = (Delegate)eventField.GetValue(dispatchTimer);
            var invocatationList = eventDelegate.GetInvocationList();

            foreach (var handler in invocatationList)
                dispatchTimer.Tick -= ((EventHandler)handler);
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            RemoveHandlers(timer);
            timer.Tick += responseScheduling;
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            RemoveHandlers(timer);
            timer.Tick += priorityScheduling;
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            RemoveHandlers(timer);
            timer.Tick += roundRobin;
            timer.Tick += timer_Tick;
            timer.Start();
        }
    }



}
