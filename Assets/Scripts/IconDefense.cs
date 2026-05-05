using UnityEngine;

public class IconDefense : MonoBehaviour
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
        if (plstat.DUP == 2) {Icon[0].sprite = IconStates[1];}
        if (plstat.DDOWN == 2) {Icon[0].sprite = IconStates[2];}
        if (plstat.DUP == 2 && plstat.DDOWN == 2) {Icon[0].sprite = IconStates[0];}
        }
    }
}