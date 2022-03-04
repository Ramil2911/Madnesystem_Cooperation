using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class EnemyTextHandler : MonoBehaviour
{
    private TextMeshProUGUI healthText;
    private EntityComponent enemy;
    private float maxEnemyHealth;

    private void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        enemy = transform.parent.parent.GetComponent<EntityComponent>();
        maxEnemyHealth = enemy.MaxHealth;
    }

    private void Update()
    {
        HandleHealthText();
    }

    private void HandleHealthText()
    {
        float currentFillAmout = enemy.Health / maxEnemyHealth;

        healthText.text = ((int)enemy.Health).ToString() + "/" + maxEnemyHealth;
        if (currentFillAmout < 0.5f)
        {
            SetHealthBarColor(Color.red);
        }
        else if (currentFillAmout < 0.8f)
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
        healthText.color = healthColor;
    }
}
