using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public GameObject[] Conditions;
    [Header("Enemy Statistics")]
    public float Power;
    public float Defense;
    public float Speed;

    public float BASEPower;
    public float BASEDefense;
    public float BASESpeed;

    public float Health;
    public bool EnemyDefeat;

    public int EnemyMoveA;
    public int EnemyMoveB;
    public int EnemyMoveC;

    [Header("Enemy Management")]
    public int Dizzy;
    public int Burning;
    public int Freeze;
    public int DizzyT;
    public int BurningT;
    public int FreezeT;

    public bool TurnEnd;
    public bool DizzyE;
    public bool BurningE;
    public bool FreezeE;

    void Start()
    {
        BASEPower = Power;
        BASEDefense = Defense;
        BASESpeed = Speed;
        Health = Random.Range(12, 24);

        EnemyMoveA = Random.Range(1,4);
        EnemyMoveB = Random.Range(5,7);
        EnemyMoveC = Random.Range(8,9);
        Refresh();
    }

    void Update()
    {
        if (TurnEnd == true)
        {
            TurnEnd = false; 
            DizzyT -= 1; BurningT -= 1; FreezeT -= 1;
            Refresh();
        }
        if (Health <= 0)
        {
            EnemyDefeat = true;
        }
        if (DizzyE == true) { DizzyE = false; SpinIT(); }
        if (BurningE == true) { BurningE = false; BurnIT(); }
        if (FreezeE == true) { FreezeE = false; FreezeIT(); }
    }

    void SpinIT()
    {Debug.Log("Enemy Dizzy!"); DizzyT = 4; Dizzy = 2; Refresh();}
    void BurnIT()
    {Debug.Log("Enemy Burned!"); BurningT = 4; Burning = 2; Refresh();}
    void FreezeIT()
    {Debug.Log("Enemy Freeze!"); FreezeT = 4; Freeze = 2; Refresh();}
    
    void Refresh()
    {
        if (DizzyT <= 0){DizzyT = 0; Dizzy = 1;}
        if (BurningT <= 0){BurningT = 0; Burning = 1;}
        if (FreezeT <= 0){FreezeT = 0; Freeze = 1;}

        Power = BASEPower / Dizzy;
        Defense = BASEDefense / Burning;
        Speed = BASESpeed / Freeze;
    }
}
