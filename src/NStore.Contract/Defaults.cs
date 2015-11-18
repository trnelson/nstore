namespace NStore.Contract
{
    public static class Defaults<T> where T : IDefault<T>, new()
    {
        public static T GetDefault()
        {
            return new T().GetDefault();
        }
    }
}