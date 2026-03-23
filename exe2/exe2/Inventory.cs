using exe2.equipment;
using exe2.user;

namespace exe2;

public class Inventory
{
    public List<IEquipment> Equipments { get; protected set; }
    public List<User> Users { get; protected set; }
    
    public List<Rental> Rentals { get; protected set; }
    public List<InactiveRental> InactiveRentals { get; protected set; }

    public List<Rental> UserActiveRentals(User user)
    {
        return Rentals.Where(r => r.Renter.Equals(user)).ToList();
    }

    public void FinalizeRental(Rental rental)
    {
        Rentals.Remove(rental);
        InactiveRentals.Append(new InactiveRental(rental, new DateTime()));
    }
    
    
}