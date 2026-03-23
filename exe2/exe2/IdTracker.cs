namespace exe2;

public class IdTracker
{
    private int _next = 1;

    public string NextId() => $"#{_next++}";
}
