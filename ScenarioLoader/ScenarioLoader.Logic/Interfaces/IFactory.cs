namespace ScenarioLoader.Logic.Interfaces
{
    public interface IFactory
    {
        T Resolve<T>();
    }
}
