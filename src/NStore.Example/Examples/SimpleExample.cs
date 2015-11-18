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

        public void Go()
        {
            // Get some data
            var data = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "example-files/mountain-fog.jpg");

            // Create definitions
            var file = new FileDefinition { Name = "output.jpg", Data = data };
            var container = new ContainerDefinition { Path = @"C:/temp/nstore-test/photos" };

            // Save file
            _fileSystemRepository.Save(file, container);

            // Done!
            Console.WriteLine("Saving file \"{0}\" in {1}", file.Name, container.Path);
        }
    }
}
