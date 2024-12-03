﻿
using App_Demo_1.Interfaces;
using App_Demo_1.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Threading.Tasks;
using Windows.Storage;

#nullable enable

namespace App_Demo_1.ViewModels
{
    [ObservableObject]
    public partial class PhotoViewModel
    {
        private readonly StorageFile _file;
        private readonly IThumbnailService _thumbnailService;
        private readonly IMetadataService _metadataService;
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

        public PhotoViewModel(StorageFile file, IThumbnailService thumbnailService, IMetadataService metadataService)
        {
            
            _file = file;
            InputFileName = _file.Name;
            InputFilePath = _file.Path.ToString();

            _thumbnailService = thumbnailService;
            

        }

        public async Task LoadThumbnailAsync()
        {
            if (Thumbnail == null) 
            {
                Thumbnail = await _thumbnailService.GetThumbnail(_file);
            }

        }


    }
}
