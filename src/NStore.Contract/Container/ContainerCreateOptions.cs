namespace NStore.Contract.Container
{
    public class ContainerCreateOptions : IDefault<ContainerCreateOptions>
    {
        public bool MakeContainerPublic { get; set; }
        public bool MakeFilesPublic { get; set; }

        public ContainerCreateOptions GetDefault()
        {
            return new ContainerCreateOptions {MakeContainerPublic = false, MakeFilesPublic = false};
        }
    }
}