using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text moneyTextMain;
    public Text moneyTextItem;
    public Text moneyTextWeapon;
    public Text noticeTextItem;
    public Text noticeTextWeapon;
    private PlayerStatus playerStatus; // for hp, damge
    private PlayerController playerController; // for speed, damage
    private GameObject[] weaponSlots;
    public GameObject[] weaponPrefabs; // 0: pistol, 1: riffle, 2: shotgun
    public GameObject[] listItems; // 0: pistol, 1: riffle, 2: shotgun, 3: speed, 4: damage, 5: recover, 6: firerate, 7: hp, 8: health
    public int money = 0;
    // Start is called before the first frame update
    void Start()
    {
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        weaponSlots = GameObject.FindGameObjectsWithTag("WeaponSlot");
    }

    public void GetMoney(int bonus)
    {
        money += bonus;
        if (money < 0)
        {
            money = 0;
        }
        moneyTextMain.text = "Money: " + money;
        moneyTextItem.text = "Money: " + money;
        moneyTextWeapon.text = "Money: " + money;
    }

    private GameObject CheckFreeWeaponSlots()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot.transform.childCount == 0)
            {
                return weaponSlot;
            }
        }
        return null;
    }

    private bool CheckEnoughMoney(int id)
    {
        Text itemCost = listItems[id].transform.Find("Item Cost").gameObject.GetComponent<Text>();
        if (itemCost != null)
        {
            int cost = int.Parse(itemCost.text.Substring(1));
            if (cost <= money)
            {
                GetMoney(-cost);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            Debug.LogError("Cannot get Item Cost!");
            return false;
        }
    }

    private void AddWeapon(int prefabsId)
    {
        GameObject freeSlot = CheckFreeWeaponSlots();
        if (freeSlot != null)
        {
            GameObject newWeapon = Instantiate(weaponPrefabs[prefabsId]);
            newWeapon.transform.SetParent(freeSlot.transform, false);
        }
        else
        {
            noticeTextWeapon.text = "You have all slots full!";
        }
    }

    public void ClearNoticeText()
    {
        noticeTextWeapon.text = "";
    }

    private void NoticeLowMoney()
    {
        noticeTextWeapon.text = "You dont have enough money!";
    }

    public void NoticePistol()
    {
        noticeTextWeapon.text = "More damage, normal fire rate";
    }

    public void NoticeRiffle()
    {
        noticeTextWeapon.text = "More fire rate, normal damage";
    }

    public void NoticeShotgun()
    {
        noticeTextWeapon.text = "More bullet, very low damage";
    }

    public void NoticeSpeed()
    {
        noticeTextItem.text = "Increased speed by 10%";
    }
    
    public void NoticeDamage()
    {
        noticeTextItem.text = "Increased damage by 20%";
    }
    
    public void NoticeRecover()
    {
        noticeTextItem.text = "Health +1HP when wave end";
    }
    
    public void NoticeFirerate()
    {
        noticeTextItem.text = "Increased FireRate by 10%";
    }
    
    public void NoticeMaxHP()
    {
        noticeTextItem.text = "Increased MaxHP by 1";
    }
    
    public void NoticeHealth()
    {
        noticeTextItem.text = "Already health HP by 1 + 50% Recover HP";
    }

    public void AddPistol()
    {
        if (CheckEnoughMoney(0))
        {
            AddWeapon(0);
        }
        else
        {
            NoticeLowMoney();
        }
    }

    public void AddRiffle()
    {
        if (CheckEnoughMoney(0))
        {
            AddWeapon(1);
        }
        else
        {
            NoticeLowMoney();
        }
    }

    public void AddShotgun()
    {
        if (CheckEnoughMoney(0))
        {
            AddWeapon(2);
        }
        else
        {
            NoticeLowMoney();
        }
    }

    public void AddSpeed()
    {
        if (CheckEnoughMoney(3))
        {
            playerController.speedRate += 0.1f;
            NoticeSpeed();
        }
        else
        {
            NoticeLowMoney();
        }
    }

    public void AddDamage()
    {
        if (CheckEnoughMoney(4))
        {
            playerController.damageRate += 0.2f;
            NoticeDamage();
        }
        else
        {
            NoticeLowMoney();
        }
    }

    public void AddRecover()
    {
        if (CheckEnoughMoney(5))
        {
            GetComponent<GameController>().playerHealthAtEnd++;
            NoticeRecover();
        }
        else
        {
            NoticeLowMoney();
        }
    }

    public void AddFirerate()
    {
        if (CheckEnoughMoney(6))
        {
            playerController.fireRate += 0.1f;
            NoticeFirerate();
        }
        else
        {
            NoticeLowMoney();
        }
    }

    public void AddMaxHP()
    {
        if (CheckEnoughMoney(7))
        {
            playerStatus.maxHP++;
            NoticeMaxHP();
        }
        else
        {
            NoticeLowMoney();
        }
    }

    public void AddHealth()
    {
        if (CheckEnoughMoney(8))
        {
            playerStatus.Health((int)(1 + GetComponent<GameController>().playerHealthAtEnd * 0.5f));
            NoticeHealth();
        }
        else
        {
            NoticeLowMoney();
        }
    }
}
