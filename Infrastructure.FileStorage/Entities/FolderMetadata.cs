namespace Infrastructure.FileStorage.Entities
{
    public class FolderMetadata
    {
        public string Name { get; set; }
        public List<string> Folders { get; set; }
        public List<string> Files { get; set; }
    }
}
