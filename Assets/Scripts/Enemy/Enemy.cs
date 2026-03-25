using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private FlowFieldManager flowField;
    [SerializeField] private float speed = 10f;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private int health = 10;
    [SerializeField] private float rotationSpeed = 360f;

    [SerializeField] private float directChaseRadius = 2f;
    [SerializeField] private Transform target;
    [SerializeField] private float separationRadius = 1f;
    [SerializeField] private float separationWeight = 0.5f;

    [SerializeField] private GameObject deathEffectPrefab;
    [SerializeField] private int damageToCar = 10;
    [SerializeField] private float attackRadius = 1f;

    private Vector2 bias;
    private Vector3 smoothDir;

    

    public void Init(FlowFieldManager flowField, Transform target)
    {
        this.target = target;
        this.flowField = flowField;

        bias = new Vector2(
            Random.Range(-0.2f, 0.2f),
            Random.Range(-0.2f, 0.2f)
        );
    }

    private Vector3 GetFlowDirection()
    {
        Vector3 dir = flowField.GetDirection(transform.position);
        if (dir == Vector3.zero) return Vector3.zero;

        Vector2 flow2D = new Vector2(dir.x, dir.z);
        Vector2 biased = (flow2D + bias).normalized;

        Vector3 biasDir = new Vector3(biased.x, 0, biased.y);

        // Separation from other enemies
        Collider[] neighbors = Physics.OverlapSphere(transform.position, separationRadius);
        Vector3 separation = Vector3.zero;
        foreach (var n in neighbors)
        {
            if (n.gameObject == gameObject) continue;
            Vector3 diff = transform.position - n.transform.position;
            separation += diff.normalized / diff.magnitude;
        }

        Vector3 finalDir = biasDir + separation * separationWeight;
        finalDir.y = 0f;
        return finalDir.normalized;
    }

    private Vector3 GetDirectDirection()
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        return dir.normalized;
    }

    private void MoveAlongDirection(Vector3 dir)
    {
        if (dir == Vector3.zero) return;

        smoothDir = Vector3.Lerp(smoothDir, dir, 10f * Time.deltaTime);

        // Smooth rotation
        Quaternion targetRot = Quaternion.LookRotation(smoothDir, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        // Move
        transform.position += smoothDir * speed * Time.deltaTime;
    }


    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        Vector3 moveDir;

        if (distance < directChaseRadius)
        {
            moveDir = GetDirectDirection();
        }
        else
        {
            moveDir = GetFlowDirection();
        }

        MoveAlongDirection(moveDir);
        enemyAnimator.SetBool("IsMoving", moveDir != Vector3.zero);

        CheckCarCollision();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }
    private bool hasAttacked = false;

    private void Die()
    {
        if (deathEffectPrefab != null)
        {
            var effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity,transform.parent);

            if (hasAttacked) 
                effect.transform.localScale *= 4f;
            else
                effect.transform.localScale *= 2f;
        }

        Destroy(gameObject);
    }

   

    private void CheckCarCollision()
    {
        if (hasAttacked) return;

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist <= attackRadius)
        {
            if (target.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(damageToCar);

            hasAttacked = true;
            Die();
        }
    }

}
