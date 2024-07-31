using System;
using System.Diagnostics;
using System.Resources;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Trafikläget
{
	public partial class App : Application
	{

		public static PhoneApplicationFrame RootFrame { get; private set; }

		public App()
		{
			UnhandledException += Application_UnhandledException;
			InitializeComponent();
			InitializePhoneApplication();
			if (Debugger.IsAttached)
			{
				Application.Current.Host.Settings.EnableFrameRateCounter = true;
				PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
			}
		}

		private void Application_ContractActivated(object sender, Windows.ApplicationModel.Activation.IActivatedEventArgs e)
		{
		}

		private void Application_Launching(object sender, LaunchingEventArgs e)
		{
		}

		private void Application_Activated(object sender, ActivatedEventArgs e)
		{
		}

		private void Application_Deactivated(object sender, DeactivatedEventArgs e)
		{
		}

		private void Application_Closing(object sender, ClosingEventArgs e)
		{
		}

		private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			if (Debugger.IsAttached)
			{
				Debugger.Break();
			}
		}

		private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			if (Debugger.IsAttached)
			{
				Debugger.Break();
			}
		}

		#region Phone application initialization

		// Avoid double-initialization
		private bool phoneApplicationInitialized = false;

		// Do not add any additional code to this method
		private void InitializePhoneApplication()
		{
			if (phoneApplicationInitialized)
				return;

			// Create the frame but don't set it as RootVisual yet; this allows the splash
			// screen to remain active until the application is ready to render.
			RootFrame = new PhoneApplicationFrame();
			RootFrame.Navigated += CompleteInitializePhoneApplication;

			// Handle navigation failures
			RootFrame.NavigationFailed += RootFrame_NavigationFailed;

			// Handle reset requests for clearing the backstack
			RootFrame.Navigated += CheckForResetNavigation;

			// Handle contract activation such as a file open or save picker
			PhoneApplicationService.Current.ContractActivated += Application_ContractActivated;

			// Ensure we don't initialize again
			phoneApplicationInitialized = true;
		}

		// Do not add any additional code to this method
		private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
		{
			// Set the root visual to allow the application to render
			if (RootVisual != RootFrame)
				RootVisual = RootFrame;

			// Remove this handler since it is no longer needed
			RootFrame.Navigated -= CompleteInitializePhoneApplication;
		}

		private void CheckForResetNavigation(object sender, NavigationEventArgs e)
		{
			// If the app has received a 'reset' navigation, then we need to check
			// on the next navigation to see if the page stack should be reset
			if (e.NavigationMode == NavigationMode.Reset)
				RootFrame.Navigated += ClearBackStackAfterReset;
		}

		private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
		{
			// Unregister the event so it doesn't get called again
			RootFrame.Navigated -= ClearBackStackAfterReset;

			// Only clear the stack for 'new' (forward) and 'refresh' navigations
			if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
				return;

			// For UI consistency, clear the entire page stack
			while (RootFrame.RemoveBackEntry() != null)
			{
				; // do nothing
			}
		}

		#endregion

		//private void InitializeLanguage()
		//{
		//	try
		//	{
		//		// Set the font to match the display language defined by the
		//		// ResourceLanguage resource string for each supported language.
		//		//
		//		// Fall back to the font of the neutral language if the Display
		//		// language of the phone is not supported.
		//		//
		//		// If a compiler error is hit then ResourceLanguage is missing from
		//		// the resource file.
		//		//RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

		//		// Set the FlowDirection of all elements under the root frame based
		//		// on the ResourceFlowDirection resource string for each
		//		// supported language.
		//		//
		//		// If a compiler error is hit then ResourceFlowDirection is missing from
		//		// the resource file.
		//		FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
		//		RootFrame.FlowDirection = flow;
		//	}
		//	catch
		//	{
		//		// If an exception is caught here it is most likely due to either
		//		// ResourceLangauge not being correctly set to a supported language
		//		// code or ResourceFlowDirection is set to a value other than LeftToRight
		//		// or RightToLeft.

		//		if (Debugger.IsAttached)
		//		{
		//			Debugger.Break();
		//		}

		//		throw;
		//	}
		//}
	}
}