namespace exe2;

public record InactiveRental(
    Rental Rental,
    DateTime ReturnDate
)
{
    public double? Interest()
    {
        var overdueDelta = ReturnDate - Rental.Started;
        var overdueDays = overdueDelta.Days;
        
        // Rent is not overdue
        if (overdueDays < 0)
        {
            return null;
        }

        return overdueDays * Rental.Renter.LateReturnInterest;

    }
};