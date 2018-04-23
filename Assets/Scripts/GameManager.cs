using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public bool gameOver;

    private MusicManager mm;

    private void Awake() 
    {
        mm = GameObject.Find("MusicManager").GetComponent<MusicManager>();
    }

    private void Start()
    {
        mm.PlaySound(mm.music[3]);
    }
}
