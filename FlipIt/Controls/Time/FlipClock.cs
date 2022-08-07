using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FlipIt.Controls
{
    public enum TimeFormat
    {
        [Description("24 Hours")]
        TwentyFourHours,
        [Description("12 Hours")]
        TwelveHours
    }

    //https://github.com/HandyOrg/HandyControl/blob/0633da94459b3f636f2fe4263098b2cff9aa1b51/src/Shared/HandyControl_Shared/Controls/Time/FlipClock/FlipClock.cs
    public class FlipClock : Control
    {
        private readonly DispatcherTimer _dispatcherTimer;

        private bool _isDisposed;

        public static readonly DependencyProperty NumberListProperty = DependencyProperty.Register(
            nameof(NumberList), typeof(List<int>), typeof(FlipClock), new PropertyMetadata(new List<int> { 0, 0, 0, 0, 0, 0 }));

        public List<int> NumberList
        {
            get => (List<int>)GetValue(NumberListProperty);
            set => SetValue(NumberListProperty, value);
        }

        public static readonly DependencyProperty DisplayTimeProperty = DependencyProperty.Register(
            nameof(DisplayTime), typeof(DateTime), typeof(FlipClock), new PropertyMetadata(default(DateTime), OnDisplayTimeChanged));

        private static void OnDisplayTimeChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var control = (FlipClock)s;
            var value = (DateTime)e.NewValue;

            control.NumberList = new List<int>
            {
                control.TimeFormat == TimeFormat.TwentyFourHours ? value.Hour / 10 : value.ToString("tt").Equals("pm", StringComparison.OrdinalIgnoreCase) ? value.Hour switch { 10 or 11 or 12 => 1, _ => 0 } : value.Hour switch { 10 or 11 => 1, 12 => 0, _ => 0 },
                control.TimeFormat == TimeFormat.TwentyFourHours ? value.Hour % 10 : value.ToString("tt").Equals("pm", StringComparison.OrdinalIgnoreCase) ? value.Hour switch { 10 => 0, 11 => 1, 12 => 2, _ => value.Hour % 12 } : value.Hour switch { 10 => 0, 11 => 1, 12 => 0, _ => value.Hour % 12 },
                value.Minute / 10,
                value.Minute % 10,
                value.Second / 10,
                value.Second % 10
            };
        }

        public DateTime DisplayTime
        {
            get => (DateTime)GetValue(DisplayTimeProperty);
            set => SetValue(DisplayTimeProperty, value);
        }

        public static readonly DependencyProperty VisibleSecondsProperty = DependencyProperty.Register(
             nameof(VisibleSeconds), typeof(bool), typeof(FlipClock), new PropertyMetadata(true));

        public bool VisibleSeconds
        {
            get => (bool)GetValue(VisibleSecondsProperty);
            set => SetValue(VisibleSecondsProperty, value);
        }

        public static readonly DependencyProperty VisibleDayOfWeekProperty = DependencyProperty.Register(
           nameof(VisibleDayOfWeek), typeof(bool), typeof(FlipClock), new PropertyMetadata(false));

        public bool VisibleDayOfWeek
        {
            get => (bool)GetValue(VisibleDayOfWeekProperty);
            set => SetValue(VisibleDayOfWeekProperty, value);
        }

        public static readonly DependencyProperty VisibleMonthProperty = DependencyProperty.Register(
           nameof(VisibleMonth), typeof(bool), typeof(FlipClock), new PropertyMetadata(false));

        public bool VisibleMonth
        {
            get => (bool)GetValue(VisibleMonthProperty);
            set => SetValue(VisibleMonthProperty, value);
        }

        public static readonly DependencyProperty VisibleDateProperty = DependencyProperty.Register(
           nameof(VisibleDate), typeof(bool), typeof(FlipClock), new PropertyMetadata(false));

        public bool VisibleDate
        {
            get => (bool)GetValue(VisibleDateProperty);
            set => SetValue(VisibleDateProperty, value);
        }

        public static readonly DependencyProperty TimeFormatTimeProperty = DependencyProperty.Register(
           nameof(TimeFormat), typeof(TimeFormat), typeof(FlipClock), new PropertyMetadata(TimeFormat.TwentyFourHours));

        public TimeFormat TimeFormat
        {
            get => (TimeFormat)GetValue(TimeFormatTimeProperty);
            set => SetValue(TimeFormatTimeProperty, value);
        }

        public FlipClock()
        {
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };

            IsVisibleChanged += FlipClock_IsVisibleChanged;
        }

        ~FlipClock() => Dispose();

        public void Dispose()
        {
            if (_isDisposed) return;

            IsVisibleChanged -= FlipClock_IsVisibleChanged;
            _dispatcherTimer.Stop();
            _isDisposed = true;

            GC.SuppressFinalize(this);
        }

        private void FlipClock_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                _dispatcherTimer.Tick += DispatcherTimer_Tick;
                _dispatcherTimer.Start();
            }
            else
            {
                _dispatcherTimer.Stop();
                _dispatcherTimer.Tick -= DispatcherTimer_Tick;
            }
        }

        private void DispatcherTimer_Tick(object? sender, EventArgs e) => DisplayTime = DateTime.Now;
    }
}