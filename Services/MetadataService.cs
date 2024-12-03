using App_Demo_1.Interfaces;
using Humanizer;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace App_Demo_1.Services
{
    public class MetadataService : IMetadataService
    {
        public async Task<string> GetHumanizedFileSize(StorageFile file)
        {
            BasicProperties basicProperties = await file.GetBasicPropertiesAsync();
            return new ByteSize(basicProperties.Size).Humanize(); //Chuyen doi du lieu tu "byte" sang "MB" hoac gi do cho de nhin
        }
        public DateTime? GetTakenDate(StorageFile file) 
        { 
            IReadOnlyList<Directory>? directories = ImageMetadataReader.ReadMetadata(file.Path); //Doc metadata tep, kieu tra ve ireadonly dam bao ketqua khong the bi thay doi, tra ve thu muc chua exif, iptc,....
            ExifSubIfdDirectory? directory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();//Lay ra thu muc co kieu exif chua cac thong tin nhu ngay, khau do, ..., lay phan tu dau tien, neu khon co tra ve
            if(directory.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out DateTime dateTime))
            {
                return dateTime;
            }
            return null;
        }
    }
}
