namespace SpaceBattle;
public interface IAdapterFactory
{
    object Create(IDictionary<string, object> obj);
}
