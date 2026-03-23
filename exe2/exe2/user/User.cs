namespace exe2.user;

public abstract class User(string fullname, string id, int balance)
{
    public string Id = id;
    public int Balance = balance;
    public readonly string Fullname = fullname;

    public abstract double LateReturnInterest { get; }
    public abstract int MaxSimultaneousEquipment { get; }
    public abstract int MaxRentalDays { get; }
    
}