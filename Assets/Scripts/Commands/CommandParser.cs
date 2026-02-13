using System.Text.RegularExpressions;

public static class CommandParser
{
    public static Command Parse(string playerInput)
    {
        playerInput = playerInput.Trim().TrimEnd(';');

        var moveRightMatch = Regex.Match(playerInput, @"Knight\.MoveRight\((\d+)\)");
        if (moveRightMatch.Success)
            return new MoveCommand("right", int.Parse(moveRightMatch.Groups[1].Value));

        var moveLeftMatch = Regex.Match(playerInput, @"Knight\.MoveLeft\((\d+)\)");
        if (moveLeftMatch.Success)
            return new MoveCommand("left", int.Parse(moveLeftMatch.Groups[1].Value));

        var moveUpMatch = Regex.Match(playerInput, @"Knight\.MoveUp\((\d+)\)");
        if (moveUpMatch.Success)
            return new MoveCommand("up", int.Parse(moveUpMatch.Groups[1].Value));

        var moveDownMatch = Regex.Match(playerInput, @"Knight\.MoveDown\((\d+)\)");
        if (moveDownMatch.Success)
            return new MoveCommand("down", int.Parse(moveDownMatch.Groups[1].Value));

        switch (playerInput)
        {
            case "Knight.Attack()":
                return new AttackCommand();
            case "Knight.Block()":
                return new BlockCommand();
            case "Knight.PickUp()":
                return new PickUpCommand();
            default:
                return null;
        }
    }
}