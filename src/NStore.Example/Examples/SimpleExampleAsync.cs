using System;
using System.IO;
using NStore.Contract.Container;
using NStore.Contract.File;
using NStore.Contract.Storage;
using NStore.Repository.FileSystem;

namespace NStore.Example.Examples
{
    internal class SimpleExampleAsync
    {
        private readonly NStoreRepository _fileSystemRepository;

        public SimpleExampleAsync()
        {
            _fileSystemRepository = NStore.GetRepository<NStoreFileSystemRepository>();
        }

        public void ExecuteSaveExample()
        {
            // Get some data
            var data = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "example-files/mountain-fog.jpg");

            // Create definitions
            var file = new FileDefinition {Name = "output.jpg", Data = data};
            var container = new ContainerDefinition {Path = @"C:/temp/nstore-test/photos"};

            // Save file asynchronously
            var t = _fileSystemRepository.SaveAsync(file, container);
            Console.WriteLine("Attempting to save file \"{0}\" in {1}", file.Name, container.Path);

            // Wait for the async operation to complete
            t.Wait();

            // Done!
            Console.WriteLine(t.Result.Message);
        }
    }
}