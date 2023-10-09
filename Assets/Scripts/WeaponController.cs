using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bullet;
    private Transform firePos;
    private AudioSource audioSrc;
    private float modifyDamage;
    private float modifyCD;
    private bool isSound;
    public float cooldown = 0.5f;
    public int eachBulletDamage;
    public int numberOfBullets = 1;
    private float delayEach = 0.05f;
    private float fireTime = 0;
    private float bulletForce = 100;
    public bool spread = false;
    private float maxSpread = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance != null)
        {
            isSound = DataManager.Instance.GetSound();
        }
        else
        {
            Debug.LogError("DataManager not found an instance!");
        }
        InitData();
    }

    void InitData()
    {
        modifyDamage = GetComponentInParent<PlayerController>().damageRate;
        modifyCD = GetComponentInParent<PlayerController>().fireRate;
        firePos = GetComponentInChildren<Transform>();
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateGun();
        if (Input.GetMouseButton(0) && fireTime <= 0)
        {
            StartCoroutine(FireBullet(transform.right));
            if (isSound)
            {
                audioSrc.Play();
            }
            fireTime = cooldown/(cooldown*modifyCD);
        }
        else if (fireTime > 0)
        {
            fireTime -= Time.deltaTime;
        }
    }

    void RotateGun()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDirection = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
        if (angle < 90 && angle > -90)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
    }

    IEnumerator FireBullet(Vector3 direction)
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            // create bullet
            GameObject bulletTmp = Instantiate(bullet, firePos.position, Quaternion.identity);
            bulletTmp.GetComponent<BulletController>().bulletDamage = (int)(eachBulletDamage * modifyDamage);
            Destroy(bulletTmp, 0.5f);
            // fire bullet
            Rigidbody2D bulletRb = bulletTmp.GetComponent<Rigidbody2D>();
            if (!spread)
            {
                // straight
                bulletRb.AddForce(direction.normalized * bulletForce, ForceMode2D.Impulse);
                yield return new WaitForSeconds(delayEach);
            }
            else
            {
                // shotgun
                Vector2 speadDir = direction + new Vector3(Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread));
                bulletRb.AddForce(speadDir.normalized * bulletForce, ForceMode2D.Impulse);
                yield return null;
            }
        }
    }

}
