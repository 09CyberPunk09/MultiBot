using Common.Entites;
using Common.Enums;
using Infrastructure.FileStorage.Implementations;
using Infrastructure.FileStorage.Interfaces;
using NETCore.Encrypt;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services.Files
{
    public class FileStorageService : AppService
    {
        private readonly LocalStorageRepository _localStorageRepository;
        public FileStorageService(LocalStorageRepository localStorageRepository)
        {
            _localStorageRepository = localStorageRepository;
        }
        
        private IFileRepository GetRepository(FileStorageStrategy strategy)
        {
            return strategy switch
            {
                FileStorageStrategy.Local => _localStorageRepository,
                FileStorageStrategy.Google => throw new NotImplementedException()
            };
        }

        public Task<Stream> GetFileAsync(Guid userId, string pathToFile, FileStorageStrategy strategy)
        {
            var repository = GetRepository(strategy);
            return repository.GetFile(userId, pathToFile);
        }

        public async Task<FileDto> GetFileByHashAsync(Guid userId,string hash)
        {
            var decodedString = hash;
            var payload = JsonConvert.DeserializeObject<StorageItemSecurePayload>(decodedString);
            var stream = await GetFileAsync(userId, payload.Path, payload.Mode);

            return new()
            {
                 FilePath = payload.Path,
                 Stream = stream
            };
        }

        //TODO: IMPLEMENT ENCRYPTION ALGORYTHM
        public async Task<string> UploadFileAsync(Guid userId, Stream input, string fileNameWithPath, FileStorageStrategy strategy = FileStorageStrategy.Local)
        {
            var path = await UploadFileWithStrategyAsunc(userId, input, fileNameWithPath, strategy);
            var json = JsonConvert.SerializeObject(new StorageItemSecurePayload()
            {
                Mode = strategy,
                IsFile = true,
                Path = path
            });
            var encrypted = json;
            return encrypted;
        }

        private async Task<string> UploadFileWithStrategyAsunc(Guid userId, Stream input,string fileNameWithPath, FileStorageStrategy strategy)
        {
            var repository = GetRepository(strategy);
            string pathToFile = await repository.Add(userId, input, fileNameWithPath);
      
            return pathToFile;
        }

        public Task InitializeUserDataStorage(FileStorageStrategy fileStorageStrategy, Guid userId)
        {
            var repository = GetRepository(fileStorageStrategy);
            return repository.InitializeUserDataStorageAsync(userId);
        }
    }
}
