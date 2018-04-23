using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour 
{
    private MusicManager mm;

    void Awake()
    {
        mm = GameObject.Find("MusicManager").GetComponent<MusicManager>();
    }

    private void Start()
    {
        mm.PlaySound(mm.music[0]);
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene(0);
    }
}
