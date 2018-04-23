using UnityEngine;

public class LevelSpawnManager : MonoBehaviour 
{
    public GameObject[] obstacles;
    public GameObject[] enemy;

    private float spawnObstacleCooldown;
    private float spawnObstacleCooldownReset;
    private float spawnEnemyCooldown;
    private float spawnEnemyCooldownReset;
    private float minValue;
    private float maxValue;
    private PlayerController pc;
    private GameManager gm;

    void Start () 
	{
        minValue = 2f;
        maxValue = 5f;
        SetRandomObstacleCooldownReset();
        SetRandomEnemyCooldownReset();
        spawnObstacleCooldown = spawnObstacleCooldownReset;
        spawnEnemyCooldown = spawnEnemyCooldownReset;
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	void Update () 
	{
        if (!gm.gameOver)
        {
            spawnObstacleCooldown -= Time.deltaTime;
            spawnEnemyCooldown -= Time.deltaTime;

            if (spawnObstacleCooldown <= 0)
            {
                SetRandomObstacleCooldownReset();
                spawnObstacleCooldown = spawnObstacleCooldownReset;
                Instantiate(obstacles[Random.Range(0, obstacles.Length)], this.transform.position, Quaternion.identity);
            }

            if (spawnEnemyCooldown <= 0)
            {
                SetRandomEnemyCooldownReset();
                spawnEnemyCooldown = spawnEnemyCooldownReset;
                int randomEnemy = GetRandomEnemy();
                if (randomEnemy == 0)
                {
                    Instantiate(enemy[0], this.transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(enemy[1], new Vector3(this.transform.position.x, this.transform.position.y + 2, 0), Quaternion.identity);
                }
            }
        }
    }

    private void SetRandomObstacleCooldownReset()
    {
        spawnObstacleCooldownReset = Random.Range(minValue, maxValue);
    }

    private void SetRandomEnemyCooldownReset()
    {
        spawnEnemyCooldownReset = Random.Range(minValue, maxValue);
    }

    private int GetRandomEnemy()
    {
        if(pc.characterLevel < 5)
        {
            return 0;
        }

        return Random.Range(0, enemy.Length);
    }
}
