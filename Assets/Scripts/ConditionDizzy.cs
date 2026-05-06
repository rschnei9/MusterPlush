using UnityEngine;

public class ConditionDizzy : MonoBehaviour
{
    public SpriteRenderer [] Icon;
    public Sprite [] IconStates;
    public BattleManager bat;
    public GameObject[] Condition;


    void Start()
    {
        Icon[0].sprite = IconStates[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (bat.MenuSummon == true)
        {
            if (bat.DizzyTB <= 0) {Icon[0].sprite = IconStates[0];}
            if (bat.DizzyTB == 1) {Icon[0].sprite = IconStates[1];}
            if (bat.DizzyTB == 2) {Icon[0].sprite = IconStates[2];}
            if (bat.DizzyTB == 3) {Icon[0].sprite = IconStates[3];}
            if (bat.DizzyTB >= 4) {Icon[0].sprite = IconStates[4];}
        }
    }
}