using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

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

    public bool TurnEnding;
    public bool MenuSummon;
    public bool MenuSelect;
    public bool MoveSelect;
    public bool StatSelect;

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
    public int ChanceTwo;

    [Header("Display (UI)")]
    public GameObject[] UIBlock;
    public GameObject[] Condition;
    public TextMeshProUGUI TopUI;
    public TextMeshProUGUI UI;
    public TextMeshProUGUI PowerUI;
    public TextMeshProUGUI DefenseUI;
    public TextMeshProUGUI SpeedUI;
    public int DizzyCheck;
    public int BurnCheck;
    public int FreezeCheck;
    public int DizzyTB;
    public int BurningTB;
    public int FreezeTB;
    public bool TimerDeath;

    void Start()
    {
        pstat = GetComponent<PlayerStats>();

        ButtonA = 0;
        ButtonB = 1;
        ButtonC = 2;
        ButtonD = 3;
    
        Defeat = false;
        MenuSummon = false;
        MenuSelect = true;
        Health = 16;

        StartingSummon();
        //transform.Translate(0,22,0); I NEED THIS TO MOVE THE GAME OBJECT ABOVE LABELED AS "UIBlock", I ALSO NEED TO MOVE THE TEXTMESHPRO "TopUI"
        //This will be so that they get moved off screen while not in use, its for the plush and move select screen. The game over screen wont be so hard.
    }

    void Update()
    {   
        //Statistic Updates UI
        PowerUI.text =  "" + pstat.Power;
        DefenseUI.text =  "" + pstat.Defense;
        SpeedUI.text =  "" + pstat.Speed;

        if (MenuSummon && estat.Dizzy == 2) {DizzyCheck = 2;}
        else {DizzyCheck = 1;}
        if (MenuSummon && estat.Burning == 2) {BurnCheck = 2;}
        else {DizzyCheck = 1;}
        if (MenuSummon && estat.Freeze == 2) {FreezeCheck = 2;}
        else {DizzyCheck = 1;}

        //Special TIMER check
        if (MenuSummon) {DizzyTB = estat.DizzyT; BurningTB = estat.BurningT; FreezeTB = estat.FreezeT;}
        if (pstat.TimerFix == false) {TimerDeath = true;}
        if (pstat.TimerFix == true) {TimerDeath = false;}
        
        //ACTIVE MODIFIERS
        if (Health >= 16)
            {Health = 16;}

        if (Health <= 0 && Defeat == false && BattleEnd == false)
        {
            Health = 0;
            Defeat = true;
            BattleEnd = true;
            Debug.Log("Player Defeated");
            if (FirstTurn == true)
            {StartCoroutine("TurnOneDoom");}
        }
        if (MenuSummon && estat.EnemyDefeat && BattleEnd == false)
        {
            Debug.Log("Enemy Defeated");
            BattleEnd = true;
            if (FirstTurn == true)
            {StartCoroutine("TurnOneDoom");}
        }
        
        //TEMPORARY TEST FUNCTIONS
        //if (Input.GetKeyDown(KeyCode.Z) && MenuSummon == true) {Destruction();}
        //if (Input.GetKeyDown(KeyCode.X) && MenuSummon == false) {Summon();}
        if (Input.GetKeyDown(KeyCode.Y)) { Health -= 1; }
        if (Input.GetKeyDown(KeyCode.U)) { Health += 1; }
        //if (Input.GetKeyDown(KeyCode.T)) {Instantiate(Screens[0], transform);}

        //PLUSH SELECTION FUNCTIONS
        if (Input.GetKeyDown(KeyCode.E) && pstat.PlushSelect == true) {Instantiate(Screens[0], transform); pstat.PlushSelect = false; MenuSelect = false; StartCoroutine("Starting");}
        if (pstat.PlushSelect == true) {Chance = 0 + pstat.PlushChoice;}

        if (pstat.PlushSelect == true && Chance == 0) {UI.text = "Cat Plush! This choice will last for the rest of the game until you lose!";}
        if (pstat.PlushSelect == true && Chance == 1) {UI.text = "Bear Plush! This choice will last for the rest of the game until you lose!";}
        if (pstat.PlushSelect == true && Chance == 2) {UI.text = "Bunny Plush! This choice will last for the rest of the game until you lose!";}

        //UI INDICATORS
        hm.UpdateHearts(Health);

        //MOVE SLIDER UI
            if (Input.GetKeyDown(KeyCode.D) && MenuSelect == true && MenuSummon == true)
                {SelectMoveR();}
            if (Input.GetKeyDown(KeyCode.A) && MenuSelect == true && MenuSummon == true)
                {SelectMoveL();}
            
        //MOVE SELECTION
        if (Input.GetKeyDown(KeyCode.E) && MenuSelect == true && Defeat == false && pstat.PlushSelect == false && estat.EnemyDefeat == false)
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
        yield return new WaitForSeconds(0.15f);
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
            {PlayerMove = true; Debug.Log("Player First!"); UI.text = "The Player is going first this turn!";}
        if (pstat.Speed < estat.Speed)
            {EnemyMove = true; Debug.Log("Enemy First!"); UI.text = "The Enemy is going first this turn!";}

        else
            yield return new WaitForSeconds(0.15f);
            //Debug.Log("End of Speed Check");
            FirstTurn = true;
            yield return new WaitForSeconds(1.5f);
            StartCoroutine("TurnOne");
    }
    public IEnumerator TurnOne()
    {
        Damage = 0;
        if (BattleEnd == false)
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
                yield return new WaitForSeconds(2f);
                FirstTurn = false; SecondTurn = true;
                StartCoroutine("TurnTwo");
        }
    }
    public IEnumerator TurnTwo()
    {
        Damage = 0;
        if (BattleEnd == false)
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
                yield return new WaitForSeconds(2f);
                SecondTurn = false;
                StartCoroutine("TurnEnd");
        }
        
    }
    public IEnumerator TurnOneDoom() {yield return new WaitForSeconds(2f); StartCoroutine("TurnEnd");}
    public IEnumerator TurnEnd()
    {
        if (BattleEnd == false)
        {
            pstat.TurnEnd = true;
            estat.TurnEnd = true;
            yield return new WaitForSeconds(0.15f);
            MenuStart();
            Debug.Log("End of turn!");
        }
        if (estat.EnemyDefeat == true)
        {
            BattleEnd = true;
            UI.text = "The Enemy has been defeated!";
            Debug.Log("Player wins the fight");
            yield return new WaitForSeconds(2f);
            StartCoroutine("MoveStarting");
        }
        if (Defeat == true)
        {
            BattleEnd = true;
            Instantiate(Screens[0], transform);
            UI.text = "The Player has been defeated!";
            Debug.Log("Player loses the fight");
            yield return new WaitForSeconds(2f);
            GameOver();
            Defeat = false;
        }
    }
    void MenuStart()
        {
        MenuSummon = true;
        Debug.Log("Started Hovering Button A"); ActiveButtons[0].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonA; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();
        DisplayMove();
        MenuSelect = true;
        }

//MOVE SELECTION PROGRAM
    void SelectMoveL()
    {
                if (MoveABCD == ButtonB) {Debug.Log("Hovering Button A"); ActiveButtons[0].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonA; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();}
                else if (MoveABCD == ButtonC) {Debug.Log("Hovering Button B"); ActiveButtons[1].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonB; ActiveButtons[0].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();}
                else if (MoveABCD == ButtonD) {Debug.Log("Hovering Button C"); ActiveButtons[2].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonC; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[0].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();}
                else if (MoveABCD == ButtonA) {Debug.Log("Hovering Button D"); ActiveButtons[3].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonD; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[0].GetComponent<MoveUpdate>().UnHover();}
            DisplayMove();
    }
    void SelectMoveR()
    {
                if (MoveABCD == ButtonD) {Debug.Log("Hovering Button A"); ActiveButtons[0].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonA; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();}
                else if (MoveABCD == ButtonA) {Debug.Log("Hovering Button B"); ActiveButtons[1].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonB; ActiveButtons[0].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();}
                else if (MoveABCD == ButtonB) {Debug.Log("Hovering Button C"); ActiveButtons[2].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonC; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[0].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();}
                else if (MoveABCD == ButtonC) {Debug.Log("Hovering Button D"); ActiveButtons[3].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonD; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[0].GetComponent<MoveUpdate>().UnHover();}
            DisplayMove();
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
            if (Damage <= 1) {Damage = 1;}
            estat.Health = estat.Health - (int) Damage;
            if (Chance == 6) {UI.text = "The Player landed a critical hit! The Enemy took " + (int) Damage + " Damage from the Basic Attack!";}
            if (Chance != 6) {UI.text = "The Enemy took " + Damage + " Damage from the Basic Attack!";}
            Debug.Log("Enemy took" + Damage + "damage");
            Damage = 0;
        }
        if (MoveABCD == 1)
        {
            //Debug.Log("Player Move 2");
            UI.text = "The Player doubled their Power for 3 turns!";
            pstat.PowerU = true;
        }
        if (MoveABCD == 2)
        {
            //Debug.Log("Player Move 3");
            UI.text = "The Player doubled their Defense for 3 turns!";
            pstat.DefenseU = true;
        }
        if (MoveABCD == 3)
        {
            //Debug.Log("Player Move 4");
            UI.text = "The Player doubled their Speed for 3 turns!";
            pstat.SpeedU = true;
        }
        if (MoveABCD == 4)
        {
            //Debug.Log("Player Move 5");BURN
            Damage = pstat.Power - estat.Defense/2;
            Chance = Random.Range(0,9);

            if (Chance == 1) {Damage = Damage * 1.5f; Debug.Log("A critical hit!"); Chance = 0;}
            if (Damage <= 1) {Damage = 1;}

            ChanceTwo = Random.Range(0,3);
            if (ChanceTwo == 1) {estat.BurningE = true; Chance = 0;}

            estat.Health = estat.Health - (int) Damage;
            if (Chance == 1) {UI.text = "The Player landed a critical hit! The Enemy took " + (int) Damage + " Damage from the Fire Attack!";}
            if (ChanceTwo == 1) {UI.text = "The Player burned the Enemy! The Enemy took " + (int) Damage + " Damage from the Fire Attack";}
            if (Chance == 1 && ChanceTwo == 1) {UI.text = "Landed a critical hit and burned the Enemy! The Enemy took " + (int) Damage + " Damage from the Fire Attack!";}
            if (Chance != 1 && ChanceTwo != 1) {UI.text = "The Enemy took " + (int) Damage + " Damage from the Fire Attack!";}
            Debug.Log("Enemy took" + Damage + "damage");
            Damage = 0;
        }
        if (MoveABCD == 5)
        {
            //Debug.Log("Player Move 6");FREEZE
            Damage = pstat.Power - estat.Defense/2;
            Chance = Random.Range(0,9);

            if (Chance == 1) {Damage = Damage * 1.5f; Debug.Log("A critical hit!"); Chance = 0;}
            if (Damage <= 1) {Damage = 1;}

            ChanceTwo = Random.Range(0,3);
            if (ChanceTwo == 1) {estat.FreezeE = true; Chance = 0;}

            estat.Health = estat.Health - (int) Damage;
            if (Chance == 1) {UI.text = "The Player landed a critical hit! The Enemy took " + (int) Damage + " Damage from the Ice Attack!";}
            if (ChanceTwo == 1) {UI.text = "The Player froze the Enemy! The Enemy took " + (int) Damage + " Damage from the Ice Attack";}
            if (Chance == 1 && ChanceTwo == 1) {UI.text = "Landed a critical hit and froze the Enemy! The Enemy took " + (int) Damage + " Damage from the Ice Attack!";}
            if (Chance != 1 && ChanceTwo != 1) {UI.text = "The Enemy took " + (int) Damage + " Damage from the Ice Attack!";}
            Debug.Log("Enemy took" + Damage + "damage");
            Damage = 0;
        }
        if (MoveABCD == 6)
        {
            //Debug.Log("Player Move 7");DIZZY
            Damage = pstat.Power - estat.Defense/2;
            Chance = Random.Range(0,9);

            if (Chance == 1) {Damage = Damage * 1.5f; Debug.Log("A critical hit!"); Chance = 0;}
            if (Damage <= 1) {Damage = 1;}

            ChanceTwo = Random.Range(0,3);
            if (ChanceTwo == 1) {estat.DizzyE = true; Chance = 0;}

            estat.Health = estat.Health - (int) Damage;
            if (Chance == 1) {UI.text = "The Player landed a critical hit! The Enemy took " + (int) Damage + " Damage from the Dizzy Attack!";}
            if (ChanceTwo == 1) {UI.text = "The Player bewildered the Enemy! The Enemy took " + (int) Damage + " Damage from the Dizzy Attack";}
            if (Chance == 1 && ChanceTwo == 1) {UI.text = "Landed a critical hit and bewildered the Enemy! The Enemy took " + (int) Damage + " Damage from the Dizzy Attack!";}
            if (Chance != 1 && ChanceTwo != 1) {UI.text = "The Enemy took " + (int) Damage + " Damage from the Dizzy Attack!";}
            Debug.Log("Enemy took" + Damage + "damage");
            Damage = 0;
        }
        if (MoveABCD == 7)
        {
            //Debug.Log("Player Move 8");REDSWORD
            Damage = (pstat.Power - estat.Defense/2) + 3;
            Chance = Random.Range(0,12);

            if (Chance == 1) {Damage = Damage * 1.5f; UI.text = "The Enemy took " + Damage + " Damage from the Ice Attack!";; Chance = 0;}
            if (Damage <= 1) {Damage = 1;}
            estat.Health = estat.Health - (int) Damage;
            if (Chance == 1) {UI.text = "The Player landed a critical hit! The Enemy took " + (int) Damage + " Damage from the Strong Attack!";}
            if (Chance != 1) {UI.text = "The Enemy took " + (int) Damage + " Damage from the Strong Attack!";}
            Debug.Log("Enemy took" + Damage + "damage");
            Damage = 0;
        }
        if (MoveABCD == 8)
        {
            //Debug.Log("Player Move 9");
            Chance = Random.Range(2,7);
            Health = (int) Health + (int) Chance;
            UI.text = "The Player recovered " + Chance + " Health from Random Heal!";
        }
        if (MoveABCD == 9)
        {
            //Debug.Log("Player Move 10");
            Health = Health + 5;
            UI.text = "The Player recovered " + 5 + " Health from Consistent Heal!";

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
            if (Damage <= 1) {Damage = 1;}
            Health = Health - (int) Damage;
            if (Chance == 1) {UI.text = "The Enemy landed a critical hit! The Player took " + (int) Damage + " Damage from the Enemy Attack!";}
            if (Chance != 1) {UI.text = "The Player took " + (int) Damage + " Damage from the Enemy Attack!";}
            Debug.Log("Player took" + Damage + "damage");
            Damage = 0;
        }
        if (EnemyChoice == 5)
        {
            UI.text = "The Player had their Power cut in half by the Enemy's curse, lasting for 3 turns!";
            Debug.Log("Enemy lowered Player Power");
            pstat.PowerD = true;
        }
        if (EnemyChoice == 6)
        {
            UI.text = "The Player had their Defense cut in half by the Enemy's curse, lasting for 3 turns!";
            Debug.Log("Enemy lowered Player Defense");
            pstat.DefenseD = true;
        }
        if (EnemyChoice == 7)
        {
            UI.text = "The Player had their Speed cut in half by the Enemy's curse, lasting for 3 turns!";
            Debug.Log("Enemy lowered Player Speed");
            pstat.SpeedD = true;
        }
        if (EnemyChoice == 8)
        {
            Debug.Log("Defense Ignorant Move");
            Damage = estat.Power - pstat.Defense/3;
            if (Damage <= 1) {Damage = 1;}
            Health = Health - (int) Damage;
            if (Chance == 1) {UI.text = "The Enemy landed a critical hit! The Player took " + (int) Damage + " Damage from the Enemies Defense Ignorant Attack!";}
            if (Chance != 1) {UI.text = "The Player took " + (int) Damage + " Damage from the Enemies Defense Ignorant Attack!";}
            Debug.Log("Player took" + Damage + "damage");
            Damage = 0;
        }
        if (EnemyChoice == 9)
        {
            UI.text = "The Enemy healed themselves by " + estat.Health + " Health!";
            Debug.Log("Enemy Move 9");
            estat.Health = estat.Health + 4;
        }
    }

    //PLAYER GETS A NEW MOVE
    void GainingMove()
    {
        MoveSelect = true;
        MenuSelect = false;
        Instantiate(Screens[1], transform);
        MoveSummon();
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
        //This summons the UI blocks AND the Conditions!
        Instantiate(UIBlock[1], transform);
        Instantiate(UIBlock[2], transform);
        //This summons the enemy on scene, EnemyManager is assigned "e" stats
        EnemyManager = Instantiate(Enemies[Random.Range(0, Enemies.Count)], transform);
        estat = EnemyManager.GetComponent<EnemyStats>();
        //Player summon, player is assigned "p" stats
        Instantiate(Plushes[pstat.PlushChoice], transform);
        //Button Summons
        ActiveButtons[0] = Instantiate(Buttons[ButtonA], new Vector3(-8.1f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[1] = Instantiate(Buttons[ButtonB], new Vector3(-6.7f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[2] = Instantiate(Buttons[ButtonC], new Vector3(-5.3f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[3] = Instantiate(Buttons[ButtonD], new Vector3(-3.9f, -0.75f, 0), Quaternion.identity, transform);
        hm.UpdateHearts(Health);
        MenuStart();
    }

    void Destruction()
    {
        Health = 16;
        pstat.ResetBool = true;
        TopUI.text = "";
        pstat.PlushSelect = false;
        MenuSummon = false;
            Debug.Log("Battle Manager Cleared");
        for (int i = 0; i < transform.childCount; i++)
            {Destroy(transform.GetChild(i).gameObject);}
    }

    void DisplayMove()
    {
        if (MoveABCD == 0) {UI.text = "Damage the Enemy, compares your full Power stat against half of their Defense stat!";}
        if (MoveABCD == 1) {UI.text = "Double your Power stat for 3 turns, it starts counting down at the end of this turn!";}
        if (MoveABCD == 2) {UI.text = "Double your Defense stat for 3 turns, it starts counting down at the end of this turn!";}
        if (MoveABCD == 3) {UI.text = "Double your Speed stat for 3 turns, it starts counting down at the end of this turn!";}
        if (MoveABCD == 4) {UI.text = "Damage the Enemy, you have a chance of burning them to half their Defense stat!";}
        if (MoveABCD == 5) {UI.text = "Damage the Enemy, you have a chance of freezing them to half their Speed stat!";}
        if (MoveABCD == 6) {UI.text = "Damage the Enemy, you have a chance of bewildering them to half their Power stat!";}
        if (MoveABCD == 7) {UI.text = "Damage the Enemy, much stronger on average with a lower critical hit chance!";}
        if (MoveABCD == 8) {UI.text = "Heal yourself between 2-7 Health, it's random every time you utilize this move!";}
        if (MoveABCD == 9) {UI.text = "Heal yourself 5 Health on each use, try not to get out-damaged by the Enemy while healing!";}
    }
    void StartingSummon()
    {
        pstat.PlushSelect = true;
        TopUI.text = "Plush Select (A: Left, D: Right) When you're ready, press (E) to start!";
        Instantiate(UIBlock[0], transform);
        Instantiate(UIBlock[1], transform);
        Instantiate(UIBlock[2], transform);
    }
    public IEnumerator Starting()
    {yield return new WaitForSeconds(1.5f); Destruction(); MenuSelect = true; Summon();}
    public IEnumerator MoveStarting()
    {estat.EnemyDefeat = false; yield return new WaitForSeconds(1.5f); Destruction(); MenuSelect = true; MoveSummon();}

    void MoveSummon()
    {

        Instantiate(UIBlock[0], transform);
        Instantiate(UIBlock[1], transform);
        Instantiate(UIBlock[2], transform);

        ActiveButtons[0] = Instantiate(Buttons[ButtonA], new Vector3(-8.1f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[1] = Instantiate(Buttons[ButtonB], new Vector3(-6.7f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[2] = Instantiate(Buttons[ButtonC], new Vector3(-5.3f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[3] = Instantiate(Buttons[ButtonD], new Vector3(-3.9f, -0.75f, 0), Quaternion.identity, transform);
        hm.UpdateHearts(Health);
    }
}
