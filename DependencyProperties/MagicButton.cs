using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DependencyProperties
{
    public static class MagicButton
    {
        public static readonly DependencyProperty IsShareButtonProperty =
            DependencyProperty.RegisterAttached(
            "IsShareButton",
            typeof(Boolean),
            typeof(MagicButton),
            new PropertyMetadata(false, Changed));

        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (button == null) return;
            if (e.NewValue != null && (bool)e.NewValue)
            {
                button.Click += ButtonClick;
            }
            else
            {
                button.Click -= ButtonClick;
            }
        }

        static void ButtonClick(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        public static void SetIsShareButton(UIElement element, Boolean value)
        {
            element.SetValue(IsShareButtonProperty, value);
        }

        public static Boolean GetIsShareButton(UIElement element)
        {
            return (Boolean)element.GetValue(IsShareButtonProperty);
        }
    }

}
