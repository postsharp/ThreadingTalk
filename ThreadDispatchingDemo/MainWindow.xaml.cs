using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [Background]
        private void DoStuff()
        {
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
        }

        [Dispatched(true)]
        private void SetProgress( int progress )
        {
            this.progressBar.Value = progress;
        }

        [Dispatched(true)]
        private void EnableControls( bool enabled )
        {
            this.startButton.IsEnabled = enabled;
        }
    }
}
