using UnityEngine;

public class EnemySpaceshipController : MonoBehaviour 
{
    public GameObject bullet;
    public Transform bulletSpawnPoint;
    public int damage;
    public GameObject enemySpaceshipExplosion;

    private float fireCooldown;
    private float fireCooldownReset;
    private float speed;
    private int HP;
    private PlayerController pc;
    private SoundManager sm;

    private void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        fireCooldownReset = 1.75f;
        fireCooldown = fireCooldownReset;
        speed = 4f;
        HP = 1;
        damage = 1;
        LevelEnemy();
    }

    private void Update()
    {
        if (transform.position.x < -1f)
        {
            Destroy(this.gameObject);
        }

        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0)
        {
            fireCooldown = fireCooldownReset;
            sm.PlaySound(sm.sounds[9]);
            Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
        }

        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            sm.PlaySound(sm.sounds[8]);
            Instantiate(enemySpaceshipExplosion, new Vector3(this.transform.position.x, this.transform.position.y - .25f, -1), Quaternion.identity);
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            sm.PlaySound(sm.sounds[5]);
            TakeDamage(pc.damage);
            pc.XP += 200;
        }
    }

    private int GetRandomNumber(int maxValue)
    {
        return Random.Range(0, maxValue);
    }
  
    private void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            sm.PlaySound(sm.sounds[8]);
            Instantiate(enemySpaceshipExplosion, new Vector3(this.transform.position.x, this.transform.position.y - .25f, -1), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void LevelEnemy()
    {
        float upgradeTimes;

        upgradeTimes = Mathf.Floor(pc.characterLevel / 2);

        for (int i = 1; i <= upgradeTimes; i++)
        {
            damage += 5;
            HP += 5;
        }
    }
}
