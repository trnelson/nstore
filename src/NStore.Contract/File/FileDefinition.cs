namespace NStore.Contract.File
{
    /// <summary>
    ///     A structure representing a file in the NStore repository.
    /// </summary>
    public class FileDefinition
    {
        public FileDefinition()
        {
        }

        /// <summary>
        ///     Constructor used for cloning
        /// </summary>
        /// <param name="fileDefinition"></param>
        public FileDefinition(FileDefinition fileDefinition)
        {
            Name = fileDefinition.Name;
            Data = fileDefinition.Data;
        }

        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}