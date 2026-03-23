using exe2.equipment;
using exe2.user;

namespace exe2;

public class Manager: IManager
{
    private readonly Inventory _inventory = new();

    public void AddUser(User user)
    {
        if (_inventory.Users.Any(u => u.Id == user.Id))
            throw new InvalidOperationException($"User with ID {user.Id} already exists.");
        _inventory.Users.Add(user);
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _inventory.Users;
    }

    public void AddEquipment(Equipment equipment)
    {
        if (_inventory.Equipments.Any(e => e.Id == equipment.Id))
            throw new InvalidOperationException($"Equipment with ID {equipment.Id} already exists.");
        _inventory.Equipments.Add(equipment);
    }

    public IEnumerable<(Equipment Equipment, string Status)> GetAllEquipmentWithStatus()
    {
        return _inventory.Equipments.Select(eq => (eq, GetEquipmentStatus(eq)));
    }

    public IEnumerable<Equipment> GetAvailableEquipment()
    {
        return _inventory.Equipments.Where(IsRentable);
    }

    public void MarkEquipmentAvailable(string equipmentId)
    {
        FindEquipment(equipmentId).IsAvailable = true;
    }

    public Rental RentEquipment(string equipmentId, string userId, int rentalDays)
    {
        var equipment = FindEquipment(equipmentId);
        var user = FindUser(userId);

        if (!IsRentable(equipment))
            throw new InvalidOperationException($"Equipment '{equipment.Name}' (ID: {equipmentId}) is not available for rental.");

        if (rentalDays > user.MaxRentalDays)
            throw new InvalidOperationException($"Rental period {rentalDays} days exceeds maximum {user.MaxRentalDays} days for {user.Fullname}.");

        var activeRentals = _inventory.UserActiveRentals(user);
        if (activeRentals.Count >= user.MaxSimultaneousEquipment)
            throw new InvalidOperationException($"{user.Fullname} has reached the maximum of {user.MaxSimultaneousEquipment} simultaneous rentals.");

        var rental = new Rental(equipment, user, DateTime.Now, DateTime.Now.AddDays(rentalDays));
        _inventory.Rentals.Add(rental);
        return rental;
    }

    public (InactiveRental InactiveRental, double? LatePenalty) ReturnEquipment(string equipmentId)
    {
        var rental = _inventory.Rentals.FirstOrDefault(r => r.Equipment.Id == equipmentId)
            ?? throw new InvalidOperationException($"No active rental found for equipment ID {equipmentId}.");

        _inventory.FinalizeRental(rental);
        var inactive = _inventory.InactiveRentals.Last();
        return (inactive, inactive.Interest());
    }

    public void MarkEquipmentUnavailable(string equipmentId)
    {
        var equipment = FindEquipment(equipmentId);

        if (_inventory.Rentals.Any(r => r.Equipment.Id == equipmentId))
            throw new InvalidOperationException($"Equipment '{equipment.Name}' (ID: {equipmentId}) is currently rented out and cannot be marked unavailable.");

        equipment.IsAvailable = false;
    }

    public IEnumerable<Rental> GetUserActiveRentals(string userId)
    {
        var user = FindUser(userId);
        return _inventory.UserActiveRentals(user);
    }

    public IEnumerable<Rental> GetOverdueRentals()
    {
        return _inventory.Rentals.Where(r => r.Due < DateTime.Now);
    }

    public RentalSummary GetSummaryReport()
    {
        return new RentalSummary(
            TotalUsers: _inventory.Users.Count,
            TotalEquipment: _inventory.Equipments.Count,
            AvailableEquipment: _inventory.Equipments.Count(IsRentable),
            CurrentlyRented: _inventory.Rentals.Count,
            UnavailableEquipment: _inventory.Equipments.Count(e => !e.IsAvailable),
            OverdueRentals: _inventory.Rentals.Count(r => r.Due < DateTime.Now),
            CompletedRentals: _inventory.InactiveRentals.Count
        );
    }

    private Equipment FindEquipment(string id) =>
        _inventory.Equipments.FirstOrDefault(e => e.Id == id)
        ?? throw new InvalidOperationException($"Equipment with ID {id} not found.");

    private User FindUser(string id) =>
        _inventory.Users.FirstOrDefault(u => u.Id == id)
        ?? throw new InvalidOperationException($"User with ID {id} not found.");

    private bool IsRentable(Equipment equipment) =>
        equipment.IsAvailable &&
        !_inventory.Rentals.Any(r => r.Equipment.Id == equipment.Id);

    private string GetEquipmentStatus(Equipment equipment)
    {
        if (!equipment.IsAvailable)
            return "Unavailable (damage/maintenance)";

        var rental = _inventory.Rentals.FirstOrDefault(r => r.Equipment.Id == equipment.Id);
        if (rental != null)
        {
            var overdue = rental.Due < DateTime.Now ? " [OVERDUE]" : "";
            return $"Rented to {rental.Renter.Fullname} until {rental.Due:yyyy-MM-dd}{overdue}";
        }

        return "Available";
    }
}