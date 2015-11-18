namespace NStore.Contract.Container
{
    /// <summary>
    ///     A structure representing a container (or drive) in the NStore repository.
    /// </summary>
    public class ContainerDefinition
    {
        public ContainerDefinition()
        {
        }

        /// <summary>
        ///     Constructor used for cloning
        /// </summary>
        /// <param name="containerDefinition"></param>
        public ContainerDefinition(ContainerDefinition containerDefinition)
        {
            Name = containerDefinition.Name;
            Path = containerDefinition.Path;
        }

        public string Name { get; set; }
        public string Path { get; set; }
    }
}