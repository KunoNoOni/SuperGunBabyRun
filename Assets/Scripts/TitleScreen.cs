using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour 
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

    public void InstructionsScreen()
    {
        SceneManager.LoadScene(1);
    }

    public void CreditsScreen()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
