using Microsoft.Extensions.Configuration;
using System.IO;

namespace Application.Services.FileStorage
{
    //TODO: Move to helper
    public class StoragePathService : AppService
    {
        private readonly string _temporaryFolderName;
        private readonly string _avaliabilityIdentifier;
        private readonly string _notsFolderName;
        public StoragePathService(IConfigurationRoot configuration)
        {
            _avaliabilityIdentifier = configuration["Storage:Local:UserUnavaliableFolderMatch"];
            _temporaryFolderName = configuration["Storage:Local:TemporaryFolderName"];
            _notsFolderName = configuration["Storage:Local:NotesFiles"];
        }

        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public string GetNotesFolderName()
        {
            return _notsFolderName;
        }

        public string[] GetPhotoExtensions()
        {
            return new[] { "jpg", "jpeg", "png" };
        }

        public string GetTemporaryPath()
        {
            var temporaryDataFolder = $"{_avaliabilityIdentifier}{_temporaryFolderName}{_avaliabilityIdentifier}";
            return temporaryDataFolder;
        }
    }
}
