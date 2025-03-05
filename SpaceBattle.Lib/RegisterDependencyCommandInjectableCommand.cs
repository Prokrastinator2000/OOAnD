﻿using App;

namespace SpaceBattle.Lib;
public class RegisterDependencyCommandInjectableCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<ICommand>("IoC.Register", "Commands.CommandInjectable", (object[] args) => new InjectableCommand()).Execute();
    }
}
