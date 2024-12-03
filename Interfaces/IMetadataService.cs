

using System;
using System.Threading.Tasks;
using Windows.Storage;

#nullable enable
namespace App_Demo_1.Interfaces
{

    public interface IMetadataService
    {
        Task<string?> GetHumanizedFileSize(StorageFile file);
        DateTime? GetTakenDate(StorageFile file);
    }
}
