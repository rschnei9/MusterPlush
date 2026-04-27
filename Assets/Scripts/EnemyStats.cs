using UnityEngine;

public class EnemyStats : MonoBehaviour
{
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

    void Start()
    {
        BASEPower = 0;
        BASEDefense = 0;
        BASESpeed = 0;

        Health = Random.Range(12, 24);

        EnemyMoveA = Random.Range(1,4);
        EnemyMoveB = Random.Range(5,7);
        EnemyMoveC = Random.Range(8,9);
    }

    void Update()
    {
        if (Health <= 0)
        {
            EnemyDefeat = true;
        }
    }
}
