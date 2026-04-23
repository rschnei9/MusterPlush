using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    public SpriteRenderer [] Button;
    public Sprite [] ButtonState;

    void Start()
    {
        
    }

    void Update()
    {
       
    }
    public void UpdateButtonA(int ButtonA)
    {
        Button[0].sprite = ButtonState[0];
    }
}
