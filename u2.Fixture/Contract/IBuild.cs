namespace u2.Fixture.Contract
{
    public interface IBuild<in T>
    {
        void Setup(T registry);
    }
}
