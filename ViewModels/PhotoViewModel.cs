
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using Windows.Storage;

namespace App_Demo_1.ViewModels
{
    [ObservableObject]
    public partial class PhotoViewModel
    {
        private readonly StorageFile _file;
        [ObservableProperty]
        private string? _inputFileName;
        [ObservableProperty]
        private string? _inputFilePath;
        [ObservableProperty]
        private string? _inputFileSize;
        [ObservableProperty]
        private DateTime? _dateTaken;
        [ObservableProperty]
        private string? _outputFilePath;
        [ObservableProperty]
        private BitmapImage _thumbnail; //Tải, giải mã và hiện thị hình ảnh

        public PhotoViewModel(StorageFile file)
        {
            _file = file;
            InputFileName = _file.Name;
            InputFilePath = _file.Path.ToString();
        }
    }
}
