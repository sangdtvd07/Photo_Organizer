using App_Demo_1.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ScottPlot;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
#nullable enable
namespace App_Demo_1
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(App_Name);
            mainViewModel = Ioc.Default.GetService<MainViewModel>();
        }

        public MainViewModel mainViewModel { get; } // Cho phép truy cập tới MainViewModel mà không thay đổi giá trị trong nó

        // Hàm Folder Picker
        private StorageFolder? SelectedInputFolder { get; set; }
        private StorageFolder? SelectedOutputFolder { get; set; }
        private async Task<StorageFolder?> SelectFolder()
        {
            
            FolderPicker folderPicker = new FolderPicker(); // Tạo folder picker
            folderPicker.FileTypeFilter.Add("*"); // Loại tệp đc chọn * = mọi loại tệp
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop; // Gợi ý vị trí cửa sổ ban đầu
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this); // Liên kết với cửa sổ hiện tại
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);
            return await folderPicker.PickSingleFolderAsync(); //Trả về vị trí folder được chọn
        }


        private async void Start_Button_Clicked(Object sender, RoutedEventArgs e)
        {
            ContentDialogResult results = await StartSettingsDialog.ShowAsync();
            if (results is ContentDialogResult.Primary)
            {
                //ViewModel.InputFolder = SelectedInputFolder;
                //ViewModel.OutputFolder = SelectedOutputFolder;
                mainViewModel.UpdateInputFolderPathCommand.Execute(SelectedInputFolder?.Path);
                mainViewModel.UpdateOutputFolderPathCommand.Execute(SelectedOutputFolder?.Path);
                //Load Photo List
                mainViewModel.LoadPhotoCommand?.ExecuteAsync(SelectedInputFolder?.Path);     
            }
        }
        private async void input_button_clicked(Object sender, RoutedEventArgs e)
        {
            StorageFolder? folder = await SelectFolder();
            if (folder != null)
            {
                SelectedInputFolder = folder;
                Input_Folder_Selected.Text = SelectedInputFolder.Path;
            }
        }

        private async void output_button_clicked(Object sender, RoutedEventArgs e)
        {
            StorageFolder? folder = await SelectFolder();
            if (folder != null)
            {
                SelectedOutputFolder = folder;
                Output_Folder_Selected.Text = SelectedOutputFolder.Path;
            }
        }
        private void Folder_Structure_Clicked(Object sender, RoutedEventArgs e) => UpdateOutputFolderExample();

        private void UpdateOutputFolderExample()
        {
            string example = @"Output";
            if (SelectedOutputFolder?.Path.Length > 0)
            {
                example = SelectedOutputFolder.Path;
            }
            // Chuyển ngày giờ hiện tại thành chuỗi văn bản chỉ định (Hàm ngày giờ) và ko ảnh hưởng đến các ký tự đặc biệt
            example += DateTime.Now.ToString(CreateDateFolderFormat(),CultureInfo.InvariantCulture); 
            example += @"\Filename";
            Example_TextBlock.Text = example;
        }

        private string CreateDateFolderFormat()
        {
            string format = string.Empty;
            if (CreateYearOutCheckBox.IsChecked == true)
            {
                format += @"\\yyyy";
            }
            if (CreateMonthOutCheckBox.IsChecked == true)
            {
                format += @"\\MM";
            }
            if (CreateDayOutCheckBox.IsChecked == true)
            {
                format += @"\\dd";
            }
            else format = @"\\yyyy-MM-dd";
            return format;
        }

        private void PhotoList_ElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
        {
            mainViewModel?.PreparePhotoCommand?.ExecuteAsync(args.Index);
        }
    }
}
