
using App;

namespace SpaceBattle.Lib
{
    public class RegisterDependencyAdapterRegister
    {
        public void Execute()
        {
            // Стратегия для создания адаптера объекта

            // Стратегия для регистрации адаптера интерфейса
            Ioc.Resolve<ICommand>("IoC.Register", "Game.Adapter.Register", (object[] args) =>
            {
                var interfaceType = (Type)args[0];
                var targetType = (Type)args[1];

                // Создаем builder и генерируем код адаптера
                var builder = new AdapterBuilder(interfaceType, targetType);

                foreach (var property in interfaceType.GetProperties())
                {
                    builder.CreateProperty(property);
                }

                return builder.Build();
            }).Execute();
        }
    }
}
