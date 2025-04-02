public interface IAuthOrder
{
    string UserId { get; }
    IDictionary<string, IDictionary<string, object>> GameItem { get; }
}
