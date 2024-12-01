using App_Demo_1.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;


#nullable enable

namespace App_Demo_1.ViewModels;

[ObservableObject] //Tạo ra các thuộc tính kế thừa INotifyPropertyChanged
public partial class MainViewModel
{
    [ObservableProperty] //Tự động tạo getter và setter cho các trường và thông báo khi giá trị các trường thay đổi
    private StorageFolder? _inputFolder; // Dấu ? thể hiện _inputFolder có thể chứa giá trị null
    [ObservableProperty]
    private StorageFolder? _outputFolder;
    [ObservableProperty]
    private ObservableCollection<PhotoViewModel> _photos = new();
    private readonly IThumbnailService _thumbnailService;

    public MainViewModel(IThumbnailService thumbnailService) 
    {
        _thumbnailService = thumbnailService;
    }
    [RelayCommand]
    private Task PreparePhotoAsync(int photoIndex)
    {
        PhotoViewModel photoViewModel = Photos[photoIndex];
        return photoViewModel.LoadThumbnailAsync();
    }
    [RelayCommand]
    private async Task UpdateInputFolderPath(string? folderPath) //Hàm không đồng bộ mang mục đích là cập nhập đường dẫn cho InputFolder
    {
        if (folderPath == null) return; //Kiểm tra folderPath (chuỗi đưa vào), null thì kết thúc hàm
        StorageFolder? folder = await StorageFolder.GetFolderFromPathAsync(folderPath);
        if (folder == null) return; //Lấy thư mục từ đường dẫn
        InputFolder = folder;
    }
    [RelayCommand]
    private async Task UpdateOutputFolderPath(string? folderPath) //Hàm không đồng bộ mang mục đích là cập nhập đường dẫn cho InputFolder
    {
        if (folderPath == null) return; //Kiểm tra folderPath (chuỗi đưa vào), null thì kết thúc hàm
        StorageFolder? folder = await StorageFolder.GetFolderFromPathAsync(folderPath); //Lấy thư mục từ đường dẫn
        if (folder == null) return;
        OutputFolder = folder;
    }
    [RelayCommand]
    private async Task LoadPhotoAsync(string? inputFolderPath)
    {
        if (LoadPhotoCommand.IsRunning || inputFolderPath is null)  return;
        StorageFolder? folder = await StorageFolder.GetFolderFromPathAsync(inputFolderPath); //Lấy các folder từ đường dẫn input
        if (folder is null) return;
        Photos.Clear();
        List<string> filesTypeFilter = new List<string>();    //Tạo 1 list các filter file có trong folder
        filesTypeFilter.Add(".jpng");
        filesTypeFilter.Add(".jpg");
        QueryOptions queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery, filesTypeFilter); //Tạo 1 truy vấn 
        queryOptions.FolderDepth = FolderDepth.Deep; //Truy vấn đến cả folder con
        StorageFileQueryResult queryResult = folder.CreateFileQueryWithOptions(queryOptions);
        IReadOnlyList<StorageFile>? files = await queryResult.GetFilesAsync(); //Lấy kết quả truy vấn file
        if (files is null) return;
        List<PhotoViewModel> photoViewModels = new List<PhotoViewModel>(); //Tạo 1 list có kiểu dữ liệu đã quy định trước
        
        foreach(StorageFile file in files) //Duyệt danh sách kết quả truy vấn file
        {
            PhotoViewModel photoViewModel = new(file, _thumbnailService); //Mỗi item tạo 1 biến lưu trữ dữ liệu và add vào list
            photoViewModels.Add(photoViewModel);
        }
        
        Photos = new ObservableCollection<PhotoViewModel>(photoViewModels);

    }
}

