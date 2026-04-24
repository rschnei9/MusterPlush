using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float Power;
    public float Defense;
    public float Speed;
    public float Health;
    public bool EnemyDefeat;

    public int EnemyMoveA;
    public int EnemyMoveB;
    public int EnemyMoveC;

    void Start()
    {
        Health = Random.Range(50,100);

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
