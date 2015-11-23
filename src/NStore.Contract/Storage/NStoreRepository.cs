using System.Threading.Tasks;
using NStore.Contract.Container;
using NStore.Contract.File;

namespace NStore.Contract.Storage
{
    public abstract class NStoreRepository
    {
        public abstract string RepositoryType { get; }
        public NStoreRepositorySettings Settings { get; set; }

        /// <summary>
        ///     Read a file from the NStore repository.
        /// </summary>
        /// <param name="fileDefinition"></param>
        /// <param name="containerDefinition"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract FileActionStatus TryRead(FileDefinition fileDefinition, ContainerDefinition containerDefinition,
            out byte[] data);

        /// <summary>
        ///     Save a file object into the NStore repository. If the target container doesn't exist, create it with the provided
        ///     options.
        /// </summary>
        /// <param name="fileDefinition"></param>
        /// <param name="containerDefinition"></param>
        /// <param name="fileCreateOptions"></param>
        /// <param name="containerCreateOptions"></param>
        /// <returns></returns>
        public abstract FileActionStatus Save(FileDefinition fileDefinition, ContainerDefinition containerDefinition,
            FileCreateOptions fileCreateOptions,
            ContainerCreateOptions containerCreateOptions);


        /// <summary>
        ///     Save a file object into the NStore repository asynchronously. If the target container doesn't exist, create it with
        ///     the provided options.
        /// </summary>
        /// <param name="fileDefinition"></param>
        /// <param name="containerDefinition"></param>
        /// <param name="fileCreateOptions"></param>
        /// <param name="containerCreateOptions"></param>
        /// <returns></returns>
        public abstract Task<FileActionStatus> SaveAsync(FileDefinition fileDefinition,
            ContainerDefinition containerDefinition,
            FileCreateOptions fileCreateOptions,
            ContainerCreateOptions containerCreateOptions);

        /// <summary>
        ///     Save a file object into the NStore repository. If the target container doesn't exist, create it with default
        ///     options.
        /// </summary>
        /// <param name="fileDefinition"></param>
        /// <param name="containerDefinition"></param>
        /// <returns></returns>
        public FileActionStatus Save(FileDefinition fileDefinition, ContainerDefinition containerDefinition)
        {
            return Save(fileDefinition, containerDefinition, Defaults<FileCreateOptions>.GetDefault(),
                Defaults<ContainerCreateOptions>.GetDefault());
        }

        /// <summary>
        ///     Save a file object into the NStore repository asynchronously. If the target container doesn't exist, create it with
        ///     default options.
        /// </summary>
        /// <param name="fileDefinition"></param>
        /// <param name="containerDefinition"></param>
        /// <returns></returns>
        public Task<FileActionStatus> SaveAsync(FileDefinition fileDefinition, ContainerDefinition containerDefinition)
        {
            return SaveAsync(fileDefinition, containerDefinition, Defaults<FileCreateOptions>.GetDefault(),
                Defaults<ContainerCreateOptions>.GetDefault());
        }

        /// <summary>
        ///     Save a file object into the NStore repository with default options. If the target container doesn't exist, create
        ///     it with default options.
        /// </summary>
        /// <param name="fileDefinition"></param>
        /// <param name="containerDefinition"></param>
        /// <param name="fileCreateOptions"></param>
        /// <returns></returns>
        public FileActionStatus Save(FileDefinition fileDefinition, ContainerDefinition containerDefinition,
            FileCreateOptions fileCreateOptions)
        {
            return Save(fileDefinition, containerDefinition, fileCreateOptions,
                Defaults<ContainerCreateOptions>.GetDefault());
        }

        /// <summary>
        ///     Save a file object into the NStore repository asynchronously with default options. If the target container doesn't
        ///     exist, create it with default options.
        /// </summary>
        /// <param name="fileDefinition"></param>
        /// <param name="containerDefinition"></param>
        /// <param name="fileCreateOptions"></param>
        /// <returns></returns>
        public Task<FileActionStatus> SaveAsync(FileDefinition fileDefinition, ContainerDefinition containerDefinition,
            FileCreateOptions fileCreateOptions)
        {
            return SaveAsync(fileDefinition, containerDefinition, fileCreateOptions,
                Defaults<ContainerCreateOptions>.GetDefault());
        }

        /// <summary>
        ///     Delete a file object from the NStore repository.
        /// </summary>
        /// <param name="fileDefinition"></param>
        /// <param name="containerDefinition"></param>
        /// <returns></returns>
        public abstract FileActionStatus Delete(FileDefinition fileDefinition, ContainerDefinition containerDefinition);

        /// <summary>
        ///     Move a file object in the NStore repository. Also used for rename.
        /// </summary>
        /// <param name="fileDefinition"></param>
        /// <param name="containerDefinition"></param>
        /// <param name="newContainerDefinition"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        public abstract FileActionStatus Move(FileDefinition fileDefinition, ContainerDefinition containerDefinition,
            ContainerDefinition newContainerDefinition, string newFileName = null);

        /// <summary>
        ///     Create a container in the NStore repository.
        /// </summary>
        /// <param name="containerDefinition"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public abstract ContainerActionStatus CreateContainer(ContainerDefinition containerDefinition,
            ContainerCreateOptions options);

        /// <summary>
        ///     Init method, called by factory method when creating the NStore repository.
        /// </summary>
        public abstract void Init();

        protected static class Constants
        {
            public static class Messages
            {
                public const string Success = "The operation was successful.";
                public const string NoAction = "The operation failed; no action was taken.";
                public const string Error = "An error occurred; no action was taken.";
                public const string FileDoesNotExist = "The file does not exist.";
            }
        }
    }
}