using System;
using System.Windows;
using System.Windows.Input;

namespace FlipIt
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

        private System.Drawing.Point mouseLocation;
        public bool previewMode = false;

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (!previewMode)
            {
                var loc = new System.Drawing.Point((int)e.GetPosition(this).X, (int)e.GetPosition(this).Y);
                if (!mouseLocation.IsEmpty)
                {
                    // Terminate if mouse is moved a significant distance
                    if (Math.Abs(mouseLocation.X - loc.X) > 5 ||
                        Math.Abs(mouseLocation.Y - loc.Y) > 5)
                        Exit();
                }

                // Update current mouse location
                mouseLocation = loc;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Exit();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Exit();
        }

        private void Exit()
        {
            if (!previewMode) Environment.Exit(0);
        }
    }
}