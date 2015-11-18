namespace NStore.Contract.File
{
    public class FileCreateOptions : IDefault<FileCreateOptions>
    {
        public bool OverwriteExisting { get; set; }

        public FileCreateOptions GetDefault()
        {
            return new FileCreateOptions {OverwriteExisting = false};
        }
    }
}