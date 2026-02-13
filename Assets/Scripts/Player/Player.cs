using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 1f;
    [SerializeField] private int maxHealth = 2;

    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerBlock;

    public static Player Instance { get; private set; }
    public bool IsBusy => _isBusy;
    public bool IsWalking => _isWalking;

    private bool _isBusy = false;
    private bool _isAlive;
    private bool _isWalking;

    private int _currentHealth;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _isAlive = true;
        _currentHealth = maxHealth;
    }

    public void MoveUp(int stepCount)
    {
        if (!_isBusy && _isAlive)
            StartCoroutine(Move(Vector3.up * stepCount));
    }
    
    public void MoveDown(int stepCount)
    {
        if (!_isBusy && _isAlive)
            StartCoroutine(Move(Vector3.down * stepCount));
    }
    
    public void MoveLeft(int stepCount)
    {
        if (!_isBusy && _isAlive)
            StartCoroutine(Move(Vector3.left * stepCount));
    }
    
    public void MoveRight(int stepCount)
    {
        if (!_isBusy && _isAlive)
            StartCoroutine(Move(Vector3.right * stepCount));
    }

    public void Attack()
    {
        if (!_isBusy && _isAlive)
        {
            OnPlayerAttack?.Invoke(this, EventArgs.Empty);
            StartCoroutine(DelayedAction());
        }
    }

    public void Block()
    {
        if (!_isBusy && _isAlive)
        {
            OnPlayerBlock?.Invoke(this, EventArgs.Empty);
            StartCoroutine(DelayedAction());
        }
    }

    public void PickUp()
    {
        if (!_isBusy && _isAlive)
        {
            Debug.Log("Pick Up");
            StartCoroutine(DelayedAction());
        }
    }
    
    private IEnumerator Move(Vector3 direction)
    {
        Vector3 targetPosition = transform.position + direction;
        
        _isBusy = true;
        _isWalking = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, targetPosition, movingSpeed * Time.deltaTime
            );
            yield return null;
        }

        _isWalking = false;
        _isBusy = false;
    }

    private IEnumerator DelayedAction(float animationDelay = 1f)
    {
        _isBusy = true;
        yield return new WaitForSeconds(animationDelay);
        _isBusy = false;
    }
}
