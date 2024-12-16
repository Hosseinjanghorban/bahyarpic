using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.CV;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;
using sample_project.Data_access;

namespace sample_project.ViewModels
{
    public class histogramViewModel : BindableBase
    {
        private ObservableCollection<HistogramDataItem> _histogramData;
        public ObservableCollection<HistogramDataItem> HistogramData
        {
            get { return _histogramData; }
            set { SetProperty(ref _histogramData, value); }

        }
        public DelegateCommand change_histogram { get; set; }

        public string path;

        private readonly ISharedDataService _sharedDataService;
        public histogramViewModel(ISharedDataService sharedDataService)
        {
            HistogramData = new ObservableCollection<HistogramDataItem>();
            change_histogram = new DelegateCommand(do_histogram);
            _sharedDataService = sharedDataService;
            path = _sharedDataService.path_histogram;
            ProcessImage(path);
        }

        private void do_histogram()
        {
            path = _sharedDataService.path_histogram;
            ProcessImage(path);
        }

        private void ProcessImage(string imagePath)
        {
            if (!File.Exists(imagePath))
                return;
            Mat image = CvInvoke.Imread(imagePath, ImreadModes.Color);
            Mat grayImage = new Mat();
            CvInvoke.CvtColor(image, grayImage, ColorConversion.Bgr2Gray);

            Mat hist = new Mat();
            int[] histSize = { 256 };
            float[] ranges = { 0.0f, 256.0f };
            int[] channels = { 0 };
            CvInvoke.CalcHist(new VectorOfMat(grayImage), channels, null, hist, histSize, ranges, false);
            HistogramData.Clear();
            float[] histData = new float[histSize[0]];
            System.Runtime.InteropServices.Marshal.Copy(hist.DataPointer, histData, 0, histData.Length);
            for (int i = 0; i < histData.Length; i++)
            {
                HistogramData.Add(new HistogramDataItem { Key = i, Value = histData[i] });
            }
        }
    }
}
