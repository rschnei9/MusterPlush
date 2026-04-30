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
    public int EnemyChoice;

    //THESE ARE FOR THE UI
    [Header("User Interface")]
    public GameObject[] Buttons;
    public GameObject[] ActiveButtons;
    public GameObject[] Hearts;
    public GameObject[] Screens;
    public bool Select;
    public bool BattleEnd;
    public bool Pause;

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
    public bool MenuSelect;

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
    public int Chance;

    void Start()
    {
        ButtonA = 0;
        ButtonB = 1;
        ButtonC = 2;
        ButtonD = 3;

        Defeat = false;
        MenuSummon = false;
        MenuSelect = true;
        Health = 16;
        pstat = GetComponent<PlayerStats>();
    }

    void Update()
    {
        //ACTIVE MODIFIERS
        if (Health >= 16)
            {Health = 16;}

        if (Health <= 0 && Defeat == false && BattleEnd == false)
        {
            Health = 0;
            Defeat = true;
            Debug.Log("Player Defeated");
            Destruction();
        }
        //TEMPORARY TEST FUNCTIONS
        if (Input.GetKeyDown(KeyCode.Z) && MenuSummon == true) {Destruction();}
        if (Input.GetKeyDown(KeyCode.X) && MenuSummon == false) {Summon();}
        if (Input.GetKeyDown(KeyCode.Y)) { Health -= 1; }
        if (Input.GetKeyDown(KeyCode.U)) { Health += 1; }
        //UI INDICATORS
        hm.UpdateHearts(Health);

        //MOVE SLIDER UI
            if (Input.GetKeyDown(KeyCode.D) && MenuSelect == true)
                {SelectMoveR();}
            if (Input.GetKeyDown(KeyCode.A) && MenuSelect == true)
                {SelectMoveL();}
            
        //MOVE SELECTION
        if (Input.GetKeyDown(KeyCode.E) && MenuSelect == true && Defeat == false && estat.EnemyDefeat == false)
        {
            MenuSelect = false;
            StartCoroutine("TurnStart");
        }
    }

    //THIS IS THE TURN CALCULATOR, DETERMINING WHO MOVES FIRST AND SECOND.
    public IEnumerator TurnStart()
    {
        Debug.Log("Start of the turn!");
        ActiveButtons[0].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();
        yield return new WaitForSeconds(1f);
        StartCoroutine("TurnSpeed");
    }
    public IEnumerator TurnSpeed()
    {
        if (pstat.Speed == estat.Speed)
        {
            Debug.Log("Speed Tied!"); int flipcoin = Random.Range(0,2);

            if (flipcoin == 0)
                {PlayerMove = true; Debug.Log("Player First!");}
            if (flipcoin == 1)
                {EnemyMove = true; Debug.Log("Enemy First!");}
        }
        if (pstat.Speed > estat.Speed)
            {PlayerMove = true; Debug.Log("Player First!");}
        if (pstat.Speed < estat.Speed)
            {EnemyMove = true; Debug.Log("Enemy First!");}

        else
            yield return new WaitForSeconds(0.25f);
            //Debug.Log("End of Speed Check");
            FirstTurn = true;
            yield return new WaitForSeconds(0.75f);
            StartCoroutine("TurnOne");
    }
    public IEnumerator TurnOne()
    {
        if (estat.EnemyDefeat == false || Defeat == false)
        {
        if (FirstTurn = true && PlayerMove == true && Defeat == false && estat.EnemyDefeat == false)
            {PlayerMovePlan();}
        if (FirstTurn = true && EnemyMove == true && Defeat == false && estat.EnemyDefeat == false)
            {
                EnemyChoice = Random.Range(1,12);
                if (EnemyChoice <= 7){EnemyChoice = estat.EnemyMoveA;}
                if (EnemyChoice >= 8 && EnemyChoice <= 10){EnemyChoice = estat.EnemyMoveB;}
                if (EnemyChoice >= 11 && EnemyChoice <= 12){EnemyChoice = estat.EnemyMoveC;}
                EnemyMovePlan();
            }

                yield return new WaitForSeconds(1f);
                FirstTurn = false; SecondTurn = true;
                StartCoroutine("TurnTwo");
        }
        else
        StartCoroutine("TurnEnd");
    }
    public IEnumerator TurnTwo()
    {
        if (estat.EnemyDefeat == false || Defeat == false)
        {
        if (SecondTurn = true && PlayerMove == false && Defeat == false && estat.EnemyDefeat == false)
            {PlayerMovePlan();}
        if (SecondTurn = true && EnemyMove == false && Defeat == false && estat.EnemyDefeat == false)
            {
                EnemyChoice = Random.Range(1,12);
                if (EnemyChoice <= 7){EnemyChoice = estat.EnemyMoveA;}
                if (EnemyChoice >= 8 && EnemyChoice <= 10){EnemyChoice = estat.EnemyMoveB;}
                if (EnemyChoice >= 11 && EnemyChoice <= 12){EnemyChoice = estat.EnemyMoveC;}
                EnemyMovePlan();
            }

                yield return new WaitForSeconds(1f);
                SecondTurn = false;
                StartCoroutine("TurnEnd");
        }
        else
                StartCoroutine("TurnEnd");
    }
    public IEnumerator TurnEnd()
    {
        if (estat.EnemyDefeat == false || Defeat == false)
        {
            pstat.TurnEnd = true;
            estat.TurnEnd = true;
            yield return new WaitForSeconds(0.15f);
            MenuStart();
            Debug.Log("End of turn!");
        }
        if (estat.EnemyDefeat == true)
        {
            Instantiate(Screens[0], transform);
            BattleEnd = true;
            Debug.Log("Enemy is defeated, select new move!");
            Destruction();
            GainingMove();
            estat.EnemyDefeat = false;
        }
        if (Defeat == true)
        {
            Instantiate(Screens[0], transform);
            BattleEnd = true;
            Debug.Log("Player loses the fight");
            Destruction();
            GameOver();
            Defeat = false;
        }
    }
    void MenuStart()
        {
        MenuSummon = true;
        Debug.Log("Started Hovering Button A"); ActiveButtons[0].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonA; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();
        MenuSelect = true;
        }

//MOVE SELECTION PROGRAM
    void SelectMoveL()
    {
                if (MoveABCD == ButtonB) {Debug.Log("Hovering Button A"); ActiveButtons[0].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonA; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();}
                else if (MoveABCD == ButtonC) {Debug.Log("Hovering Button B"); ActiveButtons[1].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonB; ActiveButtons[0].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();}
                else if (MoveABCD == ButtonD) {Debug.Log("Hovering Button C"); ActiveButtons[2].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonC; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[0].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();}
                else if (MoveABCD == ButtonA) {Debug.Log("Hovering Button D"); ActiveButtons[3].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonD; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[0].GetComponent<MoveUpdate>().UnHover();}
    }
    void SelectMoveR()
    {
                if (MoveABCD == ButtonD) {Debug.Log("Hovering Button A"); ActiveButtons[0].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonA; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();}
                else if (MoveABCD == ButtonA) {Debug.Log("Hovering Button B"); ActiveButtons[1].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonB; ActiveButtons[0].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();}
                else if (MoveABCD == ButtonB) {Debug.Log("Hovering Button C"); ActiveButtons[2].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonC; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[0].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();}
                else if (MoveABCD == ButtonC) {Debug.Log("Hovering Button D"); ActiveButtons[3].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonD; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[0].GetComponent<MoveUpdate>().UnHover();}
    }
//EVERY POSSIBLE MOVE!
    void PlayerMovePlan()
    {
        if (MoveABCD == 0)
        {
            //Debug.Log("Player Move 1");
            Damage = pstat.Power - estat.Defense/2;
            Chance = Random.Range(0,6);

            if (Chance == 6) {Damage = Damage * 1.5f; Debug.Log("A critical hit!"); Chance = 0;}
            if (Damage <= 1){Damage = 1;}
            estat.Health = estat.Health - (int) Damage;
            Debug.Log("Enemy took" + Damage + "damage");
            Damage = 0;
        }
        if (MoveABCD == 1)
        {
            //Debug.Log("Player Move 2");
            pstat.PowerU = true;
        }
        if (MoveABCD == 2)
        {
            //Debug.Log("Player Move 3");
            pstat.DefenseU = true;
        }
        if (MoveABCD == 3)
        {
            //Debug.Log("Player Move 4");
            pstat.SpeedU = true;
        }
        if (MoveABCD == 4)
        {
            //Debug.Log("Player Move 5");BURN
            Damage = pstat.Power - estat.Defense/2;
            Chance = Random.Range(0,9);

            if (Chance == 1) {Damage = Damage * 1.5f; Debug.Log("A critical hit!"); Chance = 0;}
            if (Damage <= 1){Damage = 1;}
            estat.Health = estat.Health - (int) Damage;
            Debug.Log("Enemy took" + Damage + "damage");
            Damage = 0;
            
            Chance = Random.Range(0,3);
            if (Chance == 1) {estat.BurningE = true; Chance = 0;}
        }
        if (MoveABCD == 5)
        {
            //Debug.Log("Player Move 6");FREEZE
            Damage = pstat.Power - estat.Defense/2;
            Chance = Random.Range(0,9);

            if (Chance == 1) {Damage = Damage * 1.5f; Debug.Log("A critical hit!"); Chance = 0;}
            if (Damage <= 1){Damage = 1;}
            estat.Health = estat.Health - (int) Damage;
            Debug.Log("Enemy took" + Damage + "damage");
            Damage = 0;
            
            Chance = Random.Range(0,3);
            if (Chance == 1) {estat.FreezeE = true; Chance = 0;}
        }
        if (MoveABCD == 6)
        {
            //Debug.Log("Player Move 7");DIZZY
            Damage = pstat.Power - estat.Defense/2;
            Chance = Random.Range(0,9);

            if (Chance == 1) {Damage = Damage * 1.5f; Debug.Log("A critical hit!"); Chance = 0;}
            if (Damage <= 1){Damage = 1;}
            estat.Health = estat.Health - (int) Damage;
            Debug.Log("Enemy took" + Damage + "damage");
            Damage = 0;
            
            Chance = Random.Range(0,3);
            if (Chance == 1) {estat.DizzyE = true; Chance = 0;}
        }
        if (MoveABCD == 7)
        {
            //Debug.Log("Player Move 8");REDSWORD
            Damage = (pstat.Power - estat.Defense/2) + 3;
            Chance = Random.Range(0,12);

            if (Chance == 1) {Damage = Damage * 1.5f; Debug.Log("A critical hit!"); Chance = 0;}
            if (Damage <= 1){Damage = 1;}
            estat.Health = estat.Health - (int) Damage;
            Debug.Log("Enemy took" + Damage + "damage");
            Damage = 0;
        }
        if (MoveABCD == 8)
        {
            //Debug.Log("Player Move 9");
            Health = Health + Random.Range(2,6);
        }
        if (MoveABCD == 9)
        {
            //Debug.Log("Player Move 10");
            Health = Health + 5;
        }
    }

//ENEMY MOVE SELECTION
    void EnemyMovePlan()
    {
        if (EnemyChoice == 1 || EnemyChoice == 2 || EnemyChoice == 3 || EnemyChoice == 4)
        {
            //Debug.Log("Enemy Move 1");
            Damage = estat.Power - pstat.Defense/2;
            Chance = Random.Range(0,12);

            if (Chance == 1) {Damage = Damage * 1.5f; Debug.Log("A critical hit!"); Chance = 0;}
            if (Damage <= 1){Damage = 1;}
            Health = Health - (int) Damage;
            Debug.Log("Player took" + Damage + "damage");
            Damage = 0;
        }
        if (EnemyChoice == 5)
        {
            Debug.Log("Enemy lowered Player Power");
            pstat.PowerD = true;
        }
        if (EnemyChoice == 6)
        {
            Debug.Log("Enemy lowered Player Defense");
            pstat.DefenseD = true;
        }
        if (EnemyChoice == 7)
        {
            Debug.Log("Enemy lowered Player Speed");
            pstat.SpeedD = true;
        }
        if (EnemyChoice == 8)
        {
            Debug.Log("Defense Ignorant Move");
            Damage = estat.Power - pstat.Defense/3;
            if (Damage <= 1){Damage = 1;}
            Health = Health - (int) Damage;
            Debug.Log("Player took" + Damage + "damage");
            Damage = 0;
        }
        if (EnemyChoice == 9)
        {
            Debug.Log("Enemy Move 9");
            estat.Health = estat.Health + 4;
        }
    }

    //PLAYER GETS A NEW MOVE
    void GainingMove()
    {
        Instantiate(Screens[1], transform);
    }
    //DOOM, MUAHAHAHAHHAHAHA!
    void GameOver()
    {
        Instantiate(Screens[2], transform);
    }
    //THIS IS FOR SUMMONING BATTLE UI AND TAKING IT DOWN
    void Summon()
    {
        MenuSummon = true;
        BattleEnd = false;
        pstat.PlushSelect = false;
        //This summons the enemy on scene, EnemyManager is assigned "e" stats
        EnemyManager = Instantiate(Enemies[Random.Range(0, Enemies.Count)], transform);
        estat = EnemyManager.GetComponent<EnemyStats>();
        //Player summon
        Instantiate(Plushes[pstat.PlushChoice], transform);
        //Button Summons
        ActiveButtons[0] = Instantiate(Buttons[ButtonA], new Vector3(-8.1f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[1] = Instantiate(Buttons[ButtonB], new Vector3(-6.7f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[2] = Instantiate(Buttons[ButtonC], new Vector3(-5.3f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[3] = Instantiate(Buttons[ButtonD], new Vector3(-3.9f, -0.75f, 0), Quaternion.identity, transform);
        //Heart Summons (broken)
        //Instantiate(Hearts[0], transform);
        //Instantiate(Hearts[1], transform);
        //Instantiate(Hearts[2], transform);
        //Instantiate(Hearts[3], transform);
        hm.UpdateHearts(Health);
        MenuStart();
    }

    void Destruction()
    {
        Instantiate(Screens[0], transform);
        pstat.PlushSelect = true;
        MenuSummon = false;
            Debug.Log("Battle Manager Cleared");
        for (int i = 0; i < transform.childCount; i++)
            {Destroy(transform.GetChild(i).gameObject);}
    }
}
