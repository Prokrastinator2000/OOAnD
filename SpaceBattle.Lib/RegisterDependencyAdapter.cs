using App;
namespace SpaceBattle.Lib
{
    public class RegisterDependencyAdapter
    {
        public void Execute()
        {
            // Стратегия для создания адаптера объекта
            Ioc.Resolve<ICommand>("IoC.Register", "Game.Adapter", (object[] args) =>
            {
                var interfaceType = (Type)args[0];
                var obj = (IDictionary<string, object>)args[1];

                // Регистрируем необходимые типы для фабрики
                Ioc.Resolve<ICommand>("IoC.Register", "Game.Adapter.InterfaceType",
                    (object[] _) => interfaceType).Execute();

                Ioc.Resolve<ICommand>("IoC.Register", "Game.Adapter.TypeName",
                    (object[] _) => $"{interfaceType.Name}Adapter").Execute();

                // Создаем адаптер через фабрику
                var adapterFactory = new AdapterFactory();
                return adapterFactory.Create(obj);
            }).Execute();

        }
    }
}
