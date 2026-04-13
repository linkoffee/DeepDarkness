public abstract class Command
{
    public abstract void Execute(Player player);
}

public class MoveCommand : Command
{
    private readonly string _direction;
    private readonly int _stepCount;

    public string Direction => _direction;
    public int StepCount => _stepCount;

    public MoveCommand(string direction, int stepCount)
    {
        _direction = direction;
        _stepCount = stepCount;
    }
    
    public override void Execute(Player player)
    {
        switch (_direction.ToLower())
        {
            case "up": player.MoveUp(_stepCount); break;
            case "down": player.MoveDown(_stepCount); break;
            case "left": player.MoveLeft(_stepCount); break;
            case "right": player.MoveRight(_stepCount); break;
        }
    }
}

public class AttackCommand : Command
{
    public override void Execute(Player player) => player.Attack();
}

public class BlockCommand : Command
{
    public override void Execute(Player player) => player.Block();
}