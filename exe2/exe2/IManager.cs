using exe2.crud;

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

public interface IManager : IEquipmentManager, IUserManager, IRentManager, ISummarizer;
