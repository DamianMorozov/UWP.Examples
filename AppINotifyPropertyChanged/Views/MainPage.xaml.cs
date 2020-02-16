using Windows.UI.Xaml.Controls;
using AppINotifyPropertyChanged.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppINotifyPropertyChanged.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        /// <summary>
        /// 
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        private void TextBoxFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Resources != null && Resources.Count > 0)
            {
                var context = Resources["MyFontSettings"];
                if (context != null && context is FontSettings fontSettings)
                {
                    PageMain.FontSize = fontSettings.Size;
                    TextBlockFont.FontSize = fontSettings.Size;
                    TextBoxFontSize.FontSize = fontSettings.Size;
                }
            }
        }
    }
}
