using UnityEngine;

public class EnemyController : MonoBehaviour 
{
    public GameObject bullet;
    public Transform bulletSpawnPoint;
    public GameObject[] upgrades;
    public GameObject[] items;
    public int damage;
    public GameObject enemyBloodSplatter;

    private float fireCooldown;
    private float fireCooldownReset;
    private float speed;
    private int HP;
    private PlayerController pc;
    private SoundManager sm;

    private void Start () 
	{
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        fireCooldownReset = 3.5f;
        speed = 4f;
        HP = 1;
        damage = 1;
        LevelEnemy();
        fireCooldown = fireCooldownReset;
    }
	
	private void Update () 
	{
        if(transform.position.x < -1f)
        {
            Destroy(this.gameObject);
        }

        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0)
        {
            fireCooldown = fireCooldownReset;
            sm.PlaySound(sm.sounds[6]);
            Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
        }

        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            sm.PlaySound(sm.sounds[7]);
            Instantiate(enemyBloodSplatter, new Vector3(this.transform.position.x, this.transform.position.y - .25f, -1), Quaternion.identity);
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            sm.PlaySound(sm.sounds[5]);
            TakeDamage(pc.damage);
            pc.XP += 100;
        }
    }

    private int GetRandomNumber(int maxValue)
    {
        return Random.Range(0, maxValue);
    }

    private void CheckForLootDrop()
    {
        int upgradeDropPercent = 25;
        int itemDropPercent = 15;

        if (GetRandomNumber(100) <= itemDropPercent)
        {
            Instantiate(items[GetRandomNumber(items.Length)], this.transform.position, Quaternion.identity);
        }
        else if (GetRandomNumber(100) <= upgradeDropPercent)
        {
            Instantiate(upgrades[GetRandomNumber(upgrades.Length)], this.transform.position, Quaternion.identity);
        }
    }

    private void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            sm.PlaySound(sm.sounds[7]);
            Destroy(this.gameObject);
            Instantiate(enemyBloodSplatter, new Vector3(this.transform.position.x, this.transform.position.y-.25f, -1), Quaternion.identity);
            CheckForLootDrop();
        }
    }

    private void LevelEnemy()
    {
        float upgradeTimes;

        upgradeTimes = Mathf.Floor(pc.characterLevel / 2);

        for (int i = 1; i <= upgradeTimes; i++)
        {
            if (i % 2 == 0)
            {
                if (fireCooldownReset > 1f)
                {
                    fireCooldownReset -= .50f;
                }
            }

            damage += 5;
            HP += 5;
        }

        Debug.Log("Enemy fireCooldownReset is " + fireCooldownReset + " damage is " + damage + " HP is " + HP);
    }
}
