using Common.Enums;

namespace LifeTracker.FileStorage.Models
{
    public class UploadFilesModel
    {
        public int? AbleFor { get; set; }
        public FileStorageStrategy? PreferableStorage { get; set; }
        public Guid UserId { get; set; }
        public Guid? FolderId { get; set; }
        public IList<IFormFile> Files { get; set; }

    }
}
