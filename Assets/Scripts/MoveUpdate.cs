using UnityEngine;

public class MoveUpdate : MonoBehaviour
{
    public SpriteRenderer [] Move;
    public Sprite [] MoveState;

    public void Hover()
    {
        Move[0].sprite = MoveState[1];
    }
    public void UnHover()
    {
        Move[0].sprite = MoveState[0];
    }
}
