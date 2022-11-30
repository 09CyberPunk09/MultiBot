using Common.Enums;

namespace Common.Entites
{
    public class StorageItemSecurePayload
    {
        public FileStorageStrategy Mode { get; set; }
        public string Path { get; set; }
        public bool IsFile { get; set; } = true;
    }
}
