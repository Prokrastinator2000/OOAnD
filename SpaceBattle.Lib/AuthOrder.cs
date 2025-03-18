public class AuthOrder : IAuthOrder
{
    public string UserId { get; }
    public IDictionary<string, IDictionary<string, object>> GameItem { get; }

    public AuthOrder(string userId, IDictionary<string, IDictionary<string, object>> gameItem)
    {
        UserId = userId;
        GameItem = gameItem;
    }
}
