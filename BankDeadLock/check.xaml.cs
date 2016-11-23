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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
namespace BankDeadLock
{
    /// <summary>
    /// Interaction logic for check.xaml
    /// </summary>
    public partial class check : Window
    {
        void bank(ref MainWindow.wrap record)
        {
            MainWindow.wrap RecordCopy = record;

            foreach (var i in record.record)
            {
                i.finish = false;
            }
            int fa = record.ava, fb = record.avb, fc = record.avc, fd = record.avd;
            fa -= record.ra;
            fb -= record.rb;
            fc -= record.rc;
            fd -= record.rd;

            record.record[record.requestIndex].aa += record.ra;
            record.record[record.requestIndex].ab += record.rb;
            record.record[record.requestIndex].ac += record.rc;
            record.record[record.requestIndex].ad += record.rd;

            record.record[record.requestIndex].na -= record.ra;
            record.record[record.requestIndex].nb -= record.rb;
            record.record[record.requestIndex].nc -= record.rc;
            record.record[record.requestIndex].nd -= record.rd;


            bool ok = true;
            do
            {
                ok = false;
                for(int i = 0;i<record.record.Count;i++)
                {
                    if(record.record[i].finish==false)
                    {
                        if(record.record[i].na<=fa&& record.record[i].nb<=fb&& record.record[i].nc<=fc&& record.record[i].nd<=fd)
                        {
                            ok = true;
                            record.record[i].Free = fa.ToString() + " " + fb.ToString() + " " + fc.ToString() + " " + fd.ToString();

                            fa += record.record[i].aa;
                            fb += record.record[i].ab;
                            fc += record.record[i].ac;
                            fd += record.record[i].ad;
                            record.record[i].FreeAllocation = fa.ToString() + " " + fb.ToString() + " " + fc.ToString() + " " + fd.ToString();

                            record.record[i].finish = true;



                            record.record.Add(record.record[i]);
                            record.record.RemoveAt(i);
                            break;
                        }
                    }
                }

            } while (ok);
            if( record.record[0].finish == false )
            {
                record = RecordCopy;
                textBlock.Text = "并没有安全序列，系统不安全。";
            }else
            {
                foreach(var i in record.record)
                {
                }

                textBlock.Text = "系统是安全的，安全序列为:{";


                foreach (var i in record.record)
                    textBlock.Text += i.name+" ";
                textBlock.Text += "}";
            }


        }

        public check(ref MainWindow.wrap record)
        {
            InitializeComponent();
            try
            {
                label1.Content = record.record[record.requestIndex].name + "发出请求向量Request(" + record.ra.ToString() + "," + record.rb.ToString() + "," + record.rc.ToString() + "," + record.rd.ToString() + "),系统按银行家算法进行检查:";

                bank(ref record);
            }
            catch
            {
            }
            dataGrid.ItemsSource = record.record;


        }


    }
}
