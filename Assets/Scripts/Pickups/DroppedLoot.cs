using System.Collections;
using UnityEngine;

public class DroppedLoot : MonoBehaviour
{
    private const float AttractionSpeed = 5f;
    private const float AttractionDelay = 0.5f;

    private Rigidbody2D _rb;

    private bool _isAttracting = false;

    private void Awake()
    {
        if (_rb == null)
        {
            _rb = gameObject.AddComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
            _rb.drag = 1;
        }
    }

    private void FixedUpdate()
    {
        if (_isAttracting)
        {
            AttractToPlayer();
        }
    }

    public void Initialize()
    {
        StartCoroutine(DelayedAttraction());
    }

    private void AttractToPlayer()
    {
        Vector2 direction = (Player.Instance.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, Player.Instance.transform.position);

        float speedMultiplier = Mathf.Lerp(1f, 2f, 1f - distance);
        _rb.velocity = direction * AttractionSpeed * speedMultiplier;
    }

    private IEnumerator DelayedAttraction()
    {
        yield return new WaitForSeconds(AttractionDelay);

        _isAttracting = true;
    }
}
