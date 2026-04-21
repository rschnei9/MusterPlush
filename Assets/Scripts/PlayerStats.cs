using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public bool PlushSelect;

    public float Power;
    public float Defense;
    public float Speed;

    void Start()
    {
        Power = 0;
        Defense = 0;
        Speed = 0;
        PlushSelect = true;
    }

    void Update()
    {
        //Cat Selected
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlushSelect = false;
            Power = 3;
            Defense = 1;
            Speed = 4;
        }
        //Bear Selected
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlushSelect = false;
            Power = 3;
            Defense = 4;
            Speed = 1;
        }
        //Bunny Selected
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlushSelect = false;
            Power = 2;
            Defense = 3;
            Speed = 3;
        }
    }
}
