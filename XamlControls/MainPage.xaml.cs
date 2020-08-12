// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

using System.ServiceModel.Channels;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using XamlControls.Views;
using Windows.UI.Xaml.Navigation;
//using SDKTemplate;
using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Media;

namespace XamlControls
{
    public enum NotifyType
    {
        StatusMessage,
        ErrorMessage
    };

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage
    {
        public static MainPage Current;

        #region Private helpers

        private PageNumberBox _pageNumberBox;
        private NavigationTransitionInfo _navigationTransitionInfo = new CommonNavigationTransitionInfo();

        #endregion


        public MainPage()
        {
            InitializeComponent();

            Current = this;
            SampleTitle.Text = FEATURE_NAME;
        }

        private void ButtonHome_OnClick(object sender, RoutedEventArgs e)
        {
            _pageNumberBox = null;
            //FrameMain.Navigate(typeof(Page), null);
        }

        private void ComboBoxNavigationTransitionInfo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //switch (ComboBoxNavigationTransitionInfo.SelectedIndex)
            //{
            //    case 1:
            //        _navigationTransitionInfo = new ContinuumNavigationTransitionInfo();
            //        break;
            //    case 2:
            //        _navigationTransitionInfo = new DrillInNavigationTransitionInfo();
            //        break;
            //    case 3:
            //        _navigationTransitionInfo = new EntranceNavigationTransitionInfo();
            //        break;
            //    case 4:
            //        _navigationTransitionInfo = new SlideNavigationTransitionInfo();
            //        break;
            //    case 5:
            //        _navigationTransitionInfo = new SuppressNavigationTransitionInfo();
            //        break;
            //    default:
            //        _navigationTransitionInfo = new CommonNavigationTransitionInfo();
            //        break;
            //}
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            //rootPage.NotifyUser("The '" + command.Label + "' command has been selected.", NotifyType.StatusMessage);
        }

        private async void ButtonNumberBox_OnClick(object sender, RoutedEventArgs e)
        {
            //var messageDialog = new MessageDialog(_navigationTransitionInfo.ToString());
            //messageDialog.Commands.Add(new UICommand("Renew", new UICommandInvokedHandler(CommandInvokedHandler)));
            //messageDialog.Commands.Add(new UICommand("Close", new UICommandInvokedHandler(CommandInvokedHandler)));
            //messageDialog.DefaultCommandIndex = 0;
            //messageDialog.CancelCommandIndex = 1;
            //await messageDialog.ShowAsync();

            //if (_pageNumberBox == null)
            //    _pageNumberBox = new PageNumberBox();
            //if (FrameMain.Content != null)
            //{
            //    if (!(FrameMain.Content is PageNumberBox))
            //        FrameMain.Navigate(typeof(PageNumberBox), null, _navigationTransitionInfo);
            //}
            //else
            //    FrameMain.Navigate(typeof(PageNumberBox), null, _navigationTransitionInfo);
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }

        #region Private methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Populate the scenario list from the SampleConfiguration.cs file
            var itemCollection = new List<Scenario>();
            int i = 1;
            foreach (Scenario s in scenarios)
            {
                itemCollection.Add(new Scenario { Title = $"{i++}) {s.Title}", ClassType = s.ClassType });
            }
            ScenarioControl.ItemsSource = itemCollection;

            if (Window.Current.Bounds.Width < 640)
            {
                ScenarioControl.SelectedIndex = -1;
            }
            else
            {
                ScenarioControl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Called whenever the user changes selection in the scenarios list.  This method will navigate to the respective
        /// sample scenario page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScenarioControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear the status block when navigating scenarios.
            NotifyUser(String.Empty, NotifyType.StatusMessage);

            ListBox scenarioListBox = sender as ListBox;
            Scenario s = scenarioListBox.SelectedItem as Scenario;
            if (s != null)
            {
                ScenarioFrame.Navigate(s.ClassType);
                if (Window.Current.Bounds.Width < 640)
                {
                    Splitter.IsPaneOpen = false;
                }
            }
        }

        public List<Scenario> Scenarios
        {
            get { return this.scenarios; }
        }

        /// <summary>
        /// Display a message to the user.
        /// This method may be called from any thread.
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="type"></param>
        public void NotifyUser(string strMessage, NotifyType type)
        {
            // If called from the UI thread, then update immediately.
            // Otherwise, schedule a task on the UI thread to perform the update.
            if (Dispatcher.HasThreadAccess)
            {
                UpdateStatus(strMessage, type);
            }
            else
            {
                var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => UpdateStatus(strMessage, type));
            }
        }

        private void UpdateStatus(string strMessage, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.StatusMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                    break;
                case NotifyType.ErrorMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    break;
            }

            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            StatusBorder.Visibility = (StatusBlock.Text != String.Empty) ? Visibility.Visible : Visibility.Collapsed;
            if (StatusBlock.Text != String.Empty)
            {
                StatusBorder.Visibility = Visibility.Visible;
                StatusPanel.Visibility = Visibility.Visible;
            }
            else
            {
                StatusBorder.Visibility = Visibility.Collapsed;
                StatusPanel.Visibility = Visibility.Collapsed;
            }

            // Raise an event if necessary to enable a screen reader to announce the status update.
            var peer = FrameworkElementAutomationPeer.FromElement(StatusBlock);
            if (peer != null)
            {
                peer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
            }
        }

        private async void Footer_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }


        #endregion
    }
}
