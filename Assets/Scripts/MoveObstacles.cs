using UnityEngine;

public class MoveObstacles : MonoBehaviour 
{
    private float speed;
    private GameManager gm;

    void Start () 
	{
        speed = 3f;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (!gm.gameOver)
        {
            if (transform.position.x < -1f)
            {
                Destroy(this.gameObject);
            }

            transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }
}
