using FlipIt.Views;
using System;
using System.Windows;
using System.Windows.Interop;

namespace FlipIt
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Used to host WPF content in preview mode, attach HwndSource to parent Win32 window.
        private HwndSource preview;
        private MainWindow winSaver;

        internal App()
        {
            GlobalData.Load();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Preview mode--display in little window in Screen Saver dialog
            // (Not invoked with Preview button, which runs Screen Saver in
            // normal /s mode).
            if (e.Args[0].ToLower().StartsWith("/p"))
            {
                winSaver = new MainWindow() { previewMode = true };

                //previewHandle
                var pPreviewHnd = new IntPtr(Convert.ToInt32(e.Args[1]));

                var lpRect = new Rect();
                _ = Win32API.GetClientRect(pPreviewHnd, ref lpRect);

                var sourceParams = new HwndSourceParameters("sourceParams")
                {
                    PositionX = 0,
                    PositionY = 0,
                    Height = lpRect.Bottom - lpRect.Top,
                    Width = lpRect.Right - lpRect.Left,
                    ParentWindow = pPreviewHnd,
                    WindowStyle = (int)(WindowStyles.WS_VISIBLE | WindowStyles.WS_CHILD | WindowStyles.WS_CLIPCHILDREN)
                };

                preview = new HwndSource(sourceParams);
                preview.Disposed += new EventHandler(Preview_Disposed);
                preview.RootVisual = winSaver.viewBox;
            }

            // Normal screensaver mode.  Either screen saver kicked in normally,
            // or was launched from Preview button
            else if (e.Args[0].ToLower().StartsWith("/s"))
            {
                MainWindow win = new MainWindow();
                win.WindowState = WindowState.Maximized;
                win.Show();
            }

            // Config mode, launched from Settings button in screen saver dialog
            else if (e.Args[0].ToLower().StartsWith("/c"))
            {
                SettingsWindow settings = new SettingsWindow();
                settings.Show();
            }

            // If not running in one of the sanctioned modes, shut down the app
            // immediately (because we don't have a GUI).
            else
            {
                Current.Shutdown();
            }
        }

        /// <summary>
        /// Event that triggers when parent window is disposed--used when doing
        /// screen saver preview, so that we know when to exit.  If we didn't 
        /// do this, Task Manager would get a new .scr instance every time
        /// we opened Screen Saver dialog or switched dropdown to this saver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Preview_Disposed(object? sender, EventArgs e)
        {
            winSaver.Close();
        }
    }
}