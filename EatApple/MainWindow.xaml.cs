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
using System.Threading;

namespace EatApple
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

        private  object lockObj = new object();
        enum plateState { empty,apple,orange};
        plateState plate = plateState.empty;
        public  void son()
        {
            while (true)
                lock (lockObj)
                {
                    if(plate == plateState.orange )
                    {
                        image.Source = new BitmapImage(new Uri(@"image/orange.jpg", UriKind.RelativeOrAbsolute));
                        label.Content = "儿子：我吃了桔子。";
                        System.Threading.Thread.Sleep(1000);
                        plate = plateState.empty;
                    }
                    else if(plate==plateState.apple)
                    {
                        image.Source = new BitmapImage(new Uri(@"image/apple.jpg", UriKind.RelativeOrAbsolute));
                        label.Content = "儿子：我不吃苹果。";
                        System.Threading.Thread.Sleep(500);
                    }
                    else if(plate == plateState.empty)
                    {
                        image.Source = new BitmapImage(new Uri(@"image/plate.jpg", UriKind.RelativeOrAbsolute));
                        label.Content = "空盘子，请等待。。。。";
                        System.Threading.Thread.Sleep(500);
                    }
                }
        }
        public  void daughter()
        {
            while (true)
                lock (lockObj)
                {
                    if (plate == plateState.orange)
                    {
                        image.Source = new BitmapImage(new Uri(@"image/orange.jpg", UriKind.RelativeOrAbsolute));
                        label.Content = "女儿：我不吃桔子。";
                        System.Threading.Thread.Sleep(500);
                    }
                    else if (plate == plateState.apple)
                    {
                        image.Source = new BitmapImage(new Uri(@"image/apple.jpg", UriKind.RelativeOrAbsolute));
                        label.Content = "女儿：我吃了苹果。";
                        System.Threading.Thread.Sleep(1000);
                        plate = plateState.empty;

                    }
                    else if (plate == plateState.empty)
                    {
                        image.Source = new BitmapImage(new Uri(@"image/plate.jpg", UriKind.RelativeOrAbsolute));
                        label.Content = "空盘子，请等待。。。。";
                        System.Threading.Thread.Sleep(500);
                    }
                }
        }
        public  void father()
        {
            while(true)
                lock (lockObj)
                {
                    if (plate == plateState.empty)
                    {
                        image.Source = new BitmapImage(new Uri(@"image/apple.jpg", UriKind.RelativeOrAbsolute));
                        label.Content = "父亲：我在盘子里放了苹果。";
                        System.Threading.Thread.Sleep(1000);
                        plate = plateState.apple;
                    }
                }
        }
        public  void mother()
        {
            while (true)
                lock (lockObj)
                {
                    if (plate == plateState.empty)
                    {
                        image.Source = new BitmapImage(new Uri(@"image/orange.jpg", UriKind.RelativeOrAbsolute));
                        label.Content = "母亲：我在盘子里放了桔子。";
                        System.Threading.Thread.Sleep(1000);
                        plate = plateState.orange;
                    }
                }

        }
        Thread thSon, thDaugther, thFather, thMother;
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            image.Source = new BitmapImage(new Uri(@"image/plate.jpg",UriKind.RelativeOrAbsolute));
            thSon = new Thread(son);
            thSon.IsBackground = true;
            thDaugther = new Thread(daughter);
            thDaugther.IsBackground = true;
            thMother = new Thread(mother);
            thMother.IsBackground = true;
            thFather = new Thread(father);
            thFather.IsBackground = true;

            thSon.Start();
            thDaugther.Start();
            thMother.Start();
            thFather.Start();


        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
