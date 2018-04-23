using UnityEngine;

public class MoveBullet : MonoBehaviour 
{
    public bool moveLeft;
    public GameObject bulletHit;
    public GameObject rockExplosion;

    private float moveSpeed = 5f;
    private Vector3 direction;
    private float timeDelay;
    private PlayerController pc;
    private SoundManager sm;

    private void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    void Update () 
	{
        direction = moveLeft ? Vector3.left : Vector3.right;
        transform.position += direction * moveSpeed * Time.deltaTime;
        timeDelay = moveLeft ? 2f : 1f;
        Destroy(this.gameObject, timeDelay);
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            Instantiate(bulletHit, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle") && pc.canDestroyRocks)
        {
            sm.PlaySound(sm.sounds[8]);
            Instantiate(rockExplosion, new Vector3(this.transform.position.x, this.transform.position.y - .25f, -1), Quaternion.identity);
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }
    }
}
