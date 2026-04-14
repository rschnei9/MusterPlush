using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
//REFERENCES
public HeartManager hm;

private PlayerStats pstat;
private EnemyStats estat;

public GameObject PlayerManager;
public GameObject EnemyManager;

//THESE VARIABLES ARE FOR THE PLAYER
[Header("Player Management")]
public GameObject [] Plushes;
    public float Player;
    public int Health;

//THESE VARIABLES ARE FOR THE ENEMY
[Header("Enemy Management")]
public List<GameObject> Enemies;
    public float Enemy;

//THESE ARE FOR THE UI
[Header("User Interface")]
    public GameObject [] Buttons;
    public bool Select;

    public float ButtonA;
    public float ButtonB;
    public float ButtonC;
    public float ButtonD;

    public bool MenuSummon;

//THESE ARE FOR GAME LOGIC
    public bool SpeedTie;

//THESE ARE FOR CALCULATIONS
[Header("Calculator Stuff")]
    public int Damage;

    void Start()
    {
        MenuSummon = true;
        Health = 16;
        PlayerManager = GameObject.Find("BattleManager");
        pstat = PlayerManager.GetComponent<PlayerStats>();
        EnemyManager = GameObject.Find("BattleManager");
        estat = EnemyManager.GetComponent<EnemyStats>();
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
        if (Input.GetKeyDown(KeyCode.A))
        {Health -= 1;}
        if (Input.GetKeyDown(KeyCode.D))
        {Health += 1;}

        hm.UpdateHearts(Health);

        if (Input.GetKeyDown(KeyCode.F))
        TurnStart();
    }

    void TurnStart()
    {
        //Speed Tied to start
        // if (Speed == ESpeed)
        //{
        // Speed += 1;
        // SpeedTie = true;
        //}
        //Player is faster
        //else if (Speed > ESpeed)
        //{
        // Damage = (int)(Power - EDefense);
        //}
        //Enemy is faster
        // else if (Speed < ESpeed)
        //{
        // Damage = (int)(EPower - Defense);
        //}

        {
            TestA = pstat.PlayerPower;
            TestB = estat.EnemySpeed;
        }

        TurnEnd();
    }

    void TurnEnd()
    {
        if (SpeedTie)
        {
            // Speed -= 1;
        }
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
