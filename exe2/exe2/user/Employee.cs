namespace exe2.user;

public class Employee(string fullname, string id, int balance) : User(fullname, id, balance)
{
    public override double LateReturnInterest => 1.0;
    public override int MaxSimultaneousEquipment => 5;
    public override int MaxRentalDays => 30;
}