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
        if (!_order.GameItem.Values.Any(properties => properties.ContainsKey("OwnerId") && properties["OwnerId"].ToString() == _order.UserId))
        {
            throw new InvalidOperationException();
        }
    }
}
