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
namespace BankDeadLock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public class process
        {
            public process(int a1,int a2,int a3,int a4,int c1,int c2,int c3,int c4,int n1,int n2,int n3,int n4,int pname=0)
            {
                aa = a1;
                ab = a2;
                ac = a3;
                ad = a4;

                ca = c1;
                cb = c2;
                cc = c3;
                cd = c4;

                na = n1;
                nb = n2;
                nc = n3;
                nd = n4;

                _name = pname;
            }

            public int aa, ab, ac, ad;
            public int ca, cb, cc, cd;
            public int na, nb, nc, nd;
            public string Allocation
            {
                get { return aa.ToString() + " " + ab.ToString() + " " + ac.ToString() + " " + ad.ToString(); }
            }
            public string Claim
            {
                get { return ca.ToString() + " " + cb.ToString() + " " + cc.ToString() + " " + cd.ToString(); }
            }
            public string Need
            {
                get { return na.ToString() + " " + nb.ToString() + " " + nc.ToString() + " " + nd.ToString(); }
            }

            public int _name;
            public string name
            {
                get { return "P" + _name.ToString(); }
            }
            string _free;
            public string Free
            {
                get { return _free;  }
                set { _free = value; }
            }
            string _FreeAllocation;
            public string FreeAllocation
            {
                get { return _FreeAllocation; }
                set { _FreeAllocation = value; }
            }

             bool _finish;
            public bool finish
            {
                get { return _finish; }
                set { _finish = value; }
            }
        }
        public class wrap
        {
            public ObservableCollection<process> record = new ObservableCollection<process>();
            public int ava, avb, avc, avd;// the avaiable resource;
            public int requestIndex;
            public int ra, rb, rc, rd;
        }
        wrap record = new wrap();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            record.record.Add(new process(3, 0, 1, 1, 4, 1, 1, 1, 1, 1, 0, 0, 1));
            record.record.Add(new process(0, 1, 0, 0, 0, 2, 1, 2, 0, 1, 1, 2, 2));
            record.record.Add(new process(1, 1, 1, 0, 4, 2, 1, 0, 3, 1, 0, 0, 3));
            record.record.Add(new process(1, 1, 0, 1, 1, 1, 2, 1, 0, 0, 2, 0, 4));
            record.record.Add(new process(0, 0, 0, 0, 2, 1, 1, 0, 2, 1, 1, 0, 5));

            //record.record.Add(new process(0, 0, 3, 2, 0, 0, 4, 4, 0, 0, 1, 2, 0));
            //record.record.Add(new process(1, 0, 0, 0, 2, 7, 5, 0, 1, 7, 5, 0, 1));
            //record.record.Add(new process(1, 3, 5, 4, 3, 6, 10, 10, 2, 3, 5, 6, 2));
            //record.record.Add(new process(0, 3, 3, 2, 0, 9, 8, 4, 0, 6, 5, 2, 3));
            //record.record.Add(new process(0, 0, 1, 4, 0, 6, 6, 10, 0, 6, 5, 6, 4));



            dataGrid.ItemsSource = record.record;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            record.requestIndex =  dataGrid.SelectedIndex;
            var f = textBox.Text.Split(' ');
            record.ava = int.Parse(f[0]);
            record.avb = int.Parse(f[1]);
            record.avc = int.Parse(f[2]);
            record.avd = int.Parse(f[3]);

            var g = textBox1.Text.Split(' ');
            record.ra = int.Parse(g[0]);
            record.rb = int.Parse(g[1]);
            record.rc = int.Parse(g[2]);
            record.rd = int.Parse(g[3]);

            check w2 = new check(ref record);
            w2.Show();
        }
    }
}
