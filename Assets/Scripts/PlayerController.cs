using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour 
{
    public float jumpHeight;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask theGround;
    public GameObject bullet;
    public Transform bulletSpawnPoint;
    public bool canDestroyRocks;
    public int characterLevel;
    public int damage;
    public int XP;
    public GameObject gunFire;
    public GameObject bloodsplatter;

    private bool grounded;
    private Rigidbody2D player;
    private Animator anim;
    private float fireCooldown = 0;
    private float fireCooldownReset = .5f;
    private bool gotUpgrade_RateOfFire;
    private bool gotUpgrade_DoubleDamage;
    private bool gotUpgrade_HE_Bullets;
    private float rateOfFireCooldown = 0;
    private float rateOfFireCooldownReset = 10f;
    private float doubleDamageCooldown = 0;
    private float doubleDamageCooldownReset = 10f;
    private float HE_BulletsCooldown = 0;
    private float HE_BulletsCooldownReset = 10f;
    private float smallPotionHealthPercent = .25f;
    private float largePotionHealthPercent = .50f;
    private SpriteRenderer bulletSR;
    private int agility;
    private int accuracy;
    private int endurance;
    private int HP;
    private int maxHP;
    private GameManager gm;
    private UIManager ui;
    private bool canDoubleJump;
    private int damageBonus;
    private int HPBonus;
    private int nextLevel;
    private int originalDamage;
    private int enemyDamage;
    private int numberOfJumps;
    private SoundManager sm;


    void Start () 
	{
        player = GetComponent<Rigidbody2D>();
        bulletSR = bullet.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        ui = GameObject.Find("UIManager").GetComponent<UIManager>();
        sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        bulletSR.color = Color.white;
        agility = 10;
        UpdateAgilityText();
        accuracy = 10;
        UpdateAccuracyText();
        endurance = 10;
        UpdateEnduranceText();
        HP = 10;
        UpdateHPText();
        maxHP = 10;
        damageBonus = 0;
        HPBonus = 0;
        nextLevel = 2000 * characterLevel;
        UpdateLevelText();
        UpdateDamageText();
        numberOfJumps = 0;
    }
	
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius, theGround);
    }

    void Update()
    {
        anim.SetBool("grounded", grounded);
        fireCooldown -= Time.deltaTime;
        XP++;
        ui.experienceText.text = "" + XP.ToString();

        if(grounded)
        {
            numberOfJumps = 0;
        }

        if (Input.GetButtonDown("Fire2") && grounded || Input.GetButtonDown("Fire2") && canDoubleJump)
        {
            numberOfJumps++;
            if (numberOfJumps < 2)
            {
                sm.PlaySound(sm.sounds[2]);
                player.velocity = new Vector2(0, jumpHeight);
            }
        }

        if (Input.GetButton("Fire1"))
        {
            if (fireCooldown <= 0)
            {
                fireCooldown = fireCooldownReset;
                sm.PlaySound(sm.sounds[1]);
                Instantiate(gunFire, bulletSpawnPoint.position, Quaternion.identity);
                Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
            }
        }

        if (gotUpgrade_RateOfFire)
        {
            rateOfFireCooldown -= Time.deltaTime;
            UpdateROFText();
            if (rateOfFireCooldown <= 0)
            {
                gotUpgrade_RateOfFire = false;
                rateOfFireCooldown = 0;
                fireCooldownReset = .5f;
                UpdateROFText();
                ChangeBulletColor();
            }
        }

        if(gotUpgrade_DoubleDamage)
        {
            doubleDamageCooldown -= Time.deltaTime;
            UpdateDDText();
            if(doubleDamageCooldown <= 0)
            {
                gotUpgrade_DoubleDamage = false;
                doubleDamageCooldown = 0;
                damage = originalDamage;
                UpdateDDText();
                ChangeBulletColor();
            }
        }

        if(gotUpgrade_HE_Bullets)
        {
            HE_BulletsCooldown -= Time.deltaTime;
            UpdateHEBText();
            if (HE_BulletsCooldown <= 0)
            {
                gotUpgrade_HE_Bullets = false;
                canDestroyRocks = false;
                HE_BulletsCooldown = 0;
                UpdateHEBText();
                ChangeBulletColor();
            }
        }
        
        if(GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            enemyDamage = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>().damage;
        }

        if (GameObject.FindGameObjectWithTag("EnemySpaceship") != null)
        {
            enemyDamage = GameObject.FindGameObjectWithTag("EnemySpaceship").GetComponent<EnemySpaceshipController>().damage;
        }

        player.velocity = new Vector2(0, player.velocity.y);
        CheckForLevelUp();
    }

    private void CheckForLevelUp()
    {
        if(XP > nextLevel)
        {
            sm.PlaySound(sm.sounds[0]);
            characterLevel++;
             nextLevel = 2000 * characterLevel;
            agility++;
            accuracy++;
            endurance++;
            maxHP += 5;
            HP = maxHP;
            damage++;
            UpdateLevelText();
            UpdateAgilityText();
            UpdateAccuracyText();
            UpdateEnduranceText();
        }
        CheckForAgilityBonus();
        CheckForAccuracyBonus();
        CheckForEnduranceBonus();
    }

    private void CheckForEnduranceBonus()
    {
        int bonusRequirement;

        bonusRequirement = GetBonusRequirement(endurance);

        if (bonusRequirement > HPBonus)
        {
            HPBonus = bonusRequirement;
            HP += HPBonus;
        }

        UpdateHPText();
    }

    private void CheckForAccuracyBonus()
    {
        int bonusRequirement;

        bonusRequirement = GetBonusRequirement(accuracy);

        if (bonusRequirement > damageBonus)
        {
            damageBonus = bonusRequirement;
            damage += damageBonus;
        }
        UpdateDamageText();
    }

    private void CheckForAgilityBonus()
    {
        switch (agility)
        {
            case 12:
                {
                    canDoubleJump = true;
                    UpdateDJText();
                    break;
                }
        }
    }

    private int GetBonusRequirement(int stat)
    {
        return (stat / 2) - 5;
    }

    private void ChangeBulletColor()
    {
        if(gotUpgrade_RateOfFire)
        {
            bulletSR.color = Color.green;
        }
        else if (gotUpgrade_DoubleDamage)
        {
            bulletSR.color = Color.red;
        }
        else if (gotUpgrade_HE_Bullets)
        {
            bulletSR.color = Color.yellow;
        }
        else
        {
            bulletSR.color = Color.white;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        

        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(5);
            sm.PlaySound(sm.sounds[3]);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("EnemySpaceship"))
        {
            TakeDamage(10);
            sm.PlaySound(sm.sounds[3]);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            TakeDamage(enemyDamage);
            sm.PlaySound(sm.sounds[3]);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage((characterLevel/2)+1);
            sm.PlaySound(sm.sounds[3]);
        }

        if (other.gameObject.CompareTag("Upgrade_RateOfFire"))
        {
            sm.PlaySound(sm.sounds[11]);
            Destroy(other.gameObject);
            rateOfFireCooldown += rateOfFireCooldownReset;
            gotUpgrade_RateOfFire = true;
            if (bulletSR.color == Color.white)
            {
                bulletSR.color = Color.green;
            }
            fireCooldownReset = .25f;
        }

        if (other.gameObject.CompareTag("Upgrade_DoubleDamage"))
        {
            sm.PlaySound(sm.sounds[12]);
            Destroy(other.gameObject);
            doubleDamageCooldown += doubleDamageCooldownReset;
            gotUpgrade_DoubleDamage = true;
            if (bulletSR.color == Color.white)
            {
                bulletSR.color = Color.red;
            }
            originalDamage = damage;
            damage *= 2;
            UpdateDamageText();
        }

        if (other.gameObject.CompareTag("Upgrade_HE_Bullets"))
        {
            sm.PlaySound(sm.sounds[13]);
            Destroy(other.gameObject);
            HE_BulletsCooldown += HE_BulletsCooldownReset;
            gotUpgrade_HE_Bullets = true;
            canDestroyRocks = true;
            if (bulletSR.color == Color.white)
            {
                bulletSR.color = Color.yellow;
            }
        }

        if (other.gameObject.CompareTag("Small_Potion"))
        {
            sm.PlaySound(sm.sounds[10]);
            Destroy(other.gameObject);
            CalculateHealthIncrease(smallPotionHealthPercent);
        }

        if (other.gameObject.CompareTag("Large_Potion"))
        {
            sm.PlaySound(sm.sounds[10]);
            Destroy(other.gameObject);
            CalculateHealthIncrease(largePotionHealthPercent);
        }
    }

    private void CalculateHealthIncrease(float percentToHeal)
    {
        int numberOfHealthPoints;
        int differenceBetweenHPAndMaxHP;

        numberOfHealthPoints = Mathf.RoundToInt(percentToHeal * maxHP);
        differenceBetweenHPAndMaxHP = maxHP - HP;
        if(differenceBetweenHPAndMaxHP > numberOfHealthPoints)
        {
            HP += numberOfHealthPoints;
        }
        else
        {
            HP = maxHP;
        }
        
        UpdateHPText();
    }

    private void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        UpdateHPText();
        if (HP <= 0)
        {
            Instantiate(bloodsplatter, new Vector3(this.transform.position.x, this.transform.position.y - .25f, -1), Quaternion.identity);
            Destroy(this.gameObject);
            gm.gameOver = true;
            SceneManager.LoadScene(4);
        }
    }

    private void UpdateHPText()
    {
        ui.HPText.text = "" + HP.ToString();
    }

    private void UpdateLevelText()
    {
        ui.levelText.text = "" + characterLevel.ToString();
    }

    private void UpdateROFText()
    {
        if (gotUpgrade_RateOfFire)
        {
            ui.rateOfFireText.color = Color.green;
            ui.rateOfFireText.text = "" + rateOfFireCooldown.ToString("###.##");
        }
        else
        {
            ui.rateOfFireText.color = Color.black;
            ui.rateOfFireText.text = "000.00";
        }
    }

    private void UpdateDDText()
    {
        if(gotUpgrade_DoubleDamage)
        {
            ui.doubleDamageText.color = Color.red;
            ui.doubleDamageText.text = "" + doubleDamageCooldown.ToString("###.##");
        }
        else
        {
            ui.doubleDamageText.color = Color.black;
            ui.doubleDamageText.text = "000.00";
            UpdateDamageText();
        }
    }

    private void UpdateHEBText()
    {
        if (gotUpgrade_HE_Bullets)
        {
            ui.heBulletsText.color = Color.yellow;
            ui.heBulletsText.text = "" + HE_BulletsCooldown.ToString("###.##");
        }
        else
        {
            ui.heBulletsText.color = Color.black;
            ui.heBulletsText.text = "000.00";
        }
    }

    private void UpdateAgilityText()
    {
        ui.agilityText.text = "" + agility.ToString();
    }

    private void UpdateAccuracyText()
    {
        ui.accuracyText.text = "" + accuracy.ToString();
    }

    private void UpdateEnduranceText()
    {
        ui.enduranceText.text = "" + endurance.ToString();
    }

    private void UpdateDamageText()
    {
        ui.damageText.text = "" + damage.ToString();
    }

    private void UpdateDJText()
    {
        ui.doubleJumpText.color = new Color32(255, 255, 0, 255);
    }
}
