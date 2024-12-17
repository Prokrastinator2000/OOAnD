﻿using App;
using Moq;
namespace SpaceBattle.Lib.Tests;

public class Command1 : ICommand
{
    public virtual void Execute()
    {
        Console.WriteLine("Command1 executed.");
    }
}

public class Command2 : ICommand
{
    public virtual void Execute()
    {
        Console.WriteLine("Command2 executed.");
    }
}

public class CommandRotate : ICommand
{
    public virtual void Execute()
    {
        Console.WriteLine("Command.Rotate executed.");
    }
}
public class CommandMove : ICommand
{
    public virtual void Execute()
    {
        Console.WriteLine("Command.Move executed.");
    }
}
public class SpecsMacroCommandTests
{
    [Fact]
    public void Test_CommandsExecution()
    {
        var cmd1 = new Command1();
        var cmd2 = new Command2();
        var cmd3 = new CommandRotate();
        var cmd4 = new CommandMove();

        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        cmd1.Execute();
        var output = stringWriter.ToString().Trim();
        Assert.Equal("Command1 executed.", output);
        stringWriter.GetStringBuilder().Clear();

        cmd2.Execute();
        output = stringWriter.ToString().Trim();
        Assert.Equal("Command2 executed.", output);
        stringWriter.GetStringBuilder().Clear();

        cmd3.Execute();
        output = stringWriter.ToString().Trim();
        Assert.Equal("Command.Rotate executed.", output);
        stringWriter.GetStringBuilder().Clear();

        cmd4.Execute();
        output = stringWriter.ToString().Trim();
        Assert.Equal("Command.Move executed.", output);
    }

    [Fact]
    public void Test_Resolve_MacroCommand_Success()
    {
        IEnumerable<string> CmdNames = new List<string> { "Command1", "Command2", "Command.Move", "Commands.Rotate" };

        var cmd1 = new Mock<Command1>();
        var cmd2 = new Mock<Command2>();
        var cmd3 = new Mock<CommandMove>();
        var cmd4 = new Mock<CommandRotate>();

        Ioc.Resolve<ICommand>("IoC.Register",
                            "Command1",
                            (object[] args) => cmd1.Object).Execute();
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Command2",
                            (object[] args) => cmd2.Object).Execute();
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Commands.Move",
                            (object[] args) => cmd3.Object).Execute();
        Ioc.Resolve<ICommand>("IoC.Register",
                            "Commands.Rotate",
                            (object[] args) => cmd4.Object).Execute();

        var strategy = new CreateMacroCommandStrategy("Macro.Test");
        var macroCommand = strategy.Resolve(new object[0]);
        macroCommand.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Once);
        cmd3.Verify(c => c.Execute(), Times.Once);
        cmd4.Verify(c => c.Execute(), Times.Once);

        strategy = new CreateMacroCommandStrategy("Specs.Move");
        macroCommand = strategy.Resolve(new object[0]);
        macroCommand.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Once);
        cmd3.Verify(c => c.Execute(), Times.Exactly(2));
        cmd4.Verify(c => c.Execute(), Times.Once);

        strategy = new CreateMacroCommandStrategy("Specs.Rotate");
        macroCommand = strategy.Resolve(new object[0]);
        macroCommand.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Once);
        cmd3.Verify(c => c.Execute(), Times.Exactly(2));
        cmd4.Verify(c => c.Execute(), Times.Exactly(2));
    }

    [Fact]
    public void Test_Resolve_MacroCommand_Failure()
    {
        var strategy = new CreateMacroCommandStrategy("Macro.Extended");

        Assert.Throws<InvalidOperationException>(() => strategy.Resolve(new object[0]));
    }

    [Fact]
    public void Test_Resolve_MacroCommand_UnknownStrategy()
    {
        var strategy = new CreateMacroCommandStrategy("Macro.Ex");

        Assert.Throws<InvalidOperationException>(() => strategy.Resolve(new object[0]));
    }
}
