using UnityEngine;

public class HeartManager : MonoBehaviour
{
    public SpriteRenderer [] Hearts;
    public Sprite [] HeartStates;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateHearts(int Health)
    {
        switch (Health)
        {
            case 16:
            Hearts[0].sprite = HeartStates[4];
            Hearts[1].sprite = HeartStates[4];
            Hearts[2].sprite = HeartStates[4];
            Hearts[3].sprite = HeartStates[4];
            break;

            case 15:
            Hearts[0].sprite = HeartStates[4];
            Hearts[1].sprite = HeartStates[4];
            Hearts[2].sprite = HeartStates[4];
            Hearts[3].sprite = HeartStates[3];
            break;

            case 14:
            Hearts[0].sprite = HeartStates[4];
            Hearts[1].sprite = HeartStates[4];
            Hearts[2].sprite = HeartStates[4];
            Hearts[3].sprite = HeartStates[2];
            break;

            case 13:
            Hearts[0].sprite = HeartStates[4];
            Hearts[1].sprite = HeartStates[4];
            Hearts[2].sprite = HeartStates[4];
            Hearts[3].sprite = HeartStates[1];
            break;

            case 12:
            Hearts[0].sprite = HeartStates[4];
            Hearts[1].sprite = HeartStates[4];
            Hearts[2].sprite = HeartStates[4];
            Hearts[3].sprite = HeartStates[0];
            break;

            case 11:
            Hearts[0].sprite = HeartStates[4];
            Hearts[1].sprite = HeartStates[4];
            Hearts[2].sprite = HeartStates[3];
            Hearts[3].sprite = HeartStates[0];
            break;

            case 10:
            Hearts[0].sprite = HeartStates[4];
            Hearts[1].sprite = HeartStates[4];
            Hearts[2].sprite = HeartStates[2];
            Hearts[3].sprite = HeartStates[0];
            break;

            case 9:
            Hearts[0].sprite = HeartStates[4];
            Hearts[1].sprite = HeartStates[4];
            Hearts[2].sprite = HeartStates[1];
            Hearts[3].sprite = HeartStates[0];
            break;

            case 8:
            Hearts[0].sprite = HeartStates[4];
            Hearts[1].sprite = HeartStates[4];
            Hearts[2].sprite = HeartStates[0];
            Hearts[3].sprite = HeartStates[0];
            break;

            case 7:
            Hearts[0].sprite = HeartStates[4];
            Hearts[1].sprite = HeartStates[3];
            Hearts[2].sprite = HeartStates[0];
            Hearts[3].sprite = HeartStates[0];
            break;

            case 6:
            Hearts[0].sprite = HeartStates[4];
            Hearts[1].sprite = HeartStates[2];
            Hearts[2].sprite = HeartStates[0];
            Hearts[3].sprite = HeartStates[0];
            break;

            case 5:
            Hearts[0].sprite = HeartStates[4];
            Hearts[1].sprite = HeartStates[1];
            Hearts[2].sprite = HeartStates[0];
            Hearts[3].sprite = HeartStates[0];
            break;

            case 4:
            Hearts[0].sprite = HeartStates[4];
            Hearts[1].sprite = HeartStates[0];
            Hearts[2].sprite = HeartStates[0];
            Hearts[3].sprite = HeartStates[0];
            break;

            case 3:
            Hearts[0].sprite = HeartStates[3];
            Hearts[1].sprite = HeartStates[0];
            Hearts[2].sprite = HeartStates[0];
            Hearts[3].sprite = HeartStates[0];
            break;

            case 2:
            Hearts[0].sprite = HeartStates[2];
            Hearts[1].sprite = HeartStates[0];
            Hearts[2].sprite = HeartStates[0];
            Hearts[3].sprite = HeartStates[0];
            break;

            case 1:
            Hearts[0].sprite = HeartStates[1];
            Hearts[1].sprite = HeartStates[0];
            Hearts[2].sprite = HeartStates[0];
            Hearts[3].sprite = HeartStates[0];
            break;

            default:
            Hearts[0].sprite = HeartStates[0];
            Hearts[1].sprite = HeartStates[0];
            Hearts[2].sprite = HeartStates[0];
            Hearts[3].sprite = HeartStates[0];
            break;

        }
    }
}
