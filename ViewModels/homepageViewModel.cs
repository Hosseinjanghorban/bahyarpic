using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Media.Imaging;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Windows.Navigation;
using System.Windows.Interop;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using Telerik.Charting;
using sample_project.Data_access;
using sample_project.Model;
using System.Collections.ObjectModel;
using Emgu.CV.Util;

namespace sample_project.ViewModels
{
    public class homepageViewModel : BindableBase
    {
        private Mat _originalImage;
        private Mat _adjustedImage;
        private Mat _adjustedImage2;
        public string savepath { get; set; }

        private BitmapImage _displayImage;
        public BitmapImage DisplayImage
        {
            get => _displayImage;
            set => SetProperty(ref _displayImage, value);
        }

        private double _brightness;
        public double Brightness
        {
            get => _brightness;
            set => SetProperty(ref _brightness, value);
        }

        private double _contrast;
        public double Contrast
        {
            get => _contrast;
            set => SetProperty(ref _contrast, value);
        }

        public DelegateCommand ApplyAdjustmentsCommand { get; }
        public DelegateCommand ApplySharpeningCommand { get; }

       public string filePath { get; set; }

        public DelegateCommand openfile { get; set; }
        public DelegateCommand histogram { get; set; }
        public DelegateCommand sharpening { get; set; }
        public OpenFileDialog filedialog { get; set; }
        public DelegateCommand save_image { get; set; }
        public DelegateCommand change_image { get; set; }
        public img_data_access image_dataaccess = new img_data_access();
        private ObservableCollection<image> _list_image;
        public image tmp_image { get; set; }

        private int _selectindex ;

        public int selectindex 
        {
            get { return _selectindex ; }
            set { _selectindex  = value; }
        }
        //private ObservableCollection<HistogramDataItem> _histogramData;
        //public ObservableCollection<HistogramDataItem> HistogramData { 
        //    get { return _histogramData; }
        //    set { SetProperty(ref _histogramData, value); }
        //}
        public ObservableCollection<image> list_image
        {
            get { return _list_image; }
            set { _list_image = value; }
        }

        public string UserName { get; set; }
        private readonly ISharedDataService _sharedDataService;

        homepageViewModel(ISharedDataService sharedDataService, IDialogService dialogService)
        {
            filedialog = new OpenFileDialog();
            openfile = new DelegateCommand(do_openfile);
            save_image = new DelegateCommand(do_saveimage);
            change_image = new DelegateCommand(do_changeimage);
            sharpening = new DelegateCommand(OnApplySharpening);
           // histogram = new DelegateCommand(change_histogram);
            ApplyAdjustmentsCommand = new DelegateCommand(OnApplyAdjustments);
            ApplySharpeningCommand = new DelegateCommand(OnApplySharpening);
            _sharedDataService = sharedDataService;
            UserName = _sharedDataService.list_user[_sharedDataService.user_id].name;
            //database image
           // image_dataaccess.readdata_img(list_image,_sharedDataService.user_id);
           list_image = new ObservableCollection<image>();
        }

        private void do_changeimage()
        {
            string tmp;
            try
            {
            tmp =list_image[selectindex].path;
                filePath = tmp;
            }
            catch
            {
                MessageBox.Show("choose a image first!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            if (filePath != null)
            {
            // Load the image using Emgu.CV
            _originalImage = CvInvoke.Imread(filePath, ImreadModes.Color);
            _adjustedImage = _originalImage.Clone();

            // Display the image
            UpdateDisplayImage(_originalImage);
            }
           
        }

        private void do_saveimage()
        {
            if (_adjustedImage == null)
            {
                MessageBox.Show("There is no adjusted image to save.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Show a Save File Dialog
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JPEG Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp",
                Title = "Save Adjusted Image",
                FileName = "AdjustedImage"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Save the image
                    savepath = saveFileDialog.FileName;

                    // Save the image using Emgu.CV
                    _adjustedImage.Save(savepath);

                    MessageBox.Show("Image saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            //add to list image 
            tmp_image = new image();
            tmp_image.path = savepath;
            List<string> result = SplitString(savepath, '\\');
            tmp_image.title = result[result.Count - 1];
            list_image.Add(tmp_image);
        }

        private void OnApplyAdjustments()
        {
            if (_originalImage == null)
            {
                MessageBox.Show("Please load an image first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Adjust brightness and contrast
            double contrastFactor = 1 + (Contrast / 100.0);
            double brightnessOffset = Brightness;

            _adjustedImage = new Mat();
            _originalImage.ConvertTo(_adjustedImage, DepthType.Cv8U, contrastFactor, brightnessOffset);

            // Display the adjusted image
            UpdateDisplayImage(_adjustedImage);
        }
        private void OnApplySharpening()
        {
            if (_originalImage == null)
            {
                MessageBox.Show("Please load an image first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create a sharpening kernel
            float[,] kernelData =
            {
        { -1, -1, -1 },
        { -1,  9, -1 },
        { -1, -1, -1 }
      };

            var kernel = new Matrix<float>(kernelData);

            // Ensure the destination Mat (_adjustedImage) has the same size and type as the source (_originalImage)
            _adjustedImage2 = new Mat(_adjustedImage.Rows, _adjustedImage.Cols, _adjustedImage.Depth, _adjustedImage.NumberOfChannels);

            // Apply the kernel to the image
            CvInvoke.Filter2D(_adjustedImage, _adjustedImage2, kernel, new System.Drawing.Point(-1, -1));

            // Display the sharpened image
            _adjustedImage = _adjustedImage2;
            UpdateDisplayImage(_adjustedImage);
        }

       /* private void change_histogram()
        {
            // ایجاد ماتریس برای عکس خاکستری
             Mat grayImage = new Mat();
            CvInvoke.CvtColor(_originalImage, grayImage, ColorConversion.Bgr2Gray);
            // محاسبه هیستوگرام
             VectorOfMat vm = new VectorOfMat();
            vm.Push(grayImage);
            Mat hist = new Mat();
            int[] histSize = { 256 };
           // RangeF[] ranges = { new RangeF(0.0f, 256.0f) };
            float[] ranges = { 0.0f, 256.0f };
            int[] channels = { 0 };
            CvInvoke.CalcHist(new VectorOfMat(grayImage), channels, null, hist, histSize, ranges, false);            // پر کردن داده‌های هیستوگرام
            for (int i = 0; i < hist.Rows; i++)
            {// float[] data = new float[1];
                float data = (float)hist.GetValue(i, 0);
                HistogramData.Add(new HistogramDataItem { Key = i, Value = data });
               // HistogramData.Add(new HistogramDataItem { Key = i, Value = data[0] });
            }

            //float[] histData = new float[histSize[0]];
            //hist.GetArray(0, 0, histData);
            //  HistogramData.Clear();
            //for (int i = 0; i < histData.Length; i++) { 
            //    HistogramData.Add(new HistogramDataItem { Key = i, Value = histData[i] });
            //}
        }

        public static float[] ComputeGrayscaleHistogram(Mat image)
        {
            if (image.NumberOfChannels > 1)
            {
                // Convert to grayscale if needed
                CvInvoke.CvtColor(image, image, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            }

            // Initialize histogram parameters
            int[] bins = { 256 };        // Number of bins
            float[] range = { 0, 256 }; // Intensity range
            Mat hist = new Mat();

            // Calculate the histogram
            CvInvoke.CalcHist(
                new[] { image },         // Input image(s)
                new int[] { 0 },         // Channels to compute
                null,                    // No mask
                hist,                    // Output histogram
                bins,                    // Number of bins
                new[] { range }          // Range
            );

            // Convert histogram to float array for display
            float[] histogramData = new float[bins[0]];
            hist.CopyTo(histogramData);
            return histogramData;
        }*/
        private void UpdateDisplayImage(Mat mat)
        {
            Bitmap bitmap = mat.ToBitmap();
            DisplayImage = BitmapToImageSource(bitmap);
        }
        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
        private void do_openfile()
        {
            filedialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // filedialog.ShowDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                filePath= openFileDialog.FileName;

                // Load the image using Emgu.CV
                _originalImage = CvInvoke.Imread(filePath, ImreadModes.Color);
                _adjustedImage = _originalImage.Clone();

                // Display the image
                UpdateDisplayImage(_originalImage);
            }
            else
            {
                MessageBox.Show("Image file not found.");
            }
            //add to list image 
           tmp_image =new image();
            tmp_image.path = filePath;
            List<string> result = SplitString(filePath, '\\' );
            tmp_image.title = result[result.Count - 1];
            list_image.Add(tmp_image);
           
        }
        public static List<string> SplitString(string input, char separator)
        { 
            string[] parts = input.Split(separator);
            return new List<string>(parts); 
        }

    }
}
