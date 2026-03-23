using exe2.equipment;
using exe2.user;

namespace exe2;

public record RentalSummary(
    int TotalUsers,
    int TotalEquipment,
    int AvailableEquipment,
    int CurrentlyRented,
    int UnavailableEquipment,
    int OverdueRentals,
    int CompletedRentals
);

public interface IManager
{
    void AddUser(User user);
    void AddEquipment(Equipment equipment);
    IEnumerable<(Equipment Equipment, string Status)> GetAllEquipmentWithStatus();
    IEnumerable<Equipment> GetAvailableEquipment();
    Rental RentEquipment(string equipmentId, string userId, int rentalDays);
    (InactiveRental InactiveRental, double? LatePenalty) ReturnEquipment(string equipmentId);
    void MarkEquipmentUnavailable(string equipmentId);
    IEnumerable<Rental> GetUserActiveRentals(string userId);
    IEnumerable<Rental> GetOverdueRentals();
    RentalSummary GetSummaryReport();
}
