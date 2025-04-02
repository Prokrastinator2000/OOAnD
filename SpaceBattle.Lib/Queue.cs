namespace SpaceBattle.Lib;
using App;

public interface IQueue
{
    ICommand Take();
    void Add(ICommand command);
}
