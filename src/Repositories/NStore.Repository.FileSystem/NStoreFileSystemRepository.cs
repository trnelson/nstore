using System;
using System.IO;
using System.Linq;
using NStore.Contract;
using NStore.Contract.Container;
using NStore.Contract.File;
using NStore.Contract.Storage;

namespace NStore.Repository.FileSystem
{
    public class NStoreFileSystemRepository : NStoreRepository
    {
        private bool _removeEmptyPathsOnDelete;
        private bool _removeEmptyPathsOnMove;

        public override string RepositoryType
        {
            get { return "filesystem"; }
        }

        public override FileDefinition Read(FileDefinition fileDefinition, ContainerDefinition containerDefinition)
        {
            throw new NotImplementedException();
        }

        public override FileActionStatus Save(FileDefinition fileDefinition, ContainerDefinition containerDefinition,
            FileCreateOptions fileCreateOptions,
            ContainerCreateOptions containerCreateOptions)
        {
            var status = new FileActionStatus();

            // Create directory if it doesn't exist
            // TODO: Catch exceptions here
            if (!Directory.Exists(containerDefinition.Path)) Directory.CreateDirectory(containerDefinition.Path);

            var path = GetFullFilePath(fileDefinition, containerDefinition);

            // Overwrite based on provided option
            if (!fileCreateOptions.OverwriteExisting && File.Exists(path))
            {
                status.Status = Status.NoAction;
                status.Message = "File already exists; must explicitly set option to overwrite.";
                return status;
            }

            // Try to write file
            try
            {
                File.WriteAllBytes(path, fileDefinition.Data);
            }
            catch (Exception)
            {
                status.Status = Status.NoAction;
                status.Message = Constants.Messages.NoAction;
            }

            status.Status = Status.Success;
            status.Message = Constants.Messages.Success;
            return status;
        }

        public override FileActionStatus Delete(FileDefinition fileDefinition, ContainerDefinition containerDefinition)
        {
            var status = new FileActionStatus();
            var path = GetFullFilePath(fileDefinition, containerDefinition);

            // Fail early if file doesn't exist
            if (!File.Exists(path))
            {
                status.Status = Status.NoAction;
                status.Message = Constants.Messages.FileDoesNotExist;
                return status;
            }

            try
            {
                File.Delete(path);
                if (_removeEmptyPathsOnDelete) RemoveEmptyDirectories(containerDefinition.Path);
            }
            catch (Exception)
            {
                status.Status = Status.NoAction;
                status.Message = Constants.Messages.NoAction;
            }

            status.Status = Status.Success;
            status.Message = Constants.Messages.Success;
            return status;
        }

        public override FileActionStatus Move(FileDefinition fileDefinition, ContainerDefinition containerDefinition,
            ContainerDefinition newContainerDefinition, string newFileName = null)
        {
            var status = new FileActionStatus();
            var newFileDefinition = new FileDefinition {Name = newFileName ?? fileDefinition.Name};
            // Don't clone fileDefinition since we don't need the data here, just the name

            var oldPath = GetFullFilePath(fileDefinition, containerDefinition);
            var newPath = GetFullFilePath(newFileDefinition, newContainerDefinition);

            // Fail early if file doesn't exist
            if (!File.Exists(oldPath))
            {
                status.Status = Status.NoAction;
                status.Message = Constants.Messages.FileDoesNotExist;
                return status;
            }

            try
            {
                // TODO: Catch exceptions here
                if (!Directory.Exists(newContainerDefinition.Path))
                    Directory.CreateDirectory(newContainerDefinition.Path);
                File.Move(oldPath, newPath);
                if (_removeEmptyPathsOnMove) RemoveEmptyDirectories(containerDefinition.Path);
            }
            catch (Exception)
            {
                status.Status = Status.NoAction;
                status.Message = Constants.Messages.NoAction;
            }

            status.Status = Status.Success;
            status.Message = Constants.Messages.Success;
            return status;
        }

        public override ContainerActionStatus CreateContainer(ContainerDefinition containerDefinition,
            ContainerCreateOptions options)
        {
            throw new NotImplementedException("Do not use CreateContainer for file system storage.");
        }

        public override void Init()
        {
            _removeEmptyPathsOnDelete = Settings.Setting<bool>("removeEmptyPathsOnDelete");
            _removeEmptyPathsOnMove = Settings.Setting<bool>("removeEmptyPathsOnMove");
        }

        private string GetFullFilePath(FileDefinition fileDefinition, ContainerDefinition containerDefinition)
        {
            return Path.Combine(containerDefinition.Path, fileDefinition.Name);
        }

        private void RemoveEmptyDirectories(string path)
        {
            var di = new DirectoryInfo(path);
            while (!di.EnumerateFiles().Any() && !di.EnumerateDirectories().Any())
            {
                di.Delete();
                di = di.Parent;
            }
        }
    }
}