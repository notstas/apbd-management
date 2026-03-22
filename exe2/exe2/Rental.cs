using exe2.equipment;
using exe2.user;

namespace exe2;

public record Rental(IEquipment Equipment, User Renter, DateTime rentalDate, DateTime dueDate);