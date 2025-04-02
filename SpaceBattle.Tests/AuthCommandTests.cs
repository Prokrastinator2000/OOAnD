namespace SpaceBattle.Tests;
public class AuthCommandTests
{
    [Fact]
    public void Execute_ThrowsException_WhenUserDoesNotOwnAnyItem()
    {
        // Arrange
        var user1 = "123";
        IDictionary<string, IDictionary<string, object>> gameItem1 = new Dictionary<string, IDictionary<string, object>>()
            {
                {"1", new Dictionary<string, object>(){{"ShipName","Atlanta"}, {"OwnerId","124"}}},
                {"2", new Dictionary<string, object>(){{"ShipName","Moryak"}, {"OwnerId","125"}}}
            };
        var order = new AuthOrder(user1, gameItem1);
        var cmd = new AuthCommand(order);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => cmd.Execute());
        Assert.NotNull(exception);
    }

    [Fact]
    public void Execute_DoesNotThrow_WhenUserOwnsAtLeastOneItem()
    {
        // Arrange
        var user1 = "123";
        IDictionary<string, IDictionary<string, object>> gameItem1 = new Dictionary<string, IDictionary<string, object>>()
            {
                {"1", new Dictionary<string, object>(){{"ShipName","Atlanta"}, {"OwnerId","124"}}},
                {"2", new Dictionary<string, object>(){{"ShipName","Moryak"}, {"OwnerId","123"}}}
            };
        var order = new AuthOrder(user1, gameItem1);
        var cmd = new AuthCommand(order);

        // Act & Assert
        var exception = Record.Exception(() => cmd.Execute());
        Assert.Null(exception);
    }
    [Fact]
    public void Execute_ThrowsException_WhenOwnerIdIsMissing()
    {
        // Arrange
        var user1 = "123";
        IDictionary<string, IDictionary<string, object>> gameItem1 = new Dictionary<string, IDictionary<string, object>>()
    {
        {"1", new Dictionary<string, object>(){{"ShipName","Atlanta"}}},
        {"2", new Dictionary<string, object>(){{"ShipName","Moryak"}}}
    };
        var order = new AuthOrder(user1, gameItem1);
        var cmd = new AuthCommand(order);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => cmd.Execute());
        Assert.NotNull(exception);
    }
}
