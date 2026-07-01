using DD.Utils;

public abstract class Command
{
    public abstract void Execute(Player player);

    public virtual bool IsValid(out string errMsg)
    {
        errMsg = null;
        return true;
    }
}

public class MoveCommand : Command
{
    private readonly string _direction;
    private readonly int _stepCount;

    private const int MinStepCount = 1;
    private const int MaxStepCount = 32;

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

    public override bool IsValid(out string errMsg)
    {
        if (_stepCount < MinStepCount)
        {
            errMsg = Localizator.GetLocalizedText("TerminalErrMinStepCount", MinStepCount);
            return false;
        }
        else if (_stepCount > MaxStepCount)
        {
            errMsg = Localizator.GetLocalizedText("TerminalErrMaxStepCount", MaxStepCount);
            return false;
        }

        errMsg = null;
        return true;
    }

    public string GetLocalizedDirection()
    {
        return _direction.ToLower() switch
        {
            "up" => Localizator.GetLocalizedText("DirectionUp"),
            "down" => Localizator.GetLocalizedText("DirectionDown"),
            "left" => Localizator.GetLocalizedText("DirectionLeft"),
            "right" => Localizator.GetLocalizedText("DirectionRight"),
            _ => _direction
        };
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