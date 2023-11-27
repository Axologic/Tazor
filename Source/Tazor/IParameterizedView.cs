namespace Tazor;

public interface IParameterizedView<out T>
{
    static abstract IEnumerable<T> GetData();
}