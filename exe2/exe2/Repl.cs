using exe2.equipment;
using exe2.user;

namespace exe2;

public class Repl
{
    private readonly IManager _manager;
    private readonly Dictionary<string, (string Description, Action Handler)> _commands;
    private readonly IdTracker _idTracker = new();

    public Repl(IManager manager)
    {
        _manager = manager;
        _commands = new Dictionary<string, (string Description, Action Handler)>
        {
            ["1"]  = ("Add user",                       HandleAddUser),
            ["2"]  = ("Add equipment",                  HandleAddEquipment),
            ["3"]  = ("List all equipment with status", HandleListAllEquipment),
            ["4"]  = ("List available equipment",       HandleListAvailableEquipment),
            ["5"]  = ("Rent equipment",                 HandleRentEquipment),
            ["6"]  = ("Return equipment",               HandleReturnEquipment),
            ["7"]  = ("Mark equipment unavailable",     HandleMarkUnavailable),
            ["8"]  = ("Show active rentals for user",   HandleUserActiveRentals),
            ["9"]  = ("Show overdue rentals",           HandleOverdueRentals),
            ["10"] = ("Summary report",                 HandleSummaryReport),
            ["0"]  = ("Exit",                           () => Environment.Exit(0)),
        };
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Rental System ===");
            foreach (var (key, (desc, _)) in _commands)
                Console.WriteLine($"  {key,2}. {desc}");
            Console.Write("> ");

            var input = Console.ReadLine()?.Trim();
            if (input is null) break;

            if (!_commands.TryGetValue(input, out var command))
            {
                Console.WriteLine("Unknown command.");
                continue;
            }

            try
            {
                command.Handler();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private void HandleAddUser()
    {
        Console.Write("Type (student/employee): ");
        var type = Console.ReadLine()?.Trim().ToLower();

        Console.Write("Full name: ");
        var name = Console.ReadLine()!;

        var id = _idTracker.NextId();

        Console.Write("Balance: ");
        var balance = ReadInt();

        User user = type switch
        {
            "student"  => new Student(name, id, balance),
            "employee" => new Employee(name, id, balance),
            _ => throw new InvalidOperationException($"Unknown user type '{type}'.")
        };

        _manager.AddUser(user);
        Console.WriteLine($"User '{name}' added with ID {id}.");
    }

    private void HandleAddEquipment()
    {
        Console.Write("Type (camera/laptop/projector): ");
        var type = Console.ReadLine()?.Trim().ToLower();

        var id = _idTracker.NextId();

        Console.Write("Producer: ");
        var producer = Console.ReadLine()!;

        Equipment equipment = type switch
        {
            "camera"    => CreateCamera(id, producer),
            "laptop"    => CreateLaptop(id, producer),
            "projector" => CreateProjector(id, producer),
            _ => throw new InvalidOperationException($"Unknown equipment type '{type}'.")
        };

        _manager.AddEquipment(equipment);
        Console.WriteLine($"{equipment.Name} added with ID {equipment.Id}.");
    }

    private static Equipment CreateCamera(string id, string producer)
    {
        Console.Write("Resolution: ");
        var resolution = Console.ReadLine()!;
        return new Camera(id, resolution, producer);
    }

    private static Equipment CreateLaptop(string id, string producer)
    {
        Console.Write("Description: ");
        var description = Console.ReadLine()!;
        return new Laptop(id, producer, description);
    }

    private static Equipment CreateProjector(string id, string producer)
    {
        Console.Write("Description: ");
        var description = Console.ReadLine()!;
        return new Projector(id, producer, description);
    }

    private void HandleListAllEquipment()
    {
        var items = _manager.GetAllEquipmentWithStatus().ToList();
        if (items.Count == 0) { Console.WriteLine("No equipment registered."); return; }
        foreach (var (eq, status) in items)
            Console.WriteLine($"  [{eq.Id}] {eq.Description}  |  {status}");
    }

    private void HandleListAvailableEquipment()
    {
        var items = _manager.GetAvailableEquipment().ToList();
        if (items.Count == 0) { Console.WriteLine("No equipment available."); return; }
        foreach (var eq in items)
            Console.WriteLine($"  [{eq.Id}] {eq.Description}");
    }

    private void HandleRentEquipment()
    {
        Console.Write("Equipment ID: ");
        var equipmentId = Console.ReadLine()!;

        Console.Write("User ID: ");
        var userId = Console.ReadLine()!;

        Console.Write("Rental days: ");
        var days = ReadInt();

        var rental = _manager.RentEquipment(equipmentId, userId, days);
        Console.WriteLine($"Rented '{rental.Equipment.Description}' to {rental.Renter.Fullname} until {rental.Due:yyyy-MM-dd}.");
    }

    private void HandleReturnEquipment()
    {
        Console.Write("Equipment ID: ");
        var equipmentId = Console.ReadLine()!;

        var (inactive, penalty) = _manager.ReturnEquipment(equipmentId);
        if (penalty is > 0)
            Console.WriteLine($"Returned '{inactive.Rental.Equipment.Description}'. Late penalty: {penalty:F2}");
        else
            Console.WriteLine($"Returned '{inactive.Rental.Equipment.Description}'. No penalty.");
    }

    private void HandleMarkUnavailable()
    {
        Console.Write("Equipment ID: ");
        var equipmentId = Console.ReadLine()!;

        _manager.MarkEquipmentUnavailable(equipmentId);
        Console.WriteLine($"Equipment '{equipmentId}' marked as unavailable.");
    }

    private void HandleUserActiveRentals()
    {
        Console.Write("User ID: ");
        var userId = Console.ReadLine()!;

        var rentals = _manager.GetUserActiveRentals(userId).ToList();
        if (rentals.Count == 0) { Console.WriteLine("No active rentals."); return; }
        foreach (var r in rentals)
            Console.WriteLine($"  [{r.Equipment.Id}] {r.Equipment.Description}  |  Due: {r.Due:yyyy-MM-dd}");
    }

    private void HandleOverdueRentals()
    {
        var rentals = _manager.GetOverdueRentals().ToList();
        if (rentals.Count == 0) { Console.WriteLine("No overdue rentals."); return; }
        foreach (var r in rentals)
        {
            var daysOverdue = (DateTime.Now - r.Due).Days;
            Console.WriteLine($"  [{r.Equipment.Id}] {r.Equipment.Description}  |  Renter: {r.Renter.Fullname}  |  Overdue: {daysOverdue} day(s)");
        }
    }

    private static int ReadInt()
    {
        var input = Console.ReadLine();
        if (!int.TryParse(input, out var value))
            throw new InvalidOperationException($"'{input}' is not a valid integer.");
        return value;
    }

    private void HandleSummaryReport()
    {
        var s = _manager.GetSummaryReport();
        Console.WriteLine($"  Users registered:      {s.TotalUsers}");
        Console.WriteLine($"  Total equipment:       {s.TotalEquipment}");
        Console.WriteLine($"  Available:             {s.AvailableEquipment}");
        Console.WriteLine($"  Currently rented:      {s.CurrentlyRented}");
        Console.WriteLine($"  Unavailable (dmg/mnt): {s.UnavailableEquipment}");
        Console.WriteLine($"  Overdue rentals:       {s.OverdueRentals}");
        Console.WriteLine($"  Completed rentals:     {s.CompletedRentals}");
    }
}
