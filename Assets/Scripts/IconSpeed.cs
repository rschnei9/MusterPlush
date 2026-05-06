using UnityEngine;

public class IconSpeed : MonoBehaviour
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
        if (plstat.SUP == 2) {Icon[0].sprite = IconStates[1];}
        if (plstat.SDOWN == 2) {Icon[0].sprite = IconStates[2];}
        if (plstat.SUP == 2 && plstat.SDOWN == 2) {Icon[0].sprite = IconStates[0];}
        if (plstat.SUP != 2 && plstat.SDOWN != 2) {Icon[0].sprite = IconStates[0];}
        }
    }
}
