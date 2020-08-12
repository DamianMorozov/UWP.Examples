using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace XamlControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DefaultCloseCommand : Page
    {
        MainPage rootPage = MainPage.Current;

        public DefaultCloseCommand()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Click handler for the 'DefaultCloseCommandButton' button.
        /// Demonstrates showing a message dialog with a default close command and content.
        /// A message will be displayed indicating that the dialog has been closed.
        /// In this scenario, the only command is the default 'Close' command that is used if no other commands are specified.
        /// </summary>
        /// <param name="sender">The Object that caused this event to be fired.</param>
        /// <param name="e">State information and event data associated with the routed event.</param>
        private async void DefaultCloseCommandButton_Click(object sender, RoutedEventArgs e)
        {
            // Create the message dialog and set its content; it will get a default "Close" button since there aren't any other buttons being added
            var messageDialog = new MessageDialog("You've exceeded your trial period.");

            // Show the message dialog and wait
            await messageDialog.ShowAsync();
            rootPage.NotifyUser("The dialog has now been closed", NotifyType.StatusMessage);
        }
    }
}
