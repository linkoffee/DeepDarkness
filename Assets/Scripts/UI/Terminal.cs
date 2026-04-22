using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI logOutputText;
    [SerializeField] private Button knowledgeBookButton;
    [SerializeField] private Button executeButton;
    [SerializeField] private Player player;

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
        if (inputField != null)
        {
            inputField.Select();
            inputField.ActivateInputField();
        }

        if (knowledgeBookButton != null)
            knowledgeBookButton.interactable = true;

        if (executeButton != null)
            executeButton.interactable = true;

        player.OnPlayerDeath += OnPlayerDeath;
    }

    private void Update()
    {
        if (_isExecuting && _currentCommand != null && !player.IsBusy)
        {
            _currentCommand = null;
            StartCoroutine(ExecuteNext());
        }
    }

    private void OnDestroy()
    {
        player.OnPlayerDeath -= OnPlayerDeath;
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
                AddLogOutput("No valid commands found!", LogOutputType.Error);
            }
        }
    }

    private void OnPlayerDeath(object sender, EventArgs e)
    {
        inputField.interactable = false;
        knowledgeBookButton.interactable = false;
        executeButton.interactable = false;

        AddLogOutput("> ==========================");
        AddLogOutput("> The knight's soul left him");
        AddLogOutput("> Try to start all over again...");
    }

    private string GetCommandDescription(Command command)
    {
        return command switch
        {
            MoveCommand move => $"Knight moves {move.Direction} {move.StepCount} step(s)",
            AttackCommand => "Attack! The enemy will be crushed!",
            BlockCommand => "Block! Protect the hull urgently!",
            _ => command.GetType().Name
        };
    }

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

        if (_currentCommand != null)
        {
            AddLogOutput($"> {GetCommandDescription(_currentCommand)}");
            _currentCommand.Execute(player);
        }
    }
}
