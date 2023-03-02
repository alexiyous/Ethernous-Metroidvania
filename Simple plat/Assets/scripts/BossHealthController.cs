using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : MonoBehaviour
{
    public static BossHealthController instance;

    private void Awake()
    {
        instance = this;
    }

    public Slider bossHealthSlider;

    public int currentHealth = 30;

    public BossBattle theBoss;

    // Start is called before the first frame update
    void Start()
    {
        bossHealthSlider.maxValue = currentHealth;
        bossHealthSlider.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            theBoss.EndBattle();

            AudioManager.instance.PlaySFX(0);//play boss death SFX
        } else
        {
            AudioManager.instance.PlaySFX(1);//play boss impact SFX
        }

        bossHealthSlider.value = currentHealth;
    }
}
