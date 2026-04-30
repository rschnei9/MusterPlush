using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Status Conditions")]
    public GameObject[] Condition;

    [Header("Player Statistics")]
    public bool PlushSelect;

    public float BASEPower;
    public float BASEDefense;
    public float BASESpeed;

    public float Power;
    public float Defense;
    public float Speed;

    public int PlushChoice;

    [Header("Player Management")]
    public bool PowerU;
    public bool PowerD;
    public bool DefenseU;
    public bool DefenseD;
    public bool SpeedU;
    public bool SpeedD;

    public int TimePowerU;
    public int TimePowerD;
    public int TimeDefenseU;
    public int TimeDefenseD;
    public int TimeSpeedU;
    public int TimeSpeedD;

    public bool TurnEnd;

    public int PUP;
    public int PDOWN;
    public int DUP;
    public int DDOWN;
    public int SUP;
    public int SDOWN;


    void Start()
    {
        PlushChoice = 0;
            BASEPower = 5;
            BASEDefense = 2;
            BASESpeed = 9;
            Refresh();
        PlushSelect = true;
    }

    void Update()
    {
        //Cat Selected
        if (Input.GetKeyDown(KeyCode.I) && PlushSelect == true)
        {
            PlushChoice = 0;
            BASEPower = 5;
            BASEDefense = 2;
            BASESpeed = 9;
            Refresh();
        }
        //Bear Selected
        if (Input.GetKeyDown(KeyCode.O) && PlushSelect == true)
        {
            PlushChoice = 1;
            BASEPower = 6;
            BASEDefense = 7;
            BASESpeed = 3;
            Refresh();
        }
        //Bunny Selected
        if (Input.GetKeyDown(KeyCode.P) && PlushSelect == true)
        {
            PlushChoice = 2;
            BASEPower = 4;
            BASEDefense = 5;
            BASESpeed = 7;
            Refresh();
        }
        //When the turn ends
        if (TurnEnd == true)
        {
            TurnEnd = false; 
            TimePowerU -= 1; TimeDefenseU -= 1; TimeSpeedU -= 1; TimePowerD -= 1; TimeDefenseD -= 1; TimeSpeedD -= 1;
            Refresh();
        }

        //Bool Move Triggered Functions!
        if (PowerU == true)
        { PowerU = false; PowerUP(); }
        if (DefenseU == true)
        { DefenseU = false; DefenseUP(); }
        if (SpeedU == true)
        { SpeedU = false; SpeedUP(); }
        if (PowerD == true)
        { PowerD = false; PowerDOWN(); }
        if (DefenseD == true)
        { DefenseD = false; DefenseDOWN(); }
        if (SpeedD == true)
        { SpeedD = false; SpeedDOWN (); }
    }

    void PowerUP()
    {Debug.Log("Player Power Doubled"); TimePowerU = 4; PUP = 2; Refresh();}
    void DefenseUP()
    {Debug.Log("Player Defense Doubled"); TimeDefenseU = 4; DUP = 2; Refresh();}
    void SpeedUP()
    {Debug.Log("Player Speed Doubled"); TimeSpeedU = 4; SUP = 2; Refresh();}
    void PowerDOWN()
    {Debug.Log("Player Power Halved"); TimePowerD = 4; PDOWN = 2; Refresh();}
    void DefenseDOWN()
    {Debug.Log("Player Defense Halved"); TimeDefenseD = 4; DDOWN = 2; Refresh();}
    void SpeedDOWN()
    {Debug.Log("Player Speed Halved"); TimeSpeedD = 4; SDOWN = 2; Refresh();}

    void Refresh()
    {
        if (TimePowerU <= 0){TimePowerU = 0; PUP = 1;}
        if (TimeDefenseU <= 0){TimeDefenseU = 0; DUP = 1;}
        if (TimeSpeedU <= 0){TimeSpeedU = 0; SUP = 1;}
        if (TimePowerD <= 0){TimePowerD = 0; PDOWN = 1;}
        if (TimeDefenseD <= 0){TimeDefenseD = 0; DDOWN = 1;}
        if (TimeSpeedD <= 0){TimeSpeedD = 0; SDOWN = 1;}

        //for (int i = 0; i < transform.childCount; i++)
            //{Destroy(transform.GetChild(i).gameObject);}
        //if (TimePowerU > 0) {Instantiate(Condition[0], transform);}
        //if (TimeDefenseU > 0) {Instantiate(Condition[1], transform);}
        //if (TimeSpeedU > 0) {Instantiate(Condition[2], transform);}
        //if (TimePowerD > 0) {Instantiate(Condition[3], transform);}
        //if (TimeDefenseD > 0) {Instantiate(Condition[4], transform);}
        //if (TimeSpeedD > 0) {Instantiate(Condition[5], transform);}
        
        Power = BASEPower * PUP / PDOWN;
        Defense = BASEDefense * DUP / DDOWN;
        Speed = BASESpeed * SUP / SDOWN;
    }
}
