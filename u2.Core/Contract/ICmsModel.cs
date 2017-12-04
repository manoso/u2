namespace u2.Core.Contract
{
    public interface ICmsModel<out T> : ICmsKey
    {
        T Id { get; }
        string Name { get; }
    }
}