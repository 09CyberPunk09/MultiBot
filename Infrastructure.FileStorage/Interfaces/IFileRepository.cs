using Infrastructure.FileStorage.Entities;

namespace Infrastructure.FileStorage.Interfaces
{
    public interface IFileRepository
    {
        Task InitializeUserDataStorageAsync(Guid userId);
        Task<string> Add(Guid userId, Stream inputStream, string fileNameWithPath);
        Task DeleteFile(Guid userId, string fileNameWithPath);
        Task<Stream> GetFile(Guid userId, string fileNameWithPath);
        Task CreateFolder(Guid userId, string path, string folderName, bool avaliableForUser);
        Task DeleteFolder(Guid userId, string path);
        Task<FolderMetadata> GetFolder(Guid userId, string path);
        Task<FolderMetadata> GetUserTemporaryFolder(Guid userId);
    }
}
