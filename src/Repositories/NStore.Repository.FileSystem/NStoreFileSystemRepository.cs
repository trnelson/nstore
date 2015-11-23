using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NStore.Contract;
using NStore.Contract.Container;
using NStore.Contract.File;
using NStore.Contract.Storage;

namespace NStore.Repository.FileSystem
{
    public class NStoreFileSystemRepository : NStoreRepository
    {
        private bool _createPathsAutomatically;
        private bool _removeEmptyPathsOnDelete;
        private bool _removeEmptyPathsOnMove;

        public override string RepositoryType
        {
            get { return "filesystem"; }
        }

        public override FileActionStatus TryRead(FileDefinition fileDefinition, ContainerDefinition containerDefinition,
            out byte[] data)
        {
            var status = new FileActionStatus();
            var path = GetFullFilePath(fileDefinition, containerDefinition);
            try
            {
                data = File.ReadAllBytes(path);
                status.Status = Status.Success;
                status.Message = Constants.Messages.Success;
            }
            catch (Exception)
            {
                data = null;
                status.Status = Status.NoAction;
                status.Message = Constants.Messages.NoAction;
            }
            return status;
        }

        public override FileActionStatus Save(FileDefinition fileDefinition, ContainerDefinition containerDefinition,
            FileCreateOptions fileCreateOptions,
            ContainerCreateOptions containerCreateOptions)
        {
            var status = new FileActionStatus();

            // If we aren't allowing auto creation of directories, fail
            if (!Directory.Exists(containerDefinition.Path) && !_createPathsAutomatically)
            {
                status.Status = Status.NoAction;
                status.Message = Constants.Messages.NoAction +
                                 " Path does not exist, and config to create paths automatically is false.";
                return status;
            }

            // Create directory if it doesn't exist
            Directory.CreateDirectory(containerDefinition.Path);

            var path = GetFullFilePath(fileDefinition, containerDefinition);

            // Overwrite based on provided option
            if (!fileCreateOptions.OverwriteExisting && File.Exists(path))
            {
                status.Status = Status.NoAction;
                status.Message = Constants.Messages.NoAction +
                                 " File already exists; must explicitly set option to overwrite.";
                return status;
            }

            SaveFile(path, fileDefinition.Data).Wait();

            status.Status = Status.Success;
            status.Message = Constants.Messages.Success;
            return status;
        }

        public override async Task<FileActionStatus> SaveAsync(FileDefinition fileDefinition,
            ContainerDefinition containerDefinition,
            FileCreateOptions fileCreateOptions, ContainerCreateOptions containerCreateOptions)
        {
            // TODO: Remove some of this duplicated code

            var status = new FileActionStatus();

            // If we aren't allowing auto creation of directories, fail
            if (!Directory.Exists(containerDefinition.Path) && !_createPathsAutomatically)
            {
                status.Status = Status.NoAction;
                status.Message = Constants.Messages.NoAction +
                                 " Path does not exist, and config to create paths automatically is false.";
                return status;
            }

            // Create directory if it doesn't exist
            Directory.CreateDirectory(containerDefinition.Path);

            var path = GetFullFilePath(fileDefinition, containerDefinition);

            // Overwrite based on provided option
            if (!fileCreateOptions.OverwriteExisting && File.Exists(path))
            {
                status.Status = Status.NoAction;
                status.Message = Constants.Messages.NoAction +
                                 " File already exists; must explicitly set option to overwrite.";
                return status;
            }

            var result = await SaveFile(path, fileDefinition.Data);
            status.Status = result.Status;
            status.Message = result.Message;
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
                // If we aren't allowing auto creation of directories, fail
                if (!Directory.Exists(newContainerDefinition.Path) && !_createPathsAutomatically)
                {
                    status.Status = Status.NoAction;
                    status.Message = Constants.Messages.NoAction +
                                     " Destination path does not exist, and config to create paths automatically is false.";
                    return status;
                }
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
            _createPathsAutomatically = Settings.Setting<bool>("createPathsAutomatically");
            _removeEmptyPathsOnDelete = Settings.Setting<bool>("removeEmptyPathsOnDelete");
            _removeEmptyPathsOnMove = Settings.Setting<bool>("removeEmptyPathsOnMove");
        }

        # region Private Stuff

        private async Task<FileActionStatus> SaveFile(string path, byte[] data)
        {
            var status = new FileActionStatus();
            try
            {
                using (var sourceStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await sourceStream.WriteAsync(data, 0, data.Length);
                }
                ;
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

        # endregion
    }
}