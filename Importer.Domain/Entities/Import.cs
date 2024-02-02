namespace Importer.Domain.Entities
{
    public class Import
    {
        public Guid Id { get; private set; }
        public string FileName { get; private set; }
        public DateTime ImportedIn { get; private set; }
        public DateTime? ProcessedAt { get; private set; }
        public bool IsSuccessful => string.IsNullOrEmpty(ErrorMessage) && ProcessedAt.HasValue;
        public string ErrorMessage { get; private set; }
        public byte[] FileContent { get; private set; }

        public Guid UserId { get; private set; }

        public Import(string fileName, byte[] fileContent, Guid userId)
        {
            Id = Guid.NewGuid();
            FileName = fileName;
            ImportedIn = DateTime.UtcNow;
            FileContent = fileContent;
            UserId = userId;

            ErrorMessage = string.Empty;
        }

        public Import()
        {
            
        }

        public void ProcessImport() => ProcessedAt = DateTime.UtcNow;

        public void ImportError(string errorMessage) => ErrorMessage = errorMessage;

        public void Reprocess()
        {
            ProcessedAt = null;
            ErrorMessage = string.Empty;
        }
    }
}
