using VyshKat.App.Services;
using VyshKat.App.Domain;

namespace VyshKat.App.UI;

/// <summary>
/// Слой представления: Консольный интерфейс
/// </summary>
public class ConsoleUserInterface
{
    private readonly IFleetManager _fleetManager;
    private int _selectedIndex = 0;
    private readonly string[] _menuOptions =
    {
            "1. Принять электросамокат",
            "2. Принять механический велосипед",
            "3. Принять защитный шлем",
            "4. Принять зарядную станцию",
            "5. Показать суммарное энергопотребление",
            "6. Показать транспорт для новичков",
            "7. Показать весь инвентарь",
            "8. Выход"
        };

    public ConsoleUserInterface(IFleetManager fleetManager)
    {
        _fleetManager = fleetManager;
    }

    public void Run()
    {
        // добавляем тестовые данные
        FakeData();

        while (true)
        {
            RenderMenu();
            var keyInfo = Console.ReadKey(intercept: true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    _selectedIndex = (_selectedIndex - 1 + _menuOptions.Length) % _menuOptions.Length;
                    break;
                case ConsoleKey.DownArrow:
                    _selectedIndex = (_selectedIndex + 1) % _menuOptions.Length;
                    break;
                case ConsoleKey.Enter:
                    if (_selectedIndex == _menuOptions.Length - 1)
                    {
                        Console.Clear();
                        Console.WriteLine("Завершение работы");
                        return; // выход из приложения
                    }
                    ExecuteAction(_selectedIndex);
                    Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
                    Console.ReadKey(intercept: true);
                    break;
            }
        }
    }


    private void RenderMenu()
    {
        Console.Clear();
        Console.WriteLine("Сервис проката ВышКат");
        Console.WriteLine("Навигация: [ВВЕРХ] [ВНИЗ] | Выбор: [ENTER]\n");

        for (int i = 0; i < _menuOptions.Length; i++)
        {
            if (i == _selectedIndex)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($">> {_menuOptions[i]}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"   {_menuOptions[i]}");
            }
        }
    }


    private void ExecuteAction(int actionIndex)
    {
        Console.Clear();
        try
        {
            switch (actionIndex)
            {
                case 0: // самокат
                    Console.Write("Введите инвентарный номер: ");
                    string sId = Console.ReadLine() ?? "S-000";
                    Console.Write("Введите уровень простоты (1-10): ");
                    int sSimplicity = int.Parse(Console.ReadLine() ?? "1");
                    Console.Write("Введите суточное энергопотребление (кВт*ч): ");
                    double sEnergy = double.Parse(Console.ReadLine() ?? "0");

                    var scooter = new Scooter(sId, sSimplicity, sEnergy);
                    bool isAccepted = _fleetManager.AcceptVehicle(scooter);
                    Console.WriteLine(isAccepted
                        ? "Электросамокат успешно прошел техосмотр и добавлен в парк."
                        : "Отказ: Осмотр не пройден.");
                    break;

                case 1: // велосипед
                    Console.Write("Введите инвентарный номер: ");
                    string bId = Console.ReadLine() ?? "B-000";
                    Console.Write("Введите уровень простоты (1-10): ");
                    int bSimplicity = int.Parse(Console.ReadLine() ?? "1");

                    var bicycle = new Bicycle(bId, bSimplicity);
                    bool bAccepted = _fleetManager.AcceptVehicle(bicycle);
                    Console.WriteLine(bAccepted
                        ? "Велосипед успешно прошел техосмотр и добавлен в парк."
                        : "Отказ: Осмотр не пройден.");
                    break;

                case 2: // шлем
                    Console.Write("Введите инвентарный номер шлема: ");
                    string hId = Console.ReadLine() ?? "H-000";
                    bool helmetAccepted = _fleetManager.AcceptThing(new Helmet(hId));
                    Console.WriteLine(helmetAccepted
                        ? "Шлем добавлен на баланс (техосмотр для вещей не требуется)."
                        : "Отказ: Осмотр не пройден.");
                    break;

                case 3: // зарядная станция
                    Console.Write("Введите инвентарный номер станции: ");
                    string cId = Console.ReadLine() ?? "CS-000";
                    Console.Write("Введите суточное энергопотребление (кВт*ч): ");
                    double cEnergy = double.Parse(Console.ReadLine() ?? "0");
                    bool stationAccepted = _fleetManager.AcceptThing(new ChargingStation(cId, cEnergy));
                    Console.WriteLine(stationAccepted
                        ? "Зарядная станция добавлена на баланс."
                        : "Отказ: Осмотр не пройден.");
                    break;

                case 4: // энергопотребление
                    double totalEnergy = _fleetManager.GetTotalEnergyConsumption();
                    Console.WriteLine($"Суммарное суточное потребление энергии парком: {totalEnergy:F2} кВт*ч");
                    break;

                case 5: // для новичков
                    Console.WriteLine("Транспортные средства, разрешенные для новичков (простота >= 6):");
                    var beginners = _fleetManager.GetBeginnerFriendlyVehicles().ToList();
                    if (!beginners.Any()) Console.WriteLine("Нет подходящего транспорта");

                    foreach (var v in beginners)
                    {
                        Console.WriteLine($"- [{v.InventoryNumber}] {v.GetType().Name} (Простота: {v.Simplicity})");
                    }
                    break;

                case 6: // инвентаризация
                    Console.WriteLine("Полный список имущества на балансе:");
                    Console.WriteLine(new string('-', 50));
                    Console.WriteLine($"| {"Инв. Номер",-15} | {"Тип / Наименование",-28} |");
                    Console.WriteLine(new string('-', 50));

                    foreach (var item in _fleetManager.GetAllInventoryItems())
                    {
                        string typeName = item is Thing t ? t.Name : item.GetType().Name;
                        Console.WriteLine($"| {item.InventoryNumber,-15} | {typeName,-28} |");
                    }
                    Console.WriteLine(new string('-', 50));
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ошибка ввода: {ex.Message}");
            Console.ResetColor();
        }
    }

    private void FakeData()
    {
        _fleetManager.AcceptVehicle(new Scooter("SCOOT-01", 8, 2.5));
        _fleetManager.AcceptVehicle(new Scooter("SCOOT-02", 4, 3.0));
        _fleetManager.AcceptVehicle(new Bicycle("BIKE-01", 9));
        _fleetManager.AcceptThing(new Helmet("HELM-01"));
        _fleetManager.AcceptThing(new ChargingStation("CHARGE-MAIN", 15.0));
    }
}