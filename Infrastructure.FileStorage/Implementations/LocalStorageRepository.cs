using Infrastructure.FileStorage.Entities;
using Infrastructure.FileStorage.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.FileStorage.Implementations
{
    public class LocalStorageRepository : IFileRepository
    {
        private readonly string _temporaryFolderName;
        private readonly string _storagePath;
        private readonly string _avaliabilityIdentifier;
        private readonly string _notsFolderName;
        public LocalStorageRepository(IConfigurationRoot configuration)
        {
            _storagePath = configuration["Storage:Local:StoragePath"];
            _avaliabilityIdentifier = configuration["Storage:Local:UserUnavaliableFolderMatch"];
            _temporaryFolderName = configuration["Storage:Local:TemporaryFolderName"];
            _notsFolderName = configuration["Storage:Local:NotesFiles"];

        }

        public Task<string> Add(Guid userId, Stream inputStream, string fileNameWithPath)
        {
            var newFilePath = $"{_storagePath}/{userId}/{fileNameWithPath}";
            using (var fileStream = File.Create(newFilePath))
            {
                inputStream.Seek(0, SeekOrigin.Begin);
                inputStream.CopyTo(fileStream);
            }
            return Task.FromResult($"/{fileNameWithPath}");
        }

        public Task CreateFolder(Guid userId, string path, string folderName, bool avaliableForUser)
        {
            var newFolderPath = Path(userId, path, folderName);
            Directory.CreateDirectory(newFolderPath);
            return Task.CompletedTask;
        }

        public Task DeleteFile(Guid userId, string fileNameWithPath)
        {
            var path = Path(userId, fileNameWithPath);
            File.Delete(path);
            return Task.CompletedTask;
        }

        public Task DeleteFolder(Guid userId, string path)
        {
            var newFolderPath = Path(userId, path);
            Directory.Delete(newFolderPath);
            return Task.CompletedTask;
        }

        public Task<Stream> GetFile(Guid userId, string fileNameWithPath)
        {
            var newFolderPath = Path(userId, fileNameWithPath);
            Stream fs = File.OpenRead(newFolderPath);

            return Task.FromResult(fs);
        }

        public Task<FolderMetadata> GetFolder(Guid userId, string path)
        {
            throw new NotImplementedException();
        }

        public async Task InitializeUserDataStorageAsync(Guid userId)
        {
            var newFolderPath = $"{_storagePath}/{userId}";
            Directory.CreateDirectory(newFolderPath);

            var temporaryDataFolder = Path(userId, $"{_avaliabilityIdentifier}{_temporaryFolderName}{_avaliabilityIdentifier}");
            Directory.CreateDirectory(temporaryDataFolder);

            var notesFolder = Path(userId, $"{_notsFolderName}");
            Directory.CreateDirectory(notesFolder);
        }

        private string Path(Guid userId, string path, string fileName = null)
        {
            if (fileName == null)
                return $"{_storagePath}/{userId}/{path}";
            else
                return $"{_storagePath}/{userId}/{path}/{fileName}";
        }

        public Task<FolderMetadata> GetUserTemporaryFolder(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
