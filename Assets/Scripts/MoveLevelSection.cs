using UnityEngine;

public class MoveLevelSection : MonoBehaviour 
{
    private float minMoveTo;
    private float moveSpeed;
    private float widthOfLevelSection;
    private Vector3 teleportTo;
    private GameManager gm;

    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        widthOfLevelSection = 18f;
        moveSpeed = 3f;
        minMoveTo = -19f;
        teleportTo = new Vector3(widthOfLevelSection * 3f,0,0);
    }

    void Update () 
	{
        if (!gm.gameOver)
        {
            this.transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            if (this.transform.position.x <= minMoveTo)
            {

                this.transform.position = transform.position + teleportTo;
            }
        }
	}
}
