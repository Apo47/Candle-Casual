using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public player uimanager;

    public GameObject particle;
    public GameObject particle2;
    public GameObject particle3;
    public GameObject particle4;

    public Sprite YellowImage;
    public Sprite GreenImage;

    public GameObject Item;
    public GameObject Item2;
    public GameObject Item3;
    public GameObject Item4;

    public GameObject Lock1;
    public GameObject Lock2;
    public GameObject Lock3;
    public TMP_Text coinText;




   
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("itemselect"))
            PlayerPrefs.SetInt("itemselect", 0);

        int selectedItem = PlayerPrefs.GetInt("itemselect");
        switch (selectedItem)
        {
            case 0:
                Item1Open();
                break;
            case 1:
                Item2Open();
                break;
            case 2:
                Item3Open();
                break;
            case 3:
                Item4Open();
                break;
        }

        if (!PlayerPrefs.HasKey("lock1control"))
            PlayerPrefs.SetInt("lock1control", 0);

        if (!PlayerPrefs.HasKey("lock2control"))
            PlayerPrefs.SetInt("lock2control", 0);

        if (!PlayerPrefs.HasKey("lock3control"))
            PlayerPrefs.SetInt("lock3control", 0);

        if (PlayerPrefs.GetInt("lock1control") == 1)
            Lock1.SetActive(false);

        if (PlayerPrefs.GetInt("lock2control") == 1)
            Lock2.SetActive(false);

        if (PlayerPrefs.GetInt("lock3control") == 1)
            Lock3.SetActive(false);
    }

    public void Item1Open()
    {
        particle.SetActive(true);
        particle2.SetActive(false);
        particle3.SetActive(false);
        particle4.SetActive(false);

        Item.GetComponent<Image>().sprite = GreenImage;
        Item2.GetComponent<Image>().sprite = YellowImage;
        Item3.GetComponent<Image>().sprite = YellowImage;
        Item4.GetComponent<Image>().sprite = YellowImage;

        PlayerPrefs.SetInt("itemselect", 0);
    }

    public void Item2Open()
    {
        particle.SetActive(false);
        particle2.SetActive(true);
        particle3.SetActive(false);
        particle4.SetActive(false);

        Item.GetComponent<Image>().sprite = YellowImage;
        Item2.GetComponent<Image>().sprite = GreenImage;
        Item3.GetComponent<Image>().sprite = YellowImage;
        Item4.GetComponent<Image>().sprite = YellowImage;

        PlayerPrefs.SetInt("itemselect", 1);
    }

    public void Item3Open()
    {
        particle.SetActive(false);
        particle2.SetActive(false);
        particle3.SetActive(true);
        particle4.SetActive(false);

        Item.GetComponent<Image>().sprite = YellowImage;
        Item2.GetComponent<Image>().sprite = YellowImage;
        Item3.GetComponent<Image>().sprite = GreenImage;
        Item4.GetComponent<Image>().sprite = YellowImage;

        PlayerPrefs.SetInt("itemselect", 2);
    }

    public void Item4Open()
    {
        particle.SetActive(false);
        particle2.SetActive(false);
        particle3.SetActive(false);
        particle4.SetActive(true);

        Item.GetComponent<Image>().sprite = YellowImage;
        Item2.GetComponent<Image>().sprite = YellowImage;
        Item3.GetComponent<Image>().sprite = YellowImage;
        Item4.GetComponent<Image>().sprite = GreenImage;

        PlayerPrefs.SetInt("itemselect", 3);
    }

    //---------------------------LOCKS---------------------------

    public void Lock1Open()
    {
        int coins = uimanager.coins; // Player scriptinden coin miktarýný al
        int lock1control = PlayerPrefs.GetInt("lock1control", 0);

        if (coins >= 2000 && lock1control == 0)
        {
            coins -= 2000; // Kilidi açmak için gerekli olan parayý düþür
            uimanager.coins = coins; 
            Lock1.SetActive(false);
            PlayerPrefs.SetInt("lock1control", 1); 
            uimanager.UpdateCoinText(); 
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Yetersiz coin veya kilidin zaten açýk!");
        }
    }

    public void Lock2Open()
    {
        int coins = uimanager.coins;
        int lock2control = PlayerPrefs.GetInt("lock2control");
        if (coins >= 5000 && lock2control == 0)
        {
            coins -= 5000; 
            uimanager.coins = coins;
            Lock2.SetActive(false);
            PlayerPrefs.SetInt("lock2control", 1);
            uimanager.UpdateCoinText();
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Yetersiz coin veya kilidin zaten açýk!");
        }
    }

    public void Lock3Open()
    {
        int coins = uimanager.coins;
        int lock3control = PlayerPrefs.GetInt("lock3control");
        if (coins >= 10000 && lock3control == 0)
        {
            coins -= 10000; 
            uimanager.coins = coins;
            Lock3.SetActive(false);
            PlayerPrefs.SetInt("lock3control", 1);
            uimanager.UpdateCoinText();
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Yetersiz coin veya kilidin zaten açýk!");
        }
    }
}
