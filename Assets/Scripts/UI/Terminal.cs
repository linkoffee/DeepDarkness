using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI logOutputText;
    [SerializeField] private Player player;

    public TMP_InputField InputField => inputField;

    private Queue<Command> _commands = new Queue<Command>();
    private Command _currentCommand = null;

    private bool _isExecuting = false;


    enum LogOutputType
    {
        Info,
        Warning,
        Error
    }

    private void Start()
    {
        if (InputField != null)
        {
            InputField.Select();
            InputField.ActivateInputField();
        }
    }

    private void Update()
    {
        if (_isExecuting && _currentCommand != null && !player.IsBusy)
        {
            _currentCommand = null;
            StartCoroutine(ExecuteNext());
        }
    }

    public void ExecuteAll()
    {
        if (_isExecuting)
        {
            AddLogOutput("Script is already executing!", LogOutputType.Warning);
            return;
        }

        if (inputField != null && !string.IsNullOrWhiteSpace(inputField.text))
        {
            _commands.Clear();

            string fullCommand = inputField.text;
            string[] commandLines = fullCommand.Split('\n');

            foreach (string commandLine in commandLines)
            {
                string trimmedCommand = commandLine.Trim();

                if (string.IsNullOrWhiteSpace(trimmedCommand))
                    continue;

                Command command = CommandParser.Parse(trimmedCommand);

                if (command != null)
                    _commands.Enqueue(command);
                else
                    AddLogOutput($"Skipped: {trimmedCommand} - Unknown command", LogOutputType.Warning);
            }

            if (_commands.Count > 0)
            {
                _isExecuting = true;
                StartCoroutine(ExecuteNext());
            }
            else
            {
                AddLogOutput("No valid commands found!", LogOutputType.Warning);
            }
        }
    }

    private IEnumerator ExecuteNext()
    {
        while (player.IsBusy)
            yield return null;

        if (_commands.Count == 0)
        {
            _isExecuting = false;
            _currentCommand = null;
            yield break;
        }

        _currentCommand = _commands.Dequeue();

        if (_currentCommand != null )
        {
            AddLogOutput($"> {GetCommandDescription(_currentCommand)}", LogOutputType.Info);
            _currentCommand.Execute(player);
        }
    }

    private string GetCommandDescription(Command command)
    {
        return command switch
        {
            MoveCommand move => $"Knight moves {move.Direction} {move.StepCount} step(s)",
            AttackCommand => "Attack! The enemy will be crushed!",
            BlockCommand => "Block! Protect the hull urgently!",
            PickUpCommand => "It should be picked up...",
            _ => command.GetType().Name
        };
    }

    //private bool ProcessCommand(string command)
    //{
    //    if (string.IsNullOrWhiteSpace(command))
    //        return false;

    //    return ExecuteCommand(command);
    //}

    //private bool ExecuteCommand(string commandText)
    //{
    //    try
    //    {
    //        Command command = CommandParser.Parse(commandText);

    //        if (command != null)
    //        {
    //            command.Execute(player);
    //            AddLogOutput($"> {commandText}");
    //            return true;
    //        }
    //        else
    //        {
    //            AddLogOutput($"{commandText} - Unknown command", LogOutputType.Warning);
    //            return false;
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        AddLogOutput($"{commandText} - Error: {e.Message}", LogOutputType.Error);
    //        return false;
    //    }
    //}

    private void AddLogOutput(string text, LogOutputType logOutputType = LogOutputType.Info)
    {
        Color textColor = logOutputType switch
        {
            LogOutputType.Info => new Color(0.65f, 1f, 0.65f),
            LogOutputType.Warning => Color.yellow,
            LogOutputType.Error => Color.red,
            _ => Color.white
        };

        logOutputText.text += $"<color=#{ColorUtility.ToHtmlStringRGB(textColor)}>{text}</color>\n";
    }
}
