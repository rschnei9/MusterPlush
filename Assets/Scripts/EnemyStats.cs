using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public bool Select;

    public float EnemyPower;
    public float EnemyDefense;
    public float EnemySpeed;

    void Start()
    {
        EnemyPower = 0;
        EnemyDefense = 0;
        EnemySpeed = 0;
        Select = true;
    }

    void Update()
    {
        //Cat Selected
        if (Input.GetKeyDown(KeyCode.I))
        {
            Select = false;
            EnemyPower = 3;
            EnemyDefense = 1;
            EnemySpeed = 4;
        }
        //Bear Selected
        if (Input.GetKeyDown(KeyCode.O))
        {
            Select = false;
            EnemyPower = 3;
            EnemyDefense = 4;
            EnemySpeed = 1;
        }
        //Bunny Selected
        if (Input.GetKeyDown(KeyCode.P))
        {
            Select = false;
            EnemyPower = 2;
            EnemyDefense = 3;
            EnemySpeed = 3;
        }
    }
}
