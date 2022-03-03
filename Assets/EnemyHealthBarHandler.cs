using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarHandler : MonoBehaviour
{
    private Image healthBarImage;
    private EntityComponent enemy;
    private float maxEnemyHealth;

    private void Start()
    {
        healthBarImage = GetComponent<Image>();
        enemy = transform.parent.parent.GetComponent<EntityComponent>();
        maxEnemyHealth = enemy.MaxHealth;
    }

    private void Update() {
        HandleHealthBarValue();
    }

    private void HandleHealthBarValue()
    {
        float currentFillAmout = enemy.Health / maxEnemyHealth;
        healthBarImage.fillAmount = currentFillAmout;
        if (healthBarImage.fillAmount < 0.5f)
        {
            SetHealthBarColor(Color.red);
        }
        else if (healthBarImage.fillAmount < 0.8f)
        {
            SetHealthBarColor(Color.yellow);
        }
        else
        {
            SetHealthBarColor(Color.green);
        }
    }

    private void SetHealthBarColor(Color healthColor)
    {
        healthBarImage.color = healthColor;
    }
}