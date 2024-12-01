using App_Demo_1.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
#nullable enable
namespace App_Demo_1.Services
{
    public class ThumbnailService : IThumbnailService
    {
        private readonly IMemoryCache _memoryCache;
        private SemaphoreSlim _semaphore = new(1); //Chỉ cho phép 1 luồng chạy 

        public ThumbnailService(IMemoryCache memoryCache) 
        {
            _memoryCache = memoryCache;
        }

        public async Task<BitmapImage?> GetThumbnail(StorageFile file)
        {
            BitmapImage? thumbnail = null;
            await _semaphore.WaitAsync(); //Chờ nếu đã có 1 luồng chạy trước
            try
            {
                if (_memoryCache.TryGetValue(file.Path, out thumbnail)) //Kiểm tra xem dữ liệu có trong cache hay chưa, nếu có, gán value vào thumbnail
                {
                    return thumbnail;
                }
                StorageItemThumbnail source = await file.GetThumbnailAsync(ThumbnailMode.PicturesView); //Lấy ảnh thu nhỏ của tệp
                thumbnail = new();
                await thumbnail.SetSourceAsync(source);
                _memoryCache.Set(file.Path, thumbnail); //Lưu dữ liệu vào cache
                return thumbnail;
            }
            finally // Đảm bảo luôn giải phóng semaphore bất kể ngoại lệ xảy ra
            {
                _semaphore.Release(); //Giải phóng semaphore, cho phép luồng khác vào
            }

            
        }
    }
}
