namespace exe2.user;

public class Student(string fullname) : User(fullname)
{
    public override double LateReturnInterest => (double)4.0;
    public override int MaxSimultaneousEquipment => 2;
    public override int MaxRentalDays => 10;
}