using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    private SliderControl healthBarControl;
    private GameObject character;
    private SpriteRenderer characterSprite;
    private GameController gameController;
    public Slider healthBar;
    private int currentHP;
    public int maxHP = 10;
    private float safeTime = 0.5f;
    private float safeCoolDown = 0;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Character");
        characterSprite = character.GetComponent<SpriteRenderer>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        healthBarControl = healthBar.GetComponent<SliderControl>();
        currentHP = maxHP;
        healthBarControl.UpdateBar(currentHP, maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        // reduce safe cd when get hit
        if (safeCoolDown >= 0)
        {
            safeCoolDown -= Time.deltaTime;
        }
    }
    private void OnTriggerStay2D(Collider2D collision) // player can get damage continuously
    {
        if (safeCoolDown <= 0)
        {
            if (collision.CompareTag("EnemyBullet")) // when touch enemy bullet
            {
                GetHit(collision.GetComponent<BulletController>().bulletDamage);
                Destroy(collision);
            }
            if (collision.CompareTag("Enemy")) // when touch enemy
            {
                GetHit(collision.GetComponent<EnemyStatus>().enemyDamage);
            }
        }
    }

    public void GetHit(int damage)
    {
        safeCoolDown = safeTime;
        StartCoroutine(DamageEffect());
        currentHP -= damage;
        healthBarControl.UpdateBar(currentHP, maxHP);
        if (currentHP <= 0)
        {
            currentHP = 0;
            Death();
        }
    }

    public void Health(int amount)
    {
        if (currentHP < maxHP)
        {
            currentHP += amount;
        }
        else
        {
            currentHP = maxHP;
        }
    }

    public void Death()
    {
        Destroy(gameObject, 1);
        character.GetComponent<Animator>().SetTrigger("Death");
        // Call gameover
        gameController.EndGame();
    }

    IEnumerator DamageEffect()
    {
        characterSprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        characterSprite.color = Color.white;
    }

}
