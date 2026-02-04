using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] Health;
    private int maxHealth;
    private int curHealth;
    private int healthObjs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = player.GetComponent<Player>().getMaxHealth();
        curHealth = player.GetComponent<Player>().getMaxHealth();
        healthObjs = Health.Length;

        if (maxHealth > healthObjs)
        {
            throw new NotImplementedException();
        } else if (maxHealth < healthObjs)
        {
            for (int i = healthObjs; i > maxHealth; i--)
            {
                Health[i-1].GetComponent<Health>().Hide();
            }
        }
    }

    public void damage()
    {
        curHealth -= 1;
        Health[curHealth].GetComponent<Health>().FireOff();
    }
}
