using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject[] Music;
    public bool Destroyer;
    public bool BattleOn;
    public bool ChillOn;
    public bool DefeatOn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Destroyer == true)
        {
            Destroyer = false; Debug.Log("AUDIO BOOM");
            for (int i = 0; i < transform.childCount; i++)
            {Destroy(transform.GetChild(i).gameObject);}
        }
        if (BattleOn == true)
        {
            BattleOn = false; Debug.Log("Battle Music");
            Instantiate(Music[1], transform);
        }
        if (ChillOn == true)
        {
            ChillOn = false; Debug.Log("Chill Music");
            Instantiate(Music[0], transform);
        }
        if (DefeatOn == true)
        {
            DefeatOn = false; Debug.Log("Defeat Music");
            Instantiate(Music[2], transform);
        }
    }
}
