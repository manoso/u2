namespace u2.Core
{
    public interface IModel<out T> : IKey
    {
        T Id { get; }
    }

    public interface IKey
    {
        string GetKey();
    }
}