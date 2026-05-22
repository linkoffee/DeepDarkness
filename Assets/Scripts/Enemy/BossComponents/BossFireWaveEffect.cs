using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class BossFireWaveEffect : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;

    [SerializeField] private float delayBeforeEffect = 5f;
    [SerializeField] private float delayBeforeDamage = 0.7f;
    [SerializeField] private float effectDuration = 3f;

    [SerializeField] private float damageRadius = 3.5f;

    private Enemy _bossEnemy;
    private GameObject _spawnedEffect;
    private ParticleSystem _spawnedPS;

    private bool _isExecuting = false;


    private void Awake()
    {
        _bossEnemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
         _bossEnemy.OnEnemyDeath += OnEnemyDeath;
    }

    private void OnDisable()
    {
        _bossEnemy.OnEnemyDeath -= OnEnemyDeath;
        Destroy(_spawnedEffect);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }

    private void OnEnemyDeath()
    {
        if (_isExecuting) return;
        StartCoroutine(FireWaveEffect());
    }

    private void DealDamageToPlayer()
    {
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(transform.position, damageRadius);

        foreach (var hit in hitCollider)
        {
            if (hit.CompareTag("Player"))
            {
                Player.Instance.TakeDamage(_bossEnemy.EnemyData.attackDamage);
                break;
            }
        }
    }

    private IEnumerator FireWaveEffect()
    {
        _isExecuting = true;

        yield return new WaitForSeconds(delayBeforeEffect);

        _spawnedEffect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        _spawnedPS = _spawnedEffect.GetComponent<ParticleSystem>();

        SfxManager.Instance.PlaySound2D("FireWave");

        yield return new WaitForSeconds(delayBeforeDamage);

        DealDamageToPlayer();

        if (_spawnedPS != null)
        {
            float particleDuration = _spawnedPS.main.duration;
            yield return new WaitForSeconds(Mathf.Max(effectDuration, particleDuration));

            Destroy(_spawnedEffect);
        }
        else if (effectPrefab != null)
        {
            yield return new WaitForSeconds(effectDuration);

            if (_spawnedEffect != null)
                Destroy(_spawnedEffect);
        }

        _isExecuting = false;
    }
}
