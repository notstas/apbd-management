using exe2.equipment;
using exe2.user;

namespace exe2;

public record Rental(Equipment Equipment, User Renter, DateTime Started, DateTime Due);
