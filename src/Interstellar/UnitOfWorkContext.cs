namespace Interstellar
{
    public static class UnitOfWorkContext
    {
        public static UnitOfWork? Current { get; internal set; }
    }
}