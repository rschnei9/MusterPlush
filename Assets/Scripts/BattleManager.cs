using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleManager : MonoBehaviour
{
    //REFERENCES
    public HeartManager hm;

    private PlayerStats pstat;
    private EnemyStats estat;
    public AudioManager a;
    public PlushRead plush;

    public GameObject PlayerManager;
    public GameObject EnemyManager;

    //THESE VARIABLES ARE FOR THE PLAYER
    [Header("Player Management")]
    public GameObject[] Plushes;
    public GameObject[] PlushFront;
    public float Player;
    public int Health;

    //THESE VARIABLES ARE FOR THE ENEMY
    [Header("Enemy Management")]
    public List<GameObject> Enemies;
    public float Enemy;
    public int EnemyChoice;
    public int ModP;
    public int ModD;
    public int ModS;
    public int HBuff;

    //THESE ARE FOR THE BATTLE UI
    [Header("User Interface")]
    public GameObject[] Buttons;
    public GameObject[] ActiveButtons;
    public GameObject[] Hearts;
    public bool Select;
    public bool BattleEnd;
    public bool Pause;

    public int ButtonA;
    public int ButtonB;
    public int ButtonC;
    public int ButtonD;
    public int MoveABCD;
    public int ButtonE;

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

    //THESE ARE FOR OTHER THINGS
    [Header("Display (UI)")]
    public GameObject[] UIBlock;
    public GameObject[] Condition;
    public GameObject[] Backgrounds;
    public GameObject[] Audio;

    public TextMeshProUGUI TopUI;
    public TextMeshProUGUI StatUI;
    public TextMeshProUGUI UI;
    public TextMeshProUGUI MoveUI;
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

    public bool PlushView;
    public bool DefeatMenu;

    void Start()
    {
        pstat = GetComponent<PlayerStats>();
        DefeatMenu = false;
        PlushView = false;
        MenuSelect = false;
        UI.text = "";
        StatUI.text = "";
        TopUI.text = "";
        MoveUI.text = "";
        //Instantiate(Backgrounds[4], transform);
        //ButtonA = 0; ButtonB = 1; ButtonC = 2; ButtonD = 3;
        //Defeat = false; MenuSummon = false; MenuSelect = true; Health = 16;
        StartCoroutine("Loading");
        //Destruction();
        //StartingSummon();
    }

    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.E) && DefeatMenu == true)
        {
            Instantiate(Audio[0], transform); StartCoroutine("UnLoading");
        }
        //Statistic Updates UI
        PowerUI.text =  "" + pstat.Power;
        DefenseUI.text =  "" + pstat.Defense;
        SpeedUI.text =  "" + pstat.Speed;
        if (PlushView == true)
        {plush.PlushFront = pstat.PlushChoice;}

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
            else {TurnTwo();}
        }
        if (MenuSummon && estat.EnemyDefeat && BattleEnd == false)
        {
            estat.EnemyDefeat = true;
            BattleEnd = true;
            Debug.Log("Enemy Defeated");
            if (FirstTurn == true)
            {StartCoroutine("TurnOneDoom");}
        }
        
        //TEMPORARY TEST FUNCTIONS
        //if (Input.GetKeyDown(KeyCode.Z) && MenuSummon == true) {Destruction();}
        //if (Input.GetKeyDown(KeyCode.X) && MenuSummon == false) {Summon();}
        //if (Input.GetKeyDown(KeyCode.Y)) { Health -= 1; }
        //if (Input.GetKeyDown(KeyCode.U)) { Health += 1; }
        //if (Input.GetKeyDown(KeyCode.T)) {Instantiate(Screens[0], transform);}

        //PLUSH SELECTION FUNCTIONS
        if (Input.GetKeyDown(KeyCode.E) && pstat.PlushSelect == true) {Instantiate(Audio[0], transform); Instantiate(Backgrounds[5], transform); pstat.PlushSelect = false; MenuSelect = false; StartCoroutine("Starting");}
        if (pstat.PlushSelect == true) {Chance = 0 + pstat.PlushChoice;}

        if (pstat.PlushSelect == true && Chance == 0) {UI.text = "Cat Plush! When you start, you won't be able to switch to a different plush until defeat!";}
        if (pstat.PlushSelect == true && Chance == 1) {UI.text = "Bear Plush! When you start, you won't be able to switch to a different plush until defeat!";}
        if (pstat.PlushSelect == true && Chance == 2) {UI.text = "Bunny Plush! When you start, you won't be able to switch to a different plush until defeat!";}

        //UI INDICATORS
        hm.UpdateHearts(Health);

        //MOVE SLIDER UI
            if (Input.GetKeyDown(KeyCode.D) && pstat.PlushSelect == true)
                {Instantiate(Audio[1], transform);}
            if (Input.GetKeyDown(KeyCode.A) && pstat.PlushSelect == true)
                {Instantiate(Audio[1], transform);}

            if (Input.GetKeyDown(KeyCode.D) && MenuSelect == true && MenuSummon == true)
                {Instantiate(Audio[1], transform); SelectMoveR();}
            if (Input.GetKeyDown(KeyCode.A) && MenuSelect == true && MenuSummon == true)
                {Instantiate(Audio[1], transform); SelectMoveL();}

            if (Input.GetKeyDown(KeyCode.D) && MoveSelect == true)
                {Instantiate(Audio[1], transform); SelectMoveR();}
            if (Input.GetKeyDown(KeyCode.A) && MoveSelect == true)
                {Instantiate(Audio[1], transform); SelectMoveL();}

            if (Input.GetKeyDown(KeyCode.D) && StatSelect == true)
                {Instantiate(Audio[1], transform); SelectStatR();}
            if (Input.GetKeyDown(KeyCode.A) && StatSelect == true)
                {Instantiate(Audio[1], transform); SelectStatL();}
            
            if (pstat.PlushRoll == true)
            {
                pstat.PlushRoll = false;
                if (pstat.PlushSelect == true)
                {
                    Destruction();
                    MenuSelect = true;
                    PlushView = true;
                    Instantiate(Backgrounds[1], transform);
                    pstat.PlushSelect = true;
                    TopUI.text = "Plush Select (A: Left, D: Right) When you're ready, press (E) to start!";
                    Instantiate(UIBlock[0], transform);
                    Instantiate(UIBlock[1], transform);
                    Instantiate(UIBlock[2], transform);
                    Instantiate(PlushFront[pstat.PlushChoice], transform);
                }
            }
            
        //MOVE SELECTION
        if (Input.GetKeyDown(KeyCode.E) && MenuSummon == true && MenuSelect == true && Defeat == false && pstat.PlushSelect == false && estat.EnemyDefeat == false)
        {MenuSelect = false; StartCoroutine("TurnStart"); Instantiate(Audio[0], transform);}

        //MOVE OVERRIDE WITH NEW
        if (Input.GetKeyDown(KeyCode.E) && MoveSelect == true && MoveABCD == ButtonA && pstat.PlushSelect == false) {ButtonA = ButtonE; MoveSelect = false; StartCoroutine("StatUpgrade");}
        if (Input.GetKeyDown(KeyCode.E) && MoveSelect == true && MoveABCD == ButtonB && pstat.PlushSelect == false) {ButtonB = ButtonE; MoveSelect = false; StartCoroutine("StatUpgrade");}
        if (Input.GetKeyDown(KeyCode.E) && MoveSelect == true && MoveABCD == ButtonC && pstat.PlushSelect == false) {ButtonC = ButtonE; MoveSelect = false; StartCoroutine("StatUpgrade");}
        if (Input.GetKeyDown(KeyCode.E) && MoveSelect == true && MoveABCD == ButtonD && pstat.PlushSelect == false) {ButtonD = ButtonE; MoveSelect = false; StartCoroutine("StatUpgrade");}
        if (Input.GetKeyDown(KeyCode.Q) && MoveSelect == true) {Instantiate(Audio[0], transform); MoveSelect = false; StartCoroutine("StatUpgrade");}

        //STAT UPGRADES WOOHOO!
        if (Input.GetKeyDown(KeyCode.E) && StatSelect == true) {Instantiate(Audio[0], transform); StatSelect = false; pstat.PowerU = false; pstat.DefenseU = false; pstat.SpeedU = false; StartCoroutine("StatConfirmed");}
    }

    //THIS IS THE TURN CALCULATOR, DETERMINING WHO MOVES FIRST AND SECOND.
    public IEnumerator TurnStart()
    {
        Debug.Log("Start of the turn!");
        PlayerMove = false;
        EnemyMove = false;
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
                {PlayerMove = true; Debug.Log("Player First!"); UI.text = "Speed tie! Player won coin toss.";}
            if (flipcoin == 1)
                {EnemyMove = true; Debug.Log("Enemy First!"); UI.text = "Speed tie! Enemy won coin toss.";}
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
            {StartCoroutine("PlayerMovePlan");}
        if (FirstTurn = true && EnemyMove == true && Defeat == false && estat.EnemyDefeat == false)
            {
                EnemyChoice = Random.Range(1,12);
                if (EnemyChoice <= 7){EnemyChoice = estat.EnemyMoveA;}
                if (EnemyChoice >= 8 && EnemyChoice <= 10){EnemyChoice = estat.EnemyMoveB;}
                if (EnemyChoice >= 11 && EnemyChoice <= 12){EnemyChoice = estat.EnemyMoveC;}
                {StartCoroutine("EnemyMovePlan");}
            }
        }
            yield return new WaitForSeconds(2f);
            FirstTurn = false; SecondTurn = true;
            StartCoroutine("TurnTwo");
    }
    public IEnumerator TurnTwo()
    {
        Damage = 0;
        if (BattleEnd == false)
        {
        if (SecondTurn = true && PlayerMove == false && Defeat == false && estat.EnemyDefeat == false)
            {StartCoroutine("PlayerMovePlan");}
        if (SecondTurn = true && EnemyMove == false && Defeat == false && estat.EnemyDefeat == false)
            {
                EnemyChoice = Random.Range(1,12);
                if (EnemyChoice <= 7){EnemyChoice = estat.EnemyMoveA;}
                if (EnemyChoice >= 8 && EnemyChoice <= 10){EnemyChoice = estat.EnemyMoveB;}
                if (EnemyChoice >= 11 && EnemyChoice <= 12){EnemyChoice = estat.EnemyMoveC;}
                {StartCoroutine("EnemyMovePlan");}
            }
        }
            yield return new WaitForSeconds(2f);
            SecondTurn = false;
            StartCoroutine("TurnEnd");
        
    }
    public IEnumerator TurnOneDoom() {yield return new WaitForSeconds(2f); Debug.Log("Turn 1 Doom"); StartCoroutine("TurnEnd");}
    public IEnumerator TurnEnd()
    {
        if (BattleEnd == false)
        {
            PlayerMove = false;
            EnemyMove = false;
            pstat.TurnEnd = true;
            estat.TurnEnd = true;
            yield return new WaitForSeconds(0.15f);
            MenuStart();
            Debug.Log("End of turn!");
        }
        if (estat.EnemyDefeat == true)
        {
            BattleEnd = true;
            Instantiate(Backgrounds[5], transform);
            UI.text = "The Enemy has been defeated!";
            Debug.Log("Player wins the fight");
            yield return new WaitForSeconds(2f);
            GainingMove();
        }
        if (Defeat == true)
        {
            BattleEnd = true;
            Defeat = false;
            Instantiate(Backgrounds[5], transform);
            UI.text = "The Player has been defeated!";
            Debug.Log("Player loses the fight");
            yield return new WaitForSeconds(2f);
            GameOver();
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
    void SelectStatL()
    {
        if (pstat.DUP == 2 && StatSelect == true) {ChanceTwo = 0; pstat.PUP = 2; pstat.DUP = 1; pstat.SUP = 1;}
        else if (pstat.SUP == 2 && StatSelect == true) {ChanceTwo = 1; pstat.PUP = 1; pstat.DUP = 2; pstat.SUP = 1;}
        else if (pstat.PUP == 2 && StatSelect == true) {ChanceTwo = 2; pstat.PUP = 1; pstat.DUP = 1; pstat.SUP = 2;}
    }
    void SelectStatR()
    {
        if (pstat.SUP == 2 && StatSelect == true) {ChanceTwo = 0; pstat.PUP = 2; pstat.DUP = 1; pstat.SUP = 1;}
        else if (pstat.PUP == 2 && StatSelect == true) {ChanceTwo = 1; pstat.PUP = 1; pstat.DUP = 2; pstat.SUP = 1;}
        else if (pstat.DUP == 2 && StatSelect == true) {ChanceTwo = 2; pstat.PUP = 1; pstat.DUP = 1; pstat.SUP = 2;}
    }
//EVERY POSSIBLE MOVE!
    public IEnumerator PlayerMovePlan()
    {
        if (MoveABCD == 0)
        {
            StartCoroutine("AnimatedPlayer");
            Chance = 50; ChanceTwo = 50;
            Damage = pstat.Power - estat.Defense/2;
            Chance = Random.Range(0,6);

            if (Chance == 1) {Damage = Damage * 1.5f;}
            if (Damage <= 1) {Damage = 1;}
            if (Chance == 1) {UI.text = "The Player landed a critical hit! The Enemy took " + (int) Damage + " Damage from the Basic Attack!";}
            if (Chance != 1) {UI.text = "The Enemy took " + (int) Damage + " Damage from the Basic Attack!";}

            yield return new WaitForSeconds(0.75f);
            estat.Health = estat.Health - (int) Damage;
            Damage = 0; Chance = 50; ChanceTwo = 50;
        }
        if (MoveABCD == 1)
        {
            StartCoroutine("AnimatedPlayer");
            Chance = 50; ChanceTwo = 50;
            UI.text = "The Player doubled their Power for 3 turns!";
            pstat.PowerU = true;
            yield return new WaitForSeconds(0.75f);
            Chance = 50; ChanceTwo = 50;
        }
        if (MoveABCD == 2)
        {
            StartCoroutine("AnimatedPlayer");
            Chance = 50; ChanceTwo = 50;
            UI.text = "The Player doubled their Defense for 3 turns!";
            pstat.DefenseU = true;
            yield return new WaitForSeconds(0.75f);
            Chance = 50; ChanceTwo = 50;
        }
        if (MoveABCD == 3)
        {
            StartCoroutine("AnimatedPlayer");
            Chance = 50; ChanceTwo = 50;
            UI.text = "The Player doubled their Speed for 3 turns!";
            pstat.SpeedU = true;
            yield return new WaitForSeconds(0.75f);
            Chance = 50; ChanceTwo = 50;
        }
        if (MoveABCD == 4)
        {
            StartCoroutine("AnimatedPlayer");
            Chance = 50; ChanceTwo = 50;
            Damage = pstat.Power - estat.Defense/2;
            Chance = Random.Range(0,9);

            if (Chance == 1) {Damage = Damage * 1.5f; Debug.Log("A critical hit!");}
            if (Damage <= 1) {Damage = 1;}
            ChanceTwo = Random.Range(0,3);

            if (Chance == 1) {UI.text = "The Player landed a critical hit! The Enemy took " + (int) Damage + " Damage from the Fire Attack!";}
            if (ChanceTwo == 1) {UI.text = "The Player burned the Enemy! The Enemy took " + (int) Damage + " Damage from the Fire Attack";}
            if (Chance == 1 && ChanceTwo == 1) {UI.text = "Landed a critical hit and burned the Enemy! The Enemy took " + (int) Damage + " Damage from the Fire Attack!";}
            if (Chance != 1 && ChanceTwo != 1) {UI.text = "The Enemy took " + (int) Damage + " Damage from the Fire Attack!";}
            
            yield return new WaitForSeconds(0.75f);
            estat.Health = estat.Health - (int) Damage;
            if (ChanceTwo == 1) {estat.BurningE = true;}
            Damage = 0; Chance = 50; ChanceTwo = 50;
        }
        if (MoveABCD == 5)
        {
            StartCoroutine("AnimatedPlayer");
            Chance = 50; ChanceTwo = 50;
            Damage = pstat.Power - estat.Defense/2;
            Chance = Random.Range(0,9);

            if (Chance == 1) {Damage = Damage * 1.5f;}
            if (Damage <= 1) {Damage = 1;}
            ChanceTwo = Random.Range(0,3);

            if (Chance == 1) {UI.text = "The Player landed a critical hit! The Enemy took " + (int) Damage + " Damage from the Ice Attack!";}
            if (ChanceTwo == 1) {UI.text = "The Player froze the Enemy! The Enemy took " + (int) Damage + " Damage from the Ice Attack";}
            if (Chance == 1 && ChanceTwo == 1) {UI.text = "Landed a critical hit and froze the Enemy! The Enemy took " + (int) Damage + " Damage from the Ice Attack!";}
            if (Chance != 1 && ChanceTwo != 1) {UI.text = "The Enemy took " + (int) Damage + " Damage from the Ice Attack!";}
            
            yield return new WaitForSeconds(0.75f);
            estat.Health = estat.Health - (int) Damage;
            if (ChanceTwo == 1) {estat.FreezeE = true;}
            Damage = 0; Chance = 50; ChanceTwo = 50;
        }
        if (MoveABCD == 6)
        {
            StartCoroutine("AnimatedPlayer");
            Chance = 50; ChanceTwo = 50;
            Damage = pstat.Power - estat.Defense/2;
            Chance = Random.Range(0,9);

            if (Chance == 1) {Damage = Damage * 1.5f;}
            if (Damage <= 1) {Damage = 1;}
            ChanceTwo = Random.Range(0,3);

            if (Chance == 1) {UI.text = "The Player landed a critical hit! The Enemy took " + (int) Damage + " Damage from the Dizzy Attack!";}
            if (ChanceTwo == 1) {UI.text = "The Player bewildered the Enemy! The Enemy took " + (int) Damage + " Damage from the Dizzy Attack";}
            if (Chance == 1 && ChanceTwo == 1) {UI.text = "Landed a critical hit and bewildered the Enemy! The Enemy took " + (int) Damage + " Damage from the Dizzy Attack!";}
            if (Chance != 1 && ChanceTwo != 1) {UI.text = "The Enemy took " + (int) Damage + " Damage from the Dizzy Attack!";}

            yield return new WaitForSeconds(0.75f);
            estat.Health = estat.Health - (int) Damage;
            if (ChanceTwo == 1) {estat.DizzyE = true;}
            Damage = 0; Chance = 50; ChanceTwo = 50;
        }
        if (MoveABCD == 7)
        {
            StartCoroutine("AnimatedPlayer");
            Chance = 50; ChanceTwo = 50;
            Damage = (pstat.Power - estat.Defense/2) + 3;
            Chance = Random.Range(0,12);

            if (Chance == 1) {Damage = Damage * 1.5f;}
            if (Damage <= 1) {Damage = 1;}
            if (Chance == 1) {UI.text = "The Player landed a critical hit! The Enemy took " + (int) Damage + " Damage from the Strong Attack!";}
            if (Chance != 1) {UI.text = "The Enemy took " + (int) Damage + " Damage from the Strong Attack!";}

            yield return new WaitForSeconds(0.75f);
            estat.Health = estat.Health - (int) Damage;
            Damage = 0;
            Chance = 50; ChanceTwo = 50;
        }
        if (MoveABCD == 8)
        {
            StartCoroutine("AnimatedPlayer");
            Chance = 50; ChanceTwo = 50;
            Chance = Random.Range(2,8);
            Health = (int) Health + (int) Chance;
            UI.text = "The Player recovered " + Chance + " Health from Random Heal!";
            yield return new WaitForSeconds(0.75f);
            Chance = 50; ChanceTwo = 50;
        }
        if (MoveABCD == 9)
        {
            StartCoroutine("AnimatedPlayer");
            Chance = 50; ChanceTwo = 50;
            Health = Health + 5;
            UI.text = "The Player recovered " + 5 + " Health from Consistent Heal!";
            yield return new WaitForSeconds(0.75f);
            Chance = 50; ChanceTwo = 50;
        }
    }

//ENEMY MOVE SELECTION
    public IEnumerator EnemyMovePlan()
    {
        if (EnemyChoice == 1 || EnemyChoice == 2 || EnemyChoice == 3 || EnemyChoice == 4)
        {
            StartCoroutine("AnimatedEnemy");
            Chance = 50; ChanceTwo = 50;
            Damage = estat.Power - pstat.Defense/2;
            Chance = Random.Range(0,12);

            if (Chance == 1) {Damage = Damage * 1.5f;}
            if (Damage <= 1) {Damage = 1;}
            if (Chance == 1) {UI.text = "The Enemy landed a critical hit! The Player took " + (int) Damage + " Damage from the Enemy Attack!";}
            if (Chance != 1) {UI.text = "The Player took " + (int) Damage + " Damage from the Enemy Attack!";}

            yield return new WaitForSeconds(0.75f);
            Health = Health - (int) Damage;
            Damage = 0; Chance = 50; ChanceTwo = 50;
        }
        if (EnemyChoice == 5)
        {
            StartCoroutine("AnimatedEnemy");
            Chance = 50;
            UI.text = "The Player had their Power cut in half by the Enemy's curse, lasting for 3 turns!";

            pstat.PowerD = true;
            yield return new WaitForSeconds(0.75f);
            Chance = 50; ChanceTwo = 50;
        }
        if (EnemyChoice == 6)
        {
            StartCoroutine("AnimatedEnemy");
            Chance = 50;
            UI.text = "The Player had their Defense cut in half by the Enemy's curse, lasting for 3 turns!";

            pstat.DefenseD = true;
            yield return new WaitForSeconds(0.75f);
            Chance = 50; ChanceTwo = 50;
        }
        if (EnemyChoice == 7)
        {
            StartCoroutine("AnimatedEnemy");
            Chance = 50; ChanceTwo = 50;
            UI.text = "The Player had their Speed cut in half by the Enemy's curse, lasting for 3 turns!";

            pstat.SpeedD = true;
            yield return new WaitForSeconds(0.75f);
            Chance = 50; ChanceTwo = 50;
        }
        if (EnemyChoice == 8)
        {
            StartCoroutine("AnimatedEnemy");
            Chance = 50; ChanceTwo = 50;
            Chance = Random.Range(0,16);

            Damage = estat.Power - pstat.Defense/3;
            if (Damage <= 1) {Damage = 1;}
            if (Chance == 1) {UI.text = "The Enemy landed a critical hit! The Player took " + (int) Damage + " Damage from the Enemies Defense Ignorant Attack!";}
            if (Chance != 1) {UI.text = "The Player took " + (int) Damage + " Damage from the Enemies Defense Ignorant Attack!";}
            yield return new WaitForSeconds(0.75f);
            Health = Health - (int) Damage;
            Damage = 0; Chance = 50; ChanceTwo = 50;
        }
        if (EnemyChoice == 9)
        {
            StartCoroutine("AnimatedEnemy");
            Chance = 50; ChanceTwo = 50;
            Chance = Random.Range(3,7);
            estat.Health = estat.Health + Chance;
            UI.text = "The Enemy healed themselves by " + Chance + " Health!";

            yield return new WaitForSeconds(0.75f);
            Chance = 50; ChanceTwo = 50;
        }
    }

    //PLAYER GETS A NEW MOVE
    void GainingMove()
    {
        if (MoveSelect == false)
        {Destruction(); a.Destroyer= true; MenuSelect = true; MoveSummon();}
    }
    //DOOM, MUAHAHAHAHHAHAHA!
    void GameOver()
    {
        a.Destroyer = true;
        PlushView = true;
        DefeatMenu = true;
        Instantiate(PlushFront[pstat.PlushChoice], new Vector3(0, -1f, 0), Quaternion.identity, transform);
        Instantiate(Backgrounds[2], transform);
        a.DefeatOn = true;
        
    }
    //THIS IS FOR SUMMONING BATTLE UI AND TAKING IT DOWN
    void Summon()
    {
        if (ButtonA == 1 || ButtonA == 2 || ButtonA == 3 || ButtonA == 8 || ButtonA == 9)
        {if (ButtonB == 1 || ButtonB == 2 || ButtonB == 3 || ButtonB == 8 || ButtonB == 9)
        {if (ButtonC == 1 || ButtonC == 2 || ButtonC == 3 || ButtonC == 8 || ButtonC == 9)
        {if (ButtonC == 1 || ButtonC == 2 || ButtonC == 3 || ButtonC == 8 || ButtonC == 9)
        {if (ButtonC == 1 || ButtonC == 2 || ButtonC == 3 || ButtonC == 8 || ButtonC == 9)
        {if (ButtonD == 1 || ButtonD == 2 || ButtonD == 3 || ButtonD == 8 || ButtonD == 9)
        {ButtonA = 0;
        }}}}}}
        Instantiate(Backgrounds[0], transform);
        Instantiate(Backgrounds[6], transform);
        a.BattleOn = true;
        MenuSummon = true;
        pstat.PlushSelect = false;
        BattleEnd = false;
        FirstTurn = false;
        SecondTurn = false;
        //This summons the UI blocks AND the Conditions!
        Instantiate(UIBlock[1], transform);
        Instantiate(UIBlock[2], transform);
        //This summons the enemy on scene, EnemyManager is assigned "e" stats
        EnemyManager = Instantiate(Enemies[Random.Range(0, Enemies.Count)], transform);
        estat = EnemyManager.GetComponent<EnemyStats>();
        //Player summon, player is assigned "p" stats
        PlayerManager = Instantiate(Plushes[pstat.PlushChoice], transform);
        //Button Summons
        ActiveButtons[0] = Instantiate(Buttons[ButtonA], new Vector3(-8.1f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[1] = Instantiate(Buttons[ButtonB], new Vector3(-6.7f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[2] = Instantiate(Buttons[ButtonC], new Vector3(-5.3f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[3] = Instantiate(Buttons[ButtonD], new Vector3(-3.9f, -0.75f, 0), Quaternion.identity, transform);
        hm.UpdateHearts(Health);
        pstat.TurnEnd = false;
        estat.TurnEnd = false;

        estat.BASEPower += ModP;
        estat.BASEDefense += ModD;
        estat.BASESpeed += ModS;
        estat.Power += ModP;
        estat.Defense += ModD;
        estat.Speed += ModS;
        estat.HealthMax = Health + HBuff;
        estat.Health += HBuff;
        estat.RoundRefresh = true;
        MenuStart();
    }

    void Destruction()
    {
        PlushView = false;
        Health = 16;
        pstat.ResetBool = true;
        pstat.PUP = 1; pstat.PDOWN = 1;
        pstat.DUP = 1; pstat.DDOWN = 1;
        pstat.SUP = 1; pstat.SDOWN = 1;
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
        PlushView = true;
        a.ChillOn = true;
        StatUI.text = "Plush Select";
        Instantiate(PlushFront[pstat.PlushChoice], transform); //PROBLEM WITH THIS GUY <<<<<
        Instantiate(Backgrounds[6], transform);
        Instantiate(Backgrounds[1], transform);
        pstat.PlushSelect = true;
        TopUI.text = "Select a plush (A: Left, D: Right). When you're ready, press (E) to start!";
        Instantiate(UIBlock[0], transform);
        Instantiate(UIBlock[1], transform);
        Instantiate(UIBlock[2], transform);
    }
    public IEnumerator Starting()
    {yield return new WaitForSeconds(1.5f); Destruction(); a.Destroyer = true; StatUI.text = ""; MenuSelect = true; Summon();}
    void MoveSummon()
    {
        a.ChillOn = true;
        Instantiate(Backgrounds[6], transform);
        ButtonE = Random.Range(1,10);
        if (ButtonE == ButtonA || ButtonE == ButtonB || ButtonE == ButtonC || ButtonE == ButtonD) {ButtonE = Random.Range(1,10);}
        if (ButtonE == ButtonA || ButtonE == ButtonB || ButtonE == ButtonC || ButtonE == ButtonD) {ButtonE = Random.Range(1,10);}
        if (ButtonE == ButtonA || ButtonE == ButtonB || ButtonE == ButtonC || ButtonE == ButtonD) {ButtonE = Random.Range(1,10);}
        if (ButtonE == ButtonA || ButtonE == ButtonB || ButtonE == ButtonC || ButtonE == ButtonD) {ButtonE = Random.Range(1,10);}
        if (ButtonE == ButtonA || ButtonE == ButtonB || ButtonE == ButtonC || ButtonE == ButtonD) {ButtonE = Random.Range(1,10);}
        if (ButtonE == ButtonA || ButtonE == ButtonB || ButtonE == ButtonC || ButtonE == ButtonD) {ButtonE = 0;}
        else {MoveSelect = true; MoveSelection();}
    }
    void MoveSelection()
    {
        TopUI.text = "(A) Left (D) Right. Press (E) to replace a move, or (Q) to not replace.";
        MoveUI.text = "Your new move!";
        Instantiate(Backgrounds[1], transform);
        Instantiate(UIBlock[0], transform);
        Instantiate(UIBlock[1], transform);
        Instantiate(UIBlock[2], transform);
        Instantiate(UIBlock[3], transform);

        if (ButtonE == 0) {MoveUI.text = "Damage the Enemy, compares your full Power stat against half of their Defense stat!";}
        if (ButtonE == 1) {MoveUI.text = "Double your Power stat for 3 turns, it starts counting down at the end of this turn!";}
        if (ButtonE == 2) {MoveUI.text = "Double your Defense stat for 3 turns, it starts counting down at the end of this turn!";}
        if (ButtonE == 3) {MoveUI.text = "Double your Speed stat for 3 turns, it starts counting down at the end of this turn!";}
        if (ButtonE == 4) {MoveUI.text = "Damage the Enemy, you have a chance of burning them to half their Defense stat!";}
        if (ButtonE == 5) {MoveUI.text = "Damage the Enemy, you have a chance of freezing them to half their Speed stat!";}
        if (ButtonE == 6) {MoveUI.text = "Damage the Enemy, you have a chance of bewildering them to half their Power stat!";}
        if (ButtonE == 7) {MoveUI.text = "Damage the Enemy, much stronger on average with a lower critical hit chance!";}
        if (ButtonE == 8) {MoveUI.text = "Heal yourself between 2-7 Health, it's random every time you utilize this move!";}
        if (ButtonE == 9) {MoveUI.text = "Heal yourself 5 Health on each use, try not to get out-damaged by the Enemy while healing!";}

        Instantiate(Buttons[ButtonE], new Vector3(0f, 1f, 0), Quaternion.identity, transform);
        ActiveButtons[0] = Instantiate(Buttons[ButtonA], new Vector3(-8.1f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[1] = Instantiate(Buttons[ButtonB], new Vector3(-6.7f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[2] = Instantiate(Buttons[ButtonC], new Vector3(-5.3f, -0.75f, 0), Quaternion.identity, transform);
        ActiveButtons[3] = Instantiate(Buttons[ButtonD], new Vector3(-3.9f, -0.75f, 0), Quaternion.identity, transform);
        Debug.Log("Started Hovering Button A"); ActiveButtons[0].GetComponent<MoveUpdate>().Hover(); MoveABCD = ButtonA; ActiveButtons[1].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[2].GetComponent<MoveUpdate>().UnHover(); ActiveButtons[3].GetComponent<MoveUpdate>().UnHover();
        DisplayMove();
    }
    public IEnumerator StatUpgrade()
    {
        yield return new WaitForSeconds(0.15f);
        Destruction();
        Instantiate(Audio[0], transform);
        MoveUI.text = "";
        TopUI.text = "(A) Left (D) Right. Choose Power, Defense, or Speed, then press (E).";
        UI.text = "Gain a +2 boost to a stat of your choice. A random stat will be selected to gain +1.";
        StatUI.text = "Stat Select";
        StatSelect = true;
        Instantiate(Backgrounds[1], transform);
        Instantiate(PlushFront[pstat.PlushChoice], transform);
        Instantiate(UIBlock[0], transform);
        Instantiate(UIBlock[1], transform);
        Instantiate(UIBlock[2], transform);
        yield return new WaitForSeconds(0.05f);
        ChanceTwo = 0; pstat.PUP = 2;
    }
    public IEnumerator StatConfirmed()
    {
        StatSelect = false;
        Instantiate(Backgrounds[5], transform);
        TopUI.text = "All selected, loading the next battle!";
        if (ChanceTwo == 0) {pstat.BASEPower += 2;}
        if (ChanceTwo == 1) {pstat.BASEDefense += 2;}
        if (ChanceTwo == 2) {pstat.BASESpeed += 2;}
        yield return new WaitForSeconds(0.05f); Chance = Random.Range(0,3); yield return new WaitForSeconds(0.05f);
        if (Chance == 0) {pstat.BASEPower += 1;}
        if (Chance == 1) {pstat.BASEDefense += 1;}
        if (Chance == 2) {pstat.BASESpeed += 1;}
        yield return new WaitForSeconds(0.05f); ChanceTwo = Random.Range(0,3); yield return new WaitForSeconds(0.05f);
        pstat.ResetBool = true;
        if (ChanceTwo == 0) {ModP += 2;}
        if (ChanceTwo == 1) {ModD += 2;}
        if (ChanceTwo == 2) {ModS += 2;}
        yield return new WaitForSeconds(0.05f); Chance = Random.Range(0,3);
        if (Chance == ChanceTwo) Chance = Random.Range(0,3);
        if (Chance == ChanceTwo) Chance = Random.Range(0,3);
        if (Chance == ChanceTwo) Chance = Random.Range(0,3);
        if (Chance == ChanceTwo) Chance = Random.Range(0,3);
        if (Chance == ChanceTwo) Chance = Random.Range(0,3);
        if (Chance == ChanceTwo) Chance = Random.Range(0,3);
        if (Chance == ChanceTwo) Chance = Random.Range(0,3);
        if (Chance == ChanceTwo) Chance = Random.Range(0,3);
        yield return new WaitForSeconds(0.05f);
        if (Chance == 0) {ModP += 1;}
        if (Chance == 1) {ModD += 1;}
        if (Chance == 2) {ModS += 1;}
        yield return new WaitForSeconds(0.05f);
        HBuff = HBuff + 1;
        pstat.ResetBool = true;
        MenuSelect = false;
        yield return new WaitForSeconds(0.15f);
        StartCoroutine("Starting");
    }
    public IEnumerator Loading()
    {
        Instantiate(Backgrounds[4], transform);
        Instantiate(Backgrounds[6], transform);
        ButtonA = 0; ButtonB = 1; ButtonC = 2; ButtonD = 3;
        Defeat = false; MenuSummon = false; MenuSelect = true; Health = 16;
        yield return new WaitForSeconds(2f);
        Instantiate(Backgrounds[5], transform);
        yield return new WaitForSeconds(2f);
        Destruction();
        StartingSummon();
    }
    public IEnumerator UnLoading()
    {
        Instantiate(Backgrounds[5], transform);
        yield return new WaitForSeconds(2f);
        string MusterPlush = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(MusterPlush);
    }
    public IEnumerator AnimatedPlayer()
    {
        yield return new WaitForSeconds(0.05f);
        if (MoveABCD == 0)
        {
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.52f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.55f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.3f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -1, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 1, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 1.1f, 0); EnemyManager.transform.position = new Vector3(0, 2.77f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 1.2f, 0); EnemyManager.transform.position = new Vector3(0.1f, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 1.2f, 0); EnemyManager.transform.position = new Vector3(-0.1f, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 1.1f, 0); EnemyManager.transform.position = new Vector3(0, 2.75f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 0.75f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 0f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -1f, 0);
            yield return new WaitForSeconds(0.05f);
            PlayerManager.transform.position = new Vector3(0, -2.5f, 0);
        }
        if (MoveABCD == 1)
        {
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.4f, -2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.4f, -2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.4f, -2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.4f, -2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f);
            PlayerManager.transform.position = new Vector3(0, -2.5f, 0);
        }
        if (MoveABCD == 2)
        {
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.4f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.3f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.1f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.3f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.4f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.4f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.3f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.1f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.3f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.4f, 0);
            yield return new WaitForSeconds(0.05f);
            PlayerManager.transform.position = new Vector3(0, -2.5f, 0);
        }
        if (MoveABCD == 3)
        {
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.3f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.3f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.3f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.7f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.3f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.3f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.7f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.3f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.5f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.9f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.5f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.5f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.5f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.9f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.3f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.3f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f);
            PlayerManager.transform.position = new Vector3(0, -2.5f, 0);
        }
        if (MoveABCD == 4)
        {
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.4f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.3f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.3f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, 1, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, 1.1f, 0); EnemyManager.transform.position = new Vector3(0, 2.77f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, 1.2f, 0); EnemyManager.transform.position = new Vector3(0.3f, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, 1.2f, 0); EnemyManager.transform.position = new Vector3(-0.3f, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, 1.1f, 0); EnemyManager.transform.position = new Vector3(0, 2.75f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 0.75f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 0f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -1f, 0);
            yield return new WaitForSeconds(0.05f);
            PlayerManager.transform.position = new Vector3(0, -2.5f, 0);
        }
        if (MoveABCD == 5)
        {
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.3f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.55f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.3f, -2.55f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.7f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.3f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.3f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.7f, -2.65f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.3f, -2.65f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.5f, -2.65f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.9f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.5f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, 1, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, 1.1f, 0); EnemyManager.transform.position = new Vector3(0, 2.77f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, 1.2f, 0); EnemyManager.transform.position = new Vector3(0.3f, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.3f, 1.2f, 0); EnemyManager.transform.position = new Vector3(-0.3f, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.3f, 1.1f, 0); EnemyManager.transform.position = new Vector3(0, 2.75f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 0.75f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 0f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -1f, 0);
            yield return new WaitForSeconds(0.05f);
            PlayerManager.transform.position = new Vector3(0, -2.5f, 0);
        }
        if (MoveABCD == 6)
        {
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2.4f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.5f, -2.3f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.1f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.5f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.3f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2.4f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.5f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.5f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.3f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0f, 1, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0f, 1.2f, 0); EnemyManager.transform.position = new Vector3(0, 2.77f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0f, 1.1f, 0); EnemyManager.transform.position = new Vector3(0.3f, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0f, 1.2f, 0); EnemyManager.transform.position = new Vector3(-0.3f, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0f, 1.1f, 0); EnemyManager.transform.position = new Vector3(0, 2.75f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 0.75f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 0f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -1f, 0);
            yield return new WaitForSeconds(0.05f);
            PlayerManager.transform.position = new Vector3(0, -2.5f, 0);
        }
        if (MoveABCD == 7)
        {
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.3f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.7f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.3f, -2.55f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.5f, -2.55f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.9f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.5f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.5f, -2.65f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.5f, -2.65f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.9f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.3f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.3f, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 1, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 1.2f, 0); EnemyManager.transform.position = new Vector3(0, 2.77f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 1.3f, 0); EnemyManager.transform.position = new Vector3(0.8f, 2.95f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 1.1f, 0); EnemyManager.transform.position = new Vector3(-0.8f, 2.95f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, 0f, 0); EnemyManager.transform.position = new Vector3(-0.6f, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -1.5f, 0); EnemyManager.transform.position = new Vector3(0.6f, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.35f, 0); EnemyManager.transform.position = new Vector3(0, 2.75f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.5f, 0);
            yield return new WaitForSeconds(0.05f);
            PlayerManager.transform.position = new Vector3(0, -2.5f, 0);
        }
        if (MoveABCD == 8)
        {
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.8f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.6f, 0);
            yield return new WaitForSeconds(0.05f);
            PlayerManager.transform.position = new Vector3(0, -2.5f, 0);
        }
        if (MoveABCD == 9)
        {
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.4f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.3f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.2f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0.1f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.2f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(-0.1f, -2.2f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.3f, 0);
            yield return new WaitForSeconds(0.05f); PlayerManager.transform.position = new Vector3(0, -2.4f, 0);
            yield return new WaitForSeconds(0.05f);
            PlayerManager.transform.position = new Vector3(0, -2.5f, 0);
        }
    }
    public IEnumerator AnimatedEnemy()
    {
        yield return new WaitForSeconds(0.05f);
        if (EnemyChoice == 1 || EnemyChoice == 2 || EnemyChoice == 3 || EnemyChoice == 4)
        {
            Debug.Log("Enemy Attack");
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.77f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.79f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.81f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.1f, 2.81f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.1f, 2.81f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.1f, 2.81f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.1f, 2.81f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.81f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 1f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, -1f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, -2f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, -2.25f, 0); PlayerManager.transform.position = new Vector3(0, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, -2.35f, 0); PlayerManager.transform.position = new Vector3(0.1f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, -2.35f, 0); PlayerManager.transform.position = new Vector3(-0.1f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, -2f, 0); PlayerManager.transform.position = new Vector3(0, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, -1f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 0f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 1.5f, 0);
            yield return new WaitForSeconds(0.05f);
            EnemyManager.transform.position = new Vector3(0, 2.75f, 0);
        }
        if (EnemyChoice == 5 || EnemyChoice == 6 || EnemyChoice == 7)
        {
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.1f, 2.75f, 0); PlayerManager.transform.position = new Vector3(-01f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.1f, 2.75f, 0); PlayerManager.transform.position = new Vector3(0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.2f, 2.75f, 0); PlayerManager.transform.position = new Vector3(-0.2f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.2f, 2.75f, 0); PlayerManager.transform.position = new Vector3(0.2f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.3f, 2.75f, 0); PlayerManager.transform.position = new Vector3(-0.3f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.3f, 2.75f, 0); PlayerManager.transform.position = new Vector3(0.3f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.2f, 2.75f, 0); PlayerManager.transform.position = new Vector3(-0.2f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.2f, 2.75f, 0); PlayerManager.transform.position = new Vector3(0.2f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.1f, 2.75f, 0); PlayerManager.transform.position = new Vector3(-0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.1f, 2.75f, 0); PlayerManager.transform.position = new Vector3(0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.1f, 2.75f, 0); PlayerManager.transform.position = new Vector3(-0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.1f, 2.75f, 0); PlayerManager.transform.position = new Vector3(0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.1f, 2.75f, 0); PlayerManager.transform.position = new Vector3(-0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.1f, 2.75f, 0); PlayerManager.transform.position = new Vector3(0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.1f, 2.75f, 0); PlayerManager.transform.position = new Vector3(-0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.1f, 2.75f, 0); PlayerManager.transform.position = new Vector3(-0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.1f, 2.75f, 0); PlayerManager.transform.position = new Vector3(-0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.1f, 2.75f, 0); PlayerManager.transform.position = new Vector3(0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.1f, 2.75f, 0); PlayerManager.transform.position = new Vector3(0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.75f, 0); PlayerManager.transform.position = new Vector3(0.1f, -2.5f, 0);
            yield return new WaitForSeconds(0.05f);
            EnemyManager.transform.position = new Vector3(0, 2.75f, 0);
        }
        if (EnemyChoice == 8)
        {
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.8f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.8f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.1f, 2.7f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.1f, 2.8f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.1f, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.1f, 3.1f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.8f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 3.0f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.1f, 3.1f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.1f, 3.1f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.3f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, -2.25f, 0); PlayerManager.transform.position = new Vector3(0, -2.6f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(-0.1f, -2.35f, 0); PlayerManager.transform.position = new Vector3(0.1f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0.1f, -2.35f, 0); PlayerManager.transform.position = new Vector3(-0.1f, -2.7f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, -2f, 0); PlayerManager.transform.position = new Vector3(0, -2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, -1f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 0f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 1.5f, 0);
            yield return new WaitForSeconds(0.05f);
            EnemyManager.transform.position = new Vector3(0, 2.75f, 0);
        }
        if (EnemyChoice == 9)
        {
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.8f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.95f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.8f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.6f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.45f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.6f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.8f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.95f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.9f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.8f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.6f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.45f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.5f, 0);
            yield return new WaitForSeconds(0.05f); EnemyManager.transform.position = new Vector3(0, 2.6f, 0);
            yield return new WaitForSeconds(0.05f);
            EnemyManager.transform.position = new Vector3(0, 2.75f, 0);
        }
    }

}
