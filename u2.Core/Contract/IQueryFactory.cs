namespace u2.Core.Contract
{
    public interface IQueryFactory
    {
        ICmsQuery<T> Create<T>(IMapTask<T> mapTask) where T : class, new();
    }
}
