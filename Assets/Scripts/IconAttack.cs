using UnityEngine;

public class IconAttack : MonoBehaviour
{
    public SpriteRenderer [] Icon;
    public Sprite [] IconStates;
    public PlayerStats plstat;
    public BattleManager bat;

    void Start()
    {
        Icon[0].sprite = IconStates[0];
    }

    void Update()
    {
        if (bat.MenuSummon == true)
        {
        if (plstat.PUP == 2) {Icon[0].sprite = IconStates[1];}
        if (plstat.PDOWN == 2) {Icon[0].sprite = IconStates[2];}
        if (plstat.PUP == 2 && plstat.PDOWN == 2) {Icon[0].sprite = IconStates[0];}
        }
    }
}
