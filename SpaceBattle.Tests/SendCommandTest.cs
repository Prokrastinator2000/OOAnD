using Moq;
namespace SpaceBattle.Lib.Tests;
public class SendCommandTests
   {
      [Fact]
      public void SendCommand_ToReceiver()
      {
        var cmd = new Mock<ICommand>();
        var receiver = new Mock<ICommandReceiver>();
        var sendCommand = new SendCommand(cmd.Object, receiver.Object);

        sendCommand.Execute();

        receiver.Verify(x => x.Receive(cmd.Object), Times.Once);
      }

       [Fact]
      public void SendCommand_Exception()
      {
        
        var cmd = new Mock<ICommand>();
        var receiver = new Mock<ICommandReceiver>();
        receiver.Setup(r => r.Receive(It.IsAny<ICommand>())).Throws(new Exception());
        var sendCommand = new SendCommand(cmd.Object, receiver.Object);

        Assert.Throws<Exception>(() => sendCommand.Execute());

      }
   }
