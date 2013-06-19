using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PostSharp.Patterns.Threading;

namespace ThreadDispatchingDemo
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

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            this.EnableControls( false );
            this.DoStuff();
        }

        private void DoStuff()
        {
            ThreadPool.QueueUserWorkItem( state => { 
            Random random = new Random();
            for ( int i = 0; i < 100; i++ )
            {
                for ( int j = 0; j < 1000000; j++ )
                {
                    Math.Sin( random.NextDouble() );
                }
                this.SetProgress( i );

            }

            this.EnableControls( true );
        });
    }

         private void SetProgress( int progress )
         {
             this.Dispatcher.BeginInvoke( new Action( () =>
                                                          {
                                                              this.progressBar.Value = progress;
                                                          } ) );
         }

        [Dispatched]
        private void EnableControls( bool enabled )
        {
            this.Dispatcher.BeginInvoke( new Action( () =>
                                                         {
                                                             this.startButton.IsEnabled = enabled;
                                                         } ));
        }
    }
}
