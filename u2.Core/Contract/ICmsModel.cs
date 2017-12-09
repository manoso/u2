namespace u2.Core.Contract
{
    /// <summary>
    /// A base type CMS model can inherit from to have and id and name.
    /// </summary>
    /// <typeparam name="T">Type of the id.</typeparam>
    public interface ICmsModel<out T> : ICmsKey
    {
        T Id { get; }
        string Name { get; }
    }
}