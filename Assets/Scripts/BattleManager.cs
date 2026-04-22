using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
    public GameObject[] Hearts;
    public bool Select;

    public int ButtonA;
    public int ButtonB;
    public int ButtonC;
    public int ButtonD;
    public int MoveABCD;

    public int EnemyA;
    public int EnemyB;
    public int EnemyC;
    public int EnemyABC;

    public bool MenuSummon;

    //THESE ARE FOR GAME LOGIC
    public bool SpeedTie;
    public bool Defeat;

    public bool PlayerMove;
    public bool EnemyMove;
    public bool FirstTurn;
    public bool SecondTurn;

    //THESE ARE FOR CALCULATIONS
    [Header("Calculator Stuff")]
    public float Damage;

    void Start()
    {
        ButtonA = 0;
        ButtonB = 1;
        ButtonC = 2;
        ButtonD = 3;

        Defeat = false;
        MenuSummon = true;
        Health = 16;
        pstat = GetComponent<PlayerStats>();
        Summon();
    }

    void Update()
    {
        //ACTIVE MODIFIERS
        if (Health >= 16)
        {
            Health = 16;
        }
        if (Health <= 0 && Defeat == false)
        {
            Health = 0;
            Defeat = true;
            Debug.Log("Player Defeated");
            Destruction();
        }

        //TEMPORARY TEST FUNCTIONS
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

        //UI INDICATORS
        hm.UpdateHearts(Health);

        //MOVE SELECTION
        if (Input.GetKeyDown(KeyCode.Alpha1) && MenuSummon == true)
        {
            MenuSummon = false;
            MoveABCD = ButtonA;
            StartCoroutine("TurnStart");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && MenuSummon == true)
        {
            MenuSummon = false;
            MoveABCD = ButtonB;
            StartCoroutine("TurnStart");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && MenuSummon == true)
        {
            MenuSummon = false;
            MoveABCD = ButtonC;
            StartCoroutine("TurnStart");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && MenuSummon == true)
        {
            MenuSummon = false;
            MoveABCD = ButtonD;
            StartCoroutine("TurnStart");
        }
    }

    //THIS IS THE TURN CALCULATOR, DETERMINING WHO MOVES FIRST AND SECOND.
    public IEnumerator TurnStart()
    {
        Debug.Log("Start of the turn!");
        yield return new WaitForSeconds(0.15f);
        StartCoroutine("TurnSpeed");
    }

    public IEnumerator TurnSpeed()
    {
        if (pstat.Speed == estat.Speed)
        { 
            Debug.Log("Speed Tied!"); 
            int flipcoin = Random.Range(0,2);
            if (flipcoin == 0)
            {
                PlayerMove = true;
                Debug.Log("Player First!");
            }
            if (flipcoin == 1)
            {
                EnemyMove = true;
                Debug.Log("Enemy First!");
            }
        }

        if (pstat.Speed > estat.Speed)
        {
            PlayerMove = true;
            Debug.Log("Player First!");
        }
        if (pstat.Speed < estat.Speed)
        {
            EnemyMove = true;
            Debug.Log("Enemy First!");
        }
        else
        yield return new WaitForSeconds(0.15f);
        Debug.Log("End of Speed Check");
        FirstTurn = true;
        yield return new WaitForSeconds(0.15f);
        StartCoroutine("TurnOne");
    }

    public IEnumerator TurnOne()
    {
        if (FirstTurn = true && PlayerMove == true && Defeat == false && estat.EnemyDefeat == false)
        {
            PlayerMovePlan();
        }

        if (FirstTurn = true && EnemyMove == true && Defeat == false && estat.EnemyDefeat == false)
        {
            EnemyMovePlan();
        }
            yield return new WaitForSeconds(0.15f);
            FirstTurn = false; SecondTurn = true;
            StartCoroutine("TurnTwo");
    }
    public IEnumerator TurnTwo()
    {
        if (SecondTurn = true && PlayerMove == false && Defeat == false && estat.EnemyDefeat == false)
        {
            PlayerMovePlan();
        }

        if (SecondTurn = true && EnemyMove == false && Defeat == false && estat.EnemyDefeat == false)
        {
            EnemyMovePlan();
        }
            yield return new WaitForSeconds(0.15f);
            SecondTurn = false;
            StartCoroutine("TurnEnd");
    }

    public IEnumerator TurnEnd()
    {
        MenuSummon = true;
        MoveABCD = 0;
        yield return new WaitForSeconds(0.15f);
        Debug.Log("End of turn");
    }

    //THIS IS FOR SUMMONING BATTLE UI AND TAKING IT DOWN

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

        Instantiate(Hearts[0], transform);
        Instantiate(Hearts[1], transform);
        Instantiate(Hearts[2], transform);
        Instantiate(Hearts[3], transform);

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

    //THESE CONTAIN ALL OF THE POSSIBLE MOVES IN THE ENTIRE GAME!!!!

    void PlayerMovePlan()
    {
        if (MoveABCD == 0)
        {
            Debug.Log("Player Move 1");
            Damage = pstat.Power - estat.Defense/2;
            estat.Health = estat.Health - Damage;
            Debug.Log("Enemy took" + Damage + "damage");
            Damage = 0;
        }
        if (MoveABCD == 1)
        {
            Debug.Log("Player Move 2");
        }
        if (MoveABCD == 2)
        {
            Debug.Log("Player Move 3");
        }
        if (MoveABCD == 3)
        {
            Debug.Log("Player Move 4");
        }
        if (MoveABCD == 4)
        {
            Debug.Log("Player Move 5");
        }
        if (MoveABCD == 5)
        {
            Debug.Log("Player Move 6");
        }
        if (MoveABCD == 6)
        {
            Debug.Log("Player Move 7");
        }
        if (MoveABCD == 7)
        {
            Debug.Log("Player Move 8");
        }
        if (MoveABCD == 8)
        {
            Debug.Log("Player Move 9");
        }
        if (MoveABCD == 9)
        {
            Debug.Log("Player Move 10");
        }
    }

    void EnemyMovePlan()
    {
        Damage = estat.Power - pstat.Defense/2;
        Health = Health - (int) Damage;
        Debug.Log("Player took" + Damage + "damage");
        Damage = 0;
    }

    //DOOM, MUAHAHAHAHHAHAHHAHHAHAHAHAHAHAHAHAHAHAAHAHHAHAHAHA!



}
