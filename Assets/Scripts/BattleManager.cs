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
    public GameObject[] Plushes;
    public float Player;
    public int Health;

    //THESE VARIABLES ARE FOR THE ENEMY
    [Header("Enemy Management")]
    public List<GameObject> Enemies;
    public float Enemy;

    //THESE ARE FOR THE UI
    [Header("User Interface")]
    public GameObject[] Buttons;
    public bool Select;

    public int ButtonA;
    public int ButtonB;
    public int ButtonC;
    public int ButtonD;

    public bool MenuSummon;

    //THESE ARE FOR GAME LOGIC
    public bool SpeedTie;

    //THESE ARE FOR CALCULATIONS
    [Header("Calculator Stuff")]
    public float Damage;

    void Start()
    {
        ButtonA = 0;
        ButtonB = 1;
        ButtonC = 2;
        ButtonD = 3;

        MenuSummon = true;
        Health = 16;
        pstat = GetComponent<PlayerStats>();
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
        { Health -= 1; }
        if (Input.GetKeyDown(KeyCode.D))
        { Health += 1; }

        hm.UpdateHearts(Health);

        if (Input.GetKeyDown(KeyCode.F))
            TurnStart();

        if (Input.GetKeyDown(KeyCode.Q) && MenuSummon == true)
        {
            PlayerMove1();
        }
        if (Input.GetKeyDown(KeyCode.E) && MenuSummon == true)
        {
            EnemyMove1();
        }
    }

    void TurnStart()
    {
        if (pstat.Speed == estat.Speed)
        { 
            Debug.Log("Speed Tied!"); 
            int flipcoin = Random.Range(0,2);
            if (flipcoin == 0)
            {
                Debug.Log("Player First!");
            }
            if (flipcoin == 1)
            {
                Debug.Log("Enemy First!");
            }
        }

        if (pstat.Speed > estat.Speed)
        {
            Debug.Log("Player First!");
        }
        if (pstat.Speed < estat.Speed)
        {
            Debug.Log("Enemy First!");
        }
        TurnMiddle();
    }

    void TurnMiddle()
    {
        Debug.Log("Middle of turn");
        TurnEnd();
    }

    void TurnEnd()
    {
        Debug.Log("End of turn");
    }

    void Summon()
    {
        MenuSummon = true;
        //This summons the enemy on scene, EnemyManager is assigned "e" stats
        EnemyManager = Instantiate(Enemies[Random.Range(0, Enemies.Count)], transform);
        estat = EnemyManager.GetComponent<EnemyStats>();

        //Player summon
        Instantiate(Plushes[0], transform);

        Instantiate(Buttons[ButtonA], new Vector3(-8.1f, -0.75f, 0), Quaternion.identity, transform);
        Instantiate(Buttons[ButtonB], new Vector3(-6.7f, -0.75f, 0), Quaternion.identity, transform);
        Instantiate(Buttons[ButtonC], new Vector3(-5.3f, -0.75f, 0), Quaternion.identity, transform);
        Instantiate(Buttons[ButtonD], new Vector3(-3.9f, -0.75f, 0), Quaternion.identity, transform);
        hm.UpdateHearts(Health);
    }

    void Destruction()
    {
        MenuSummon = false;
        Debug.Log("Battle Manager Cleared");
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void PlayerMove1()
    {
        Damage = pstat.Power - estat.Defense;
        if (Damage < 0)
        {
            Damage = 0;
        }
        estat.Health = estat.Health - Damage;
        Debug.Log("Enemy took" + Damage + "damage");
        Damage = 0;
    }

    void EnemyMove1()
    {
        Damage = estat.Power - pstat.Defense;
        if (Damage < 0)
        {
            Damage = 0;
        }
        Health = Health - (int) Damage;
        Debug.Log("Player took" + Damage + "damage");
        Damage = 0;
    }
}
