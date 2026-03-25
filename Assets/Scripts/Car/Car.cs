using UnityEngine;

public class Car : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    public event System.Action OnCarDestroyed;
    public event System.Action<float> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(1f);
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        float normalized = (float)currentHealth / maxHealth;
        OnHealthChanged?.Invoke(normalized);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Car destroyed! Game over");
        OnCarDestroyed?.Invoke();
    }
}