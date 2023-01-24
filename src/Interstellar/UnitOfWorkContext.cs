namespace Interstellar
{
    using System.Threading;

    public static class UnitOfWorkContext
    {
        private static readonly AsyncLocal<UnitOfWork> Storage = new AsyncLocal<UnitOfWork>();

        public static UnitOfWork Current
        {
            get => Storage.Value;
            set => Storage.Value = value;
        }
    }
}