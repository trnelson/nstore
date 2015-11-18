using System;
using System.IO;
using NStore.Contract;
using NStore.Contract.Container;
using NStore.Contract.File;
using NStore.Contract.Storage;
using NStore.Repository.FileSystem;

namespace NStore.Example.Examples
{
    internal class AdvancedExample
    {
        private readonly NStoreRepository _fileSystemRepository;

        public AdvancedExample()
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

            // File and container options
            var fileCreateOptions = Defaults<FileCreateOptions>.GetDefault();
            fileCreateOptions.OverwriteExisting = true;
            var containerCreateOptions = Defaults<ContainerCreateOptions>.GetDefault();
            containerCreateOptions.MakeFilesPublic = true;

            // Save file
            _fileSystemRepository.Save(file, container);
            Console.WriteLine("Saving file \"{0}\" in {1}", file.Name, container.Path);

            // Let's move it too by specifying a new container (a clone of our container, actually) and a new name for the file
            var newContainer = new ContainerDefinition(container) {Path = "C:/temp/nstore-test2/photos"};
            var newFileName = "output-new.jpg";
            _fileSystemRepository.Move(file, container, newContainer, newFileName);
            Console.WriteLine("Moving file \"{0}\" to {1} with new name \"{2}\"", file.Name, newContainer.Path, newFileName);
        }
    }
}
