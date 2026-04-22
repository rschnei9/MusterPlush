using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float Power;
    public float Defense;
    public float Speed;
    public float Health;
    public bool EnemyDefeat;

    void Start()
    {
        Health = Random.Range(5,10);
    }

    void Update()
    {
        if (Health <= 0)
        {
            EnemyDefeat = true;
        }
    }
}
