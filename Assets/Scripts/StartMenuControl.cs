using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class StartMenuControl : MonoBehaviour
{
    /* id - skin - special
     * 0 - Aura - speed
     * 1 - Axel - fire rate
     * 2 - Blaze - damage
     * 3 - Draven - HP
     */
    private bool isMusic;
    private bool isSound;
    private AudioSource audioSrc;
    public GameObject infoText;
    public Text buffText;
    public Button musicBtn;
    public Sprite musicOnImage;
    public Sprite musicOffImage;
    public Button soundBtn;
    public Sprite soundOnImage;
    public Sprite soundOffImage;
    public Slider modeSlider;
    public GameObject[] vCams;

    private void Start()
    {
        isMusic = true;
        isSound = true;
        audioSrc = GetComponent<AudioSource>();
        audioSrc.Play();
        infoText.GetComponent<Text>().text = Application.companyName + "\nVersion: " + Application.version;
    }

    public void PlayerAura()
    {
        DataManager.Instance.SetCharacterID(0);
        buffText.text = "Buff: Faster speed";
    }
    public void PlayerAxel()
    {
        DataManager.Instance.SetCharacterID(1);
        buffText.text = "Buff: Quicker fire rate";
    }
    public void PlayerBlaze()
    {
        DataManager.Instance.SetCharacterID(2);
        buffText.text = "Buff: More powerful";
    }
    public void PlayerDraven()
    {
        DataManager.Instance.SetCharacterID(3);
        buffText.text = "Buff: Stronger HP";
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void ToggleMusic()
    {
        isMusic = !isMusic;
        DataManager.Instance.SetMusic(isMusic);
        if (!isMusic)
        {
            audioSrc.Stop();
            musicBtn.GetComponent<Image>().sprite = musicOffImage;
        }
        else
        {
            audioSrc.Play();
            musicBtn.GetComponent<Image>().sprite = musicOnImage;
        }
    }

    public void ToggleSound()
    {
        isSound = !isSound;
        DataManager.Instance.SetSound(isSound);
        if (!isSound)
        {
            soundBtn.GetComponent<Image>().sprite = soundOffImage;
        }
        else
        {
            soundBtn.GetComponent<Image>().sprite = soundOnImage;
        }
    }

    public void ToggleInfo()
    {
        if (infoText.activeInHierarchy)
        {
            infoText.SetActive(false);
        }
        else
        {
            infoText.SetActive(true);
        }
    }

    public void SetMode()
    {
        int level = (int)modeSlider.value;
        DataManager.Instance.SetMode(level);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    private void EnableVirtualCam(int id)
    {
        foreach (var vCam in vCams)
        {
            vCam.SetActive(false);
        }
        vCams[id].SetActive(true);
    }

    public void VCam0()
    {
        EnableVirtualCam(0);
    }
    public void VCam1()
    {
        EnableVirtualCam(1);
    }
    public void VCam2()
    {
        EnableVirtualCam(2);
    }
    public void VCam3()
    {
        EnableVirtualCam(3);
    }

    public void MapAutumn()
    {
        DataManager.Instance.SetMap("Autumn");
    }
    public void MapWinter()
    {
        DataManager.Instance.SetMap("Winter");
    }

}
