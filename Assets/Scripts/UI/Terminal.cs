using System.Collections;
using System.Collections.Generic;
using TMPro;
using DD.Utils;
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
        player.OnPlayerRanIntoObstacle += OnPlayerRanIntoObstacle;
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
        player.OnPlayerRanIntoObstacle -= OnPlayerRanIntoObstacle;
    }

    public void ExecuteAll()
    {
        if (_isExecuting)
        {
            AddLocalizedLogOutput("TerminalScriptIsAlreadyExecuting", LogOutputType.Warning);
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

                if (command == null)
                {
                    AddLocalizedLogOutput("TerminalUnknownCommand", LogOutputType.Warning, trimmedCommand);
                    continue;
                }

                string errMsg;
                if (IsCommandValid(command, out errMsg))
                    _commands.Enqueue(command);
                else
                    AddLocalizedLogOutput("TerminalInvalidCommand", LogOutputType.Warning, trimmedCommand, errMsg);
            }

            if (_commands.Count > 0)
            {
                _isExecuting = true;
                StartCoroutine(ExecuteNext());
            }
            else
            {
                AddLocalizedLogOutput("TerminalNoValidCommands", LogOutputType.Error);
            }
        }
    }

    private bool IsCommandValid(Command command, out string errMsg)
    {
        if (command is MoveCommand moveCommand)
            return moveCommand.IsValid(out errMsg);

        errMsg = null;
        return true;
    }

    private void OnPlayerDeath()
    {
        inputField.interactable = false;
        knowledgeBookButton.interactable = false;
        executeButton.interactable = false;

        AddLocalizedLogOutput("TerminalOnPlayerDeath");
    }

    private void OnPlayerRanIntoObstacle()
    {
        _isExecuting = false;

        inputField.interactable = false;
        knowledgeBookButton.interactable = false;
        executeButton.interactable = false;

        AddLocalizedLogOutput("TerminalOnPlayerRanIntoObstacle");
    }

    private string GetCommandDescription(Command command)
    {
        return command switch
        {
            MoveCommand move => Localizator.GetLocalizedText("TerminalStatusMove", move.GetLocalizedDirection(), move.StepCount),
            AttackCommand => Localizator.GetLocalizedText("TerminalStatusAttack"),
            BlockCommand => Localizator.GetLocalizedText("TerminalStatusBlock"),
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

    private void AddLocalizedLogOutput(string translationName, LogOutputType logOutputType = LogOutputType.Info, params object[] args)
    {
        string localizedText = Localizator.GetLocalizedText(translationName, args);
        AddLogOutput(localizedText, logOutputType);
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
