
using Microsoft.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.Storage;

namespace App_Demo_1.Interfaces
{
    public interface IThumbnailService
    {
        Task<BitmapImage> GetThumbnail(StorageFile file);
    }
}
