using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
//REFERENCES
public HeartManager hm;
//THESE VARIABLES ARE FOR THE PLAYER
[Header("Player Management")]
public GameObject [] Plushes;
    public float Player;
    public int Health;

    public float Defense;
    public float Power;
    public float Speed;

    public float Buff;
    public float Debuff;
    public float Condition;

    public float DebuffTimer;
    public float BuffTimer;
    public float ConditionTimer;

//THESE VARIABLES ARE FOR THE ENEMY
[Header("Enemy Management")]
public List<GameObject> Enemies;
    public float Enemy;

    public float EDefense;
    public float EPower;
    public float ESpeed;

    public float EBuff;
    public float EDebuff;
    public float ECondition;

    public float EDebuffTimer;
    public float EBuffTimer;
    public float EConditionTimer;

//THESE ARE FOR THE UI
    public GameObject [] Buttons;
    public bool Select;

    public float ButtonA;
    public float ButtonB;
    public float ButtonC;
    public float ButtonD;

    void Start()
    {
        Health = 16;
        Summon();
        hm.UpdateHearts(Health);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Destruction();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Summon();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            hm.UpdateHearts(Health);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {Health -= 1;}
        if (Input.GetKeyDown(KeyCode.D))
        {Health += 1;}
    }

    void Summon()
    {
        Instantiate(Enemies[Random.Range(0,Enemies.Count)],transform);
        Instantiate(Plushes[0],transform);

        Instantiate(Buttons[0],transform);
        Instantiate(Buttons[1],transform);
        Instantiate(Buttons[2],transform);
        Instantiate(Buttons[3],transform);
    }

    void Destruction()
    {
        Debug.Log("Battle Manager Cleared");
        for (int i=0; i<transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
