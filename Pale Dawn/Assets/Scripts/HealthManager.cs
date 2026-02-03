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
        healthObjs = Health.Length; //if length doesnt count 0, then change for loop to be healthObjs-1

        if (maxHealth > healthObjs)
        {
            throw new NotImplementedException();
        } else if (maxHealth < healthObjs)
        {
            for (int i = healthObjs; i > maxHealth; i--)
            {
                Health[i].GetComponent<Health>().Hide();
            }
        }
    }

    public void damage()
    {
        Health[curHealth].GetComponent<Health>().Hide();
        curHealth -= 1;
    }
}
