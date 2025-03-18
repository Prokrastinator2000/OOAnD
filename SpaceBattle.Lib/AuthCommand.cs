using App;
public class AuthCommand : ICommand
{
    private readonly IAuthOrder _order;
    public AuthCommand(IAuthOrder order)
    {
        _order = order;
    }
    public void Execute()
    {
        var isMatchFound = false;

        foreach (var item in _order.GameItem)
        {
            var properties = item.Value;
            if (properties.ContainsKey("OwnerId") && properties["OwnerId"].ToString() == _order.UserId)
            {
                isMatchFound = true;
                break;
            }
        }

        if (!isMatchFound)
        {
            throw new InvalidOperationException();
        }
    }
}
