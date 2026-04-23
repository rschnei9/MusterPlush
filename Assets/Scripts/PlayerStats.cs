using UnityEngine;

public class PlayerStats : MonoBehaviour
{
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
    public int TimePowerU;
    public bool DefenseU;
    public int TimeDefenseU;
    public bool SpeedU;
    public int TimeSpeedU;

    void Start()
    {
        PlushChoice = 0;
        Power = 3;
        Defense = 1;
        Speed = 4;
        PlushSelect = true;
    }

    void Update()
    {
        //Cat Selected
        if (Input.GetKeyDown(KeyCode.I) && PlushSelect == true)
        {
            PlushChoice = 0;
            Power = 3;
            Defense = 1;
            Speed = 4;
        }
        //Bear Selected
        if (Input.GetKeyDown(KeyCode.O) && PlushSelect == true)
        {
            PlushChoice = 1;
            Power = 3;
            Defense = 4;
            Speed = 1;
        }
        //Bunny Selected
        if (Input.GetKeyDown(KeyCode.P) && PlushSelect == true)
        {
            PlushChoice = 2;
            Power = 2;
            Defense = 3;
            Speed = 3;
        }

        //Bool Triggered Functions!
        if (PowerU == true)
        { PowerU = false; PowerUP(); }
    }

    void PowerUP()
    {
        Debug.Log("Player Power Doubled");
        TimePowerU = 3;
        Power = Power * 2;
    }
}
