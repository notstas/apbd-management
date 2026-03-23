using exe2.equipment;
using exe2.user;

namespace exe2;

public class Inventory
{
    private int IdCounter = 1;
    public List<Equipment> Equipments { get; protected set; } = new();
    public List<User> Users { get; protected set; } = new();
    public List<Rental> Rentals { get; protected set; } = new();
    public List<InactiveRental> InactiveRentals { get; protected set; } = new();

    public List<Rental> UserActiveRentals(User user)
    {
        return Rentals.Where(r => r.Renter.Equals(user)).ToList();
    }

    public void FinalizeRental(Rental rental)
    {
        Rentals.Remove(rental);
        InactiveRentals.Add(new InactiveRental(rental, DateTime.Now));
    }

    public string NewEquipmentId()
    {
        return $"#{IdCounter++}";
    }
}
