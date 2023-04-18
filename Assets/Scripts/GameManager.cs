using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(Player.gameObject);
            Destroy(floatingTextManager.gameObject);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);

        PlayerPrefs.DeleteAll();
    }

    // Ressources 
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // References 
    public Player Player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;

    // Logic
    public int coins;
    public int experience;

    // Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }


    // Upgrade weapon
    public bool TryUpgradeWeapon()
    {
        // is the weapon max level?
        if(weaponPrices.Count <= weapon.weaponLevel)
        {
            return false;
        }
        if (coins >= weaponPrices[weapon.weaponLevel])
        {
            coins -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;

        }

        return false;
        
    }


    // Experience system
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while(experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count) // Max level
            {
                return r;
            }
        }

        return r;
    }
    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }
    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if (currLevel < GetCurrentLevel())
        {
            OnLevelUp();
        }
    }

    public void OnLevelUp()
    {
        Player.OnLevelUp();
    }
  
    // State of game -  save and load
    /* INT preferedSkin
     * INT coins
     * INT experience
     * INT weaponLevel
     */

    public void SaveState()
    {
        string s = "";

        s += "0" + "|";
        s += coins.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState"))
        {
            return;
        }
   
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // Change player skin
        coins = int.Parse(data[1]);

        // Experience 
        experience = int.Parse(data[2]);
        if(GetCurrentLevel() != 1)
            Player.SetLevel(GetCurrentLevel());

        // Change weapon level
        weapon.SetWeaponLevel(int.Parse(data[3]));

        Player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }
    


}
