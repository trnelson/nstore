using System;
using System.IO;
using NStore.Contract.Container;
using NStore.Contract.File;
using NStore.Contract.Storage;
using NStore.Repository.FileSystem;

namespace NStore.Example.Examples
{
    internal class SimpleExample
    {
        private readonly NStoreRepository _fileSystemRepository;

        public SimpleExample()
        {
            _fileSystemRepository = NStore.GetRepository<NStoreFileSystemRepository>();
        }

        public void ExecuteTryReadExample()
        {
            // Create definitions
            var file = new FileDefinition {Name = "output.jpg"};
            var container = new ContainerDefinition {Path = @"C:/temp/nstore-test/photos"};

            // Save file
            byte[] output;
            var status = _fileSystemRepository.TryRead(file, container, out output);

            // Done!
            Console.WriteLine("Attempting to read file \"{0}\" in {1}", file.Name, container.Path);
            Console.WriteLine("Read file from repository, total {0} KB", output.LongLength/1024);
            Console.WriteLine(status.Message);
        }

        public void ExecuteSaveExample()
        {
            // Get some data
            var data = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "example-files/mountain-fog.jpg");

            // Create definitions
            var file = new FileDefinition {Name = "output.jpg", Data = data};
            var container = new ContainerDefinition {Path = @"C:/temp/nstore-test/photos"};

            // Save file
            var status = _fileSystemRepository.Save(file, container);

            // Done!
            Console.WriteLine("Attempting to save file \"{0}\" in {1}", file.Name, container.Path);
            Console.WriteLine(status.Message);
        }
    }
}