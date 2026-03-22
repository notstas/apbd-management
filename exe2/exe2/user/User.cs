namespace exe2.user;

public abstract class User(string fullname)
{
    public readonly string Fullname = fullname;

    public abstract double LateReturnInterest { get; }
    public abstract int MaxSimultaneousEquipment { get; }
    public abstract int MaxRentalDays { get; }
}