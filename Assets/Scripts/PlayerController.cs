using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    /* id - skin - special
    * 0 - Aura - speed
    * 1 - Axel - fire rate
    * 2 - Blaze - damage
    * 3 - Draven - HP
    */
    private GameObject character;
    private Animator anim;
    private Vector3 moveInput;
    private Coroutine dashEffectCoroutine;
    public Image DashImage;
    public Image RollImage;
    public float speedRate = 1.0f;
    public float damageRate = 1.0f;
    public float fireRate = 1.0f;
    private int CharacterID;
    private float currentSpeed;
    private float moveSpeed = 18, rollSpeed = 30, dashSpeed = 50; // ratio 9:15:25
    private float rollTime = 5f, rollCoolDown = 0, rollRefill = 2f; // when use 0 fill, from 0 to 5
    private float dashTime = 0.25f, dashCoolDown = 0, dashDelay = -3f; // when use 0.25, from 0 and to -3
    private bool isWalk, isRoll, isDash;
    public GameObject[] CharacterPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance != null)
        {
            CharacterID = DataManager.Instance.GetCharacterID();

            character = Instantiate(CharacterPrefabs[CharacterID]);
            character.transform.SetParent(transform, false);
        }
        else
        {
            Debug.LogError("DataManager not found an instance!");
        }
        InitData();
    }

    void InitData()
    {
        character = GameObject.FindGameObjectWithTag("Character");
        anim = character.GetComponent<Animator>();
        currentSpeed = moveSpeed;
        ShopManager shopManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShopManager>();
        shopManager.money = 5000;
        switch (CharacterID)
        {
            case 0:
                shopManager.AddSpeed();
                shopManager.AddSpeed();
                shopManager.ClearNoticeText();
                break;
            case 1:
                shopManager.AddFirerate();
                shopManager.ClearNoticeText();
                break;
            case 2:
                shopManager.AddDamage();
                shopManager.ClearNoticeText();
                break;
            case 3:
                shopManager.AddMaxHP();
                shopManager.AddMaxHP();
                break;
            default:
                Debug.LogError("Invalid CharacterID!");
                break;
        }
        shopManager.money = 0;
        shopManager.ClearNoticeText();
        shopManager.GetMoney(100); // init with $100 on start to buy weapon
    }

    // Update is called once per frame
    void Update()
    {
        // reset state
        UpdateState();
        // control movement
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        transform.Translate(moveInput.normalized * currentSpeed * speedRate * Time.deltaTime);
        // flip character
        if (moveInput.x > 0)
        {
            character.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x < 0)
        {
            character.transform.localScale = new Vector3(-1, 1, 1);
        }
        // update functions
        if (moveInput.magnitude != 0) // if player change speed
        {
            if (Input.GetKey(KeyCode.Space) && rollCoolDown < rollTime) // hold space for rolling
            {
                isRoll = true;
            }
            else if (Input.GetMouseButtonDown(1)) // right click for dashing
            {
                isDash = true;
            }
            else
            {
                isWalk = true;
            }
            Walking();
            Rolling();
            Dashing();
        }
    }

    void UpdateState()
    {
        // reset all variables
        isWalk = false;
        isRoll = false;
        isDash = false;
        // reset animation
        anim.SetBool("isWalk", false);
        anim.SetBool("isRoll", false);
        // animate skill
        DashImage.fillAmount = (dashDelay - dashCoolDown) / dashDelay;
        RollImage.fillAmount = (rollTime - rollCoolDown) / rollTime;
        // reduce roll cd when not roll
        if (!isRoll && rollCoolDown > 0)
        {
            rollCoolDown -= Time.deltaTime;
        }
        // reduce dash cd when dash
        if (dashCoolDown > dashDelay)
        {
            dashCoolDown -= Time.deltaTime;
        }
    }

    void Walking()
    {
        if (isWalk)
        {
            anim.SetBool("isWalk", true);
            currentSpeed = moveSpeed;
        }
    }

    void Rolling()
    {
        // fill roll cd and increase speed continuous
        if (isRoll && rollCoolDown < rollTime)
        {
            anim.SetBool("isRoll", true);
            rollCoolDown += rollRefill * Time.deltaTime;
            currentSpeed = rollSpeed;
        }
        else
        {
            anim.SetBool("isRoll", false);
        }
    }

    void Dashing()
    {
        // set dash cd and increase speed on dash cd
        if (isDash && dashCoolDown <= dashDelay)
        {
            if (dashEffectCoroutine != null) StopCoroutine(dashEffectCoroutine);
            dashEffectCoroutine = StartCoroutine(DashEffect());
            dashCoolDown = dashTime;
        }
        if (dashCoolDown > 0)
        {
            currentSpeed = dashSpeed;
        }
    }

    IEnumerator DashEffect()
    {
        for (int i = 1; i <= 5; i++)
        {
            GameObject ghostEffect = Instantiate(character, transform.position, transform.rotation);
            ghostEffect.GetComponent<Animator>().SetTrigger("Ghost");
            Destroy(ghostEffect, 0.5f);
            yield return new WaitForSeconds(dashTime / 5);
        }
    }


}
