namespace Interstellar
{
    using System.Threading.Tasks;

    public interface IWhenOptions
    {
        Task HandleAsync(object toHandle);
    }
}