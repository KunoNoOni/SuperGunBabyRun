using UnityEngine;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour 
{
    private MusicManager mm;

    void Awake()
    {
        mm = GameObject.Find("MusicManager").GetComponent<MusicManager>();
        
    }

    private void Start()
    {
        mm.PlaySound(mm.music[1]);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(3);
    }
}
