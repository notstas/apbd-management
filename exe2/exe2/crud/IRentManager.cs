namespace exe2.crud;

public interface IRentManager
{
    Rental RentEquipment(string equipmentId, string userId, int rentalDays);
    (InactiveRental InactiveRental, double? LatePenalty) ReturnEquipment(string equipmentId);
    IEnumerable<Rental> GetUserActiveRentals(string userId);
    IEnumerable<Rental> GetOverdueRentals();
}
