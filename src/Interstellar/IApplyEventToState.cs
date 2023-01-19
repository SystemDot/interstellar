namespace Interstellar
{
    public interface IApplyEventToState<in TEvent>
    {
        void Apply(TEvent toApply);
    }
}