using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Trains.WP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection _transitions;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
	            rootFrame = new Frame {CacheSize = 1};

	            // TODO: change this value to a cache size that is appropriate for your application

	            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                var setup = new Setup(rootFrame);
                setup.Initialize();

                var start = Mvx.Resolve<IMvxAppStart>();
                start.Start();
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
        private static void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            if (frame == null) return;
            if (!frame.CanGoBack) return;
            frame.GoBack();
            e.Handled = true;
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol)
            {

	            var frame = Window.Current.Content as Frame ?? new Frame();

	            // Navigates to MainPage, passing the Uri to it.
                //frame.Navigate(typeof(MainPage), uri);

                base.OnActivated(args);

                Window.Current.Content = frame;

                // Ensure the current window is active
                Window.Current.Activate();
            }

            base.OnActivated(args);
        }
    }

}