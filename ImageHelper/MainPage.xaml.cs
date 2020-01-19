using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP.Example.ImageHelper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WriteableBitmap _writeableBitmap;
        private ShareOperation _shareOperation;
        private RandomAccessStreamReference _bitmap;

        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var args = e.Parameter as ShareTargetActivatedEventArgs;
            if (args != null)
            {
                _shareOperation = args.ShareOperation;

                if (_shareOperation.Data.Contains(StandardDataFormats.Bitmap))
                {
                    _bitmap = await _shareOperation.Data.GetBitmapAsync();
                    await ProcessBitmap();
                }
                else if (_shareOperation.Data.Contains(StandardDataFormats.StorageItems))
                {
                    IReadOnlyList<IStorageItem> items = await _shareOperation.Data.GetStorageItemsAsync();
                    await ProcessStorageItems(items);
                }
                else _shareOperation.ReportError(
                    "Image Helper was unable to find a valid bitmap.");
            }
        }

        private async Task LoadBitmap(IRandomAccessStream stream)
        {
            _writeableBitmap = new WriteableBitmap(1, 1);
            _writeableBitmap.SetSource(stream);
            _writeableBitmap.Invalidate();
            await Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal,
                () => imageTarget.Source = _writeableBitmap);
        }

        private async Task ProcessBitmap()
        {
            if (_bitmap != null)
            {
                await LoadBitmap(await _bitmap.OpenReadAsync());
            }
        }

        private async Task ProcessStorageItems(IReadOnlyList<IStorageItem> items)
        {
            foreach (var item in items)
            {
                if (item.IsOfType(StorageItemTypes.File))
                {
                    var file = item as StorageFile;
                    if (file.ContentType.StartsWith(
                        "image",
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        await LoadBitmap(await file.OpenReadAsync());
                        break;
                    }
                }
            }
        }

        private void CheckAndClearShareOperation()
        {
            if (_shareOperation != null)
            {
                _shareOperation.ReportCompleted();
                _shareOperation = null;
            }
        }

        private async void buttonCapture_Click(object sender, RoutedEventArgs e)
        {
            CheckAndClearShareOperation();
            var camera = new CameraCaptureUI();
            var result = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (result != null)
            {
                await LoadBitmap(await result.OpenAsync(FileAccessMode.Read));
            }
        }

        private async void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (_writeableBitmap != null)
            {
                var picker = new FileSavePicker
                {
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary
                };
                picker.FileTypeChoices.Add("Image", new List<string>() { ".png" });
                picker.DefaultFileExtension = ".png";
                picker.SuggestedFileName = "photo";
                var savedFile = await picker.PickSaveFileAsync();

                try
                {
                    if (savedFile != null)
                    {
                        using (var output = await
                            savedFile.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            var encoder =
                                await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, output);

                            byte[] pixels;

                            using (var stream = _writeableBitmap.PixelBuffer.AsStream())
                            {
                                pixels = new byte[stream.Length];
                                await stream.ReadAsync(pixels, 0, pixels.Length);
                            }

                            encoder.SetPixelData(BitmapPixelFormat.Rgba8,
                                                    BitmapAlphaMode.Straight,
                                                    (uint)_writeableBitmap.PixelWidth,
                                                    (uint)_writeableBitmap.PixelHeight,
                                                    96.0, 96.0,
                                                    pixels);

                            await encoder.FlushAsync();
                            await output.FlushAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    var s = ex.ToString();
                }
                finally
                {
                    CheckAndClearShareOperation();
                }
            }
        }

        private void ShowPointerPressed(string source)
        {
            var text = string.Format($"Pointer pressed from {source}.");
            textEvents.Text = string.Format($"{textEvents.Text} {text}");
            Debug.WriteLine(text);
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ShowPointerPressed("Grid");
        }

        private void textEvents_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            textEvents.Text = string.Empty;
            ShowPointerPressed("textEvents");
        }

        private void ImageTarget_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            textEvents.Text = string.Empty;
            ShowPointerPressed("imageTarget");
            e.Handled = true;
        }
    }
}
