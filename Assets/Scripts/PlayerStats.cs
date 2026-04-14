using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public bool PlushSelect;

    public float PlayerPower;
    public float PlayerDefense;
    public float PlayerSpeed;

    void Start()
    {
        PlayerPower = 0;
        PlayerDefense = 0;
        PlayerSpeed = 0;
        PlushSelect = true;
    }

    void Update()
    {
        //Cat Selected
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlushSelect = false;
            PlayerPower = 3;
            PlayerDefense = 1;
            PlayerSpeed = 4;
        }
        //Bear Selected
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlushSelect = false;
            PlayerPower = 3;
            PlayerDefense = 4;
            PlayerSpeed = 1;
        }
        //Bunny Selected
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlushSelect = false;
            PlayerPower = 2;
            PlayerDefense = 3;
            PlayerSpeed = 3;
        }
    }
}
