using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using JetBrains.Annotations;
using System.Numerics;
using Unity.VisualScripting;
using TMPro;


public class alo : MonoBehaviour
{
   
    public Animator layot;
    public GameObject settings_open;
    public GameObject settings_close;
    public GameObject ses_on;
    public GameObject ses_off;
    public GameObject vibration_On;
    public GameObject vibration_Off;
    
    public GameObject restart;
    public TMP_Text coinText;
    public AudioSource buttonClickSound;
    public GameObject nextLevelButton;
    public GameObject noThanksButton;
    public AudioSource background;
    public AudioSource click;
    private const string SesKey = "SesDurumu";
    private const string TitresimKey = "TitresimDurumu";
    public player plyer;
    public GameObject rewarded;
    public GameObject coin;
    public GameObject radial_shine;
    public GameObject complete;
    private bool radialshine;
    public GameObject finishScreen;
    public AudioSource sounds;
    public GameObject rewardcoin;
  



    public GameObject startButton;

  
    public void FinishScreen()
    {
        StartCoroutine("FinishLaunch");
    }

    public IEnumerator FinishLaunch()
    {
        Time.timeScale = 0.5f;
        radialshine = true;
        finishScreen.SetActive(true);


        // Önce complete
        yield return new WaitForSecondsRealtime(0.1f);
        complete.SetActive(true);

        // Sonra radial_shine
        yield return new WaitForSecondsRealtime(0.2f);
        radial_shine.SetActive(true);
        sounds.Play();
        // Daha sonra coin
        yield return new WaitForSecondsRealtime(0.3f);
        coin.SetActive(true);

        // Ardýndan rewarded
        yield return new WaitForSecondsRealtime(0.4f);
        rewarded.SetActive(true);
        sounds.Play();
        // En son noThanksButton
        yield return new WaitForSecondsRealtime(1f);
        noThanksButton.SetActive(true);
        
    }
    private void Start()
    {
        
        // Kaydedilen ses durumunu yükle
        if (PlayerPrefs.HasKey(SesKey))
        {
            bool sesDurumu = PlayerPrefs.GetInt(SesKey) == 1;
            SetSesDurumu(sesDurumu);
        }

        // Kaydedilen titreþim durumunu yükle
        if (PlayerPrefs.HasKey(TitresimKey))
        {
            bool titresimDurumu = PlayerPrefs.GetInt(TitresimKey) == 1;
            SetTitresimDurumu(titresimDurumu);
        }
        
    }
    private void SetSesDurumu(bool durum)
    {
        ses_on.SetActive(!durum);
        ses_off.SetActive(durum);
        if (!durum)
        {
            if (click != null)
            {
                click.Stop();
            }
            if (background != null)
            {
                background.Stop();
                
            }
            if(sounds != null)
            {
                sounds.Stop();
            }
        }
     
    }

    private void SetTitresimDurumu(bool durum)
    {
        vibration_On.SetActive(!durum);
        vibration_Off.SetActive(durum);
    }
    private void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }
    }
    public void NoThanksButtonClicked()
    {
        // NoThanks butonuna basýldýðýnda
        nextLevelButton.SetActive(true); // NextLevel butonunu aktif hale getir
        noThanksButton.SetActive(false); // NoThanks butonunu devre dýþý býrak
       



    }
    public void reward()
    { noThanksButton.SetActive(false );
        nextLevelButton.SetActive(true);
        rewarded.SetActive(false);
        rewardcoin.SetActive(true);
      

    }

  
    public void Settings_Open()
    {
        
        settings_open.SetActive(false);
        settings_close.SetActive(true);
        layot.SetTrigger("slide_in");
        
        PlayButtonClickSound();
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            ses_on.SetActive(true);
            ses_off.SetActive(false);
            AudioListener.volume = 1;
        }
        else if (PlayerPrefs.GetInt("Sound") == 2)
        {
            ses_on.SetActive(false);
            ses_off.SetActive(true);
            AudioListener.volume = 0;
        }


        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            vibration_On.SetActive(true);
            vibration_Off.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("Vibration") == 2)
        {
            vibration_On.SetActive(false);
            vibration_Off.SetActive(true);
        }

    }

    public void RestartButtonActive()
    {
        restart.SetActive(true);
    }

    public void RestartScene()
    {
        Variables.firsttouch = 0;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt("Coins", plyer.coins);
        PlayerPrefs.SetInt("CurrentLevel", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();

    }

    public void NextScene()
    {
        Variables.firsttouch = 0;
        Time.timeScale = 1f;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int maxLevelIndex = 14; // Son seviye indeksi

        // Eðer mevcut indeks son seviye indeksi deðilse
        if (currentSceneIndex < maxLevelIndex)
        {
            // PlayerPrefs'teki LevelIndex deðerini kontrol et
            int levelIndex = PlayerPrefs.GetInt("LevelIndex", 0);

            // Eðer PlayerPrefs'teki LevelIndex deðeri mevcut indekse eþitse,
            // indeks artýrma iþlemi yapmadan doðrudan bir sonraki sahneye geç
            if (levelIndex == currentSceneIndex)
            {
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
            else
            {
                // Eðer deðilse, PlayerPrefs'teki LevelIndex deðeri ile mevcut indeksi eþitle
                PlayerPrefs.SetInt("LevelIndex", currentSceneIndex);
            }
        }
        else
        {
            // Son seviyeye ulaþýldýysa, 6. indeksteki sahneye geç
            PlayerPrefs.SetInt("LevelIndex", 6);
            SceneManager.LoadScene(6);
        }


    }




    public void Settings_Close()
    {
        settings_close.SetActive(false);
        settings_open.SetActive(true);
        layot.SetTrigger("slide_out");
        
      
       
        
    }

    public void Ses_on()
    {
        ses_on.SetActive(false);
        ses_off.SetActive(true);
        PlayerPrefs.SetInt("Sound", 2);

        // Ses dosyalarýný durdur
        if (background != null && background.isPlaying)
        {
            background.Stop();
        }
        if (click != null && click.isPlaying)
        {
            click.Stop();
        }
        if (buttonClickSound != null && buttonClickSound.isPlaying)
        {
            buttonClickSound.Stop();
        }

       
    }

    public void Ses_off()
    {
        ses_on.SetActive(true);
        ses_off.SetActive(false);
        PlayerPrefs.SetInt("Sound", 1);

        // Ses dosyalarýný çal
        if (background != null && !background.isPlaying)
        {
            background.Play();
        }
        if (click != null && !click.isPlaying)
        {
            click.Play();
        }
    }




    public void Vibration_On()
    {
        vibration_On.SetActive(false);
        vibration_Off.SetActive(true);
        PlayerPrefs.SetInt("Vibration", 2);
    }

    public void Vibration_Off()
    {
        vibration_On.SetActive(true);
        vibration_Off.SetActive(false);
        PlayerPrefs.SetInt("Vibration", 1);
    }



}
