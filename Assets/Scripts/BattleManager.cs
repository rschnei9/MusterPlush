using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
//REFERENCES
public HeartManager hm;
public PlayerSprite ps;
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
[Header("User Interface")]
    public GameObject [] Buttons;
    public bool Select;

    public float ButtonA;
    public float ButtonB;
    public float ButtonC;
    public float ButtonD;

    public bool MenuSummon;

//THESE ARE FOR CALCULATIONS
[Header("Calculator Stuff")]
    public int Damage;

    void Start()
    {
        MenuSummon = true;
        Health = 16;
        Summon();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && MenuSummon == true)
        {
            Destruction();
        }
        if (Input.GetKeyDown(KeyCode.X) && MenuSummon == false)
        {
            Summon();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ps.DamageTaken(Damage);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {Health -= 1;}
        if (Input.GetKeyDown(KeyCode.D))
        {Health += 1;}

        hm.UpdateHearts(Health);
    }

    void Summon()
    {
        MenuSummon = true;
        Instantiate(Enemies[Random.Range(0,Enemies.Count)],transform);
        Instantiate(Plushes[0],transform);

        Instantiate(Buttons[0],transform);
        Instantiate(Buttons[1],transform);
        Instantiate(Buttons[2],transform);
        Instantiate(Buttons[3],transform);
        hm.UpdateHearts(Health);
    }

    void Destruction()
    {
        MenuSummon = false;
        Debug.Log("Battle Manager Cleared");
        for (int i=0; i<transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
