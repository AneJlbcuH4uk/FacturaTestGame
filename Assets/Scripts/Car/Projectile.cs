using System.Collections;
using UnityEngine;

public class Projectile: MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 100f;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float lifeDuration = 3;
 

    private void Awake()
    {
        StartCoroutine(LifetimeCoroutine());
    }

    private IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSecondsRealtime(lifeDuration);
        PlayHitEffect();
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            PlayHitEffect();

            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void PlayHitEffect() 
    {
        if (hitEffectPrefab != null)
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity,transform.parent);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

}
