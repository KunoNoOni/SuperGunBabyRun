using UnityEngine;

public class MoveSpaceshipBullet : MonoBehaviour 
{
    private float timeDelay;
    public GameObject bulletHit;
    
    void Update()
    {
        Destroy(this.gameObject, 2f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("GroundCollider") || other.gameObject.CompareTag("Player"))
        {
            Instantiate(bulletHit, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
