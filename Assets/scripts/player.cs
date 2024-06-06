using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections.Generic;
using TMPro;

public class player : MonoBehaviour
{
    private bool gameStarted = false;
    private Rigidbody rb;
    private float originalSpeed; // Orjinal h�z� saklamak i�in

    public float initialSpeed = 10f;
    public float moveSpeed = 3f;
  
    [SerializeField] private float scaleDecreaseRate = 0.2f;
    [SerializeField] private float minYScale = 0.01f;
    public GameObject basla;
    public GameObject el;
    
    public GameObject shop;
    public alo alo;
   
   
    public GameObject fire;
    public GameObject finish;
    public TMP_Text coinText; // coins de�i�kenini ekledik
    public int coins;
    public AudioSource obstacleHitSound;
    public AudioSource background;
    public GameObject canvas;
    public List<Transform> atesList; // Ate� objesini atayaca��n�z de�i�ken
    public float speedMultiplier = 2f; // Mumun erime h�z�n� ka� kat art�raca��n�z� belirleyen de�i�ken
    public float triggerDistance = 2f; // Ate�e ne kadar yakla��l�nca h�z�n art�r�laca��n� belirleyen de�i�ken
    


    private bool obstacleHit = false; // Obstacle'a �arp�l�p �arp�lmad���n� kontrol etmek i�in

    void Start()
    {
        // Daha �nce kaydedilmi� coins de�erini y�kle, e�er yoksa varsay�lan de�eri 0 olarak al
        coins = PlayerPrefs.GetInt("Coins", 0);
       
        rb = GetComponent<Rigidbody>();
        originalSpeed = initialSpeed; // Orjinal h�z� ba�lang��ta sakla

        // coinText'i ba�lang��ta g�ncelle
        UpdateCoinText();
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
        
    }
    void FixedUpdate()
    {
        if (!gameStarted)
        {
            initialSpeed = originalSpeed; // initialSpeed'i orijinal h�z de�erine ayarla
        }
    }

    public void OnDestroy()
    {
        // Seviye de�i�ti�inde veya oyundan ��k�ld���nda coins de�erini kaydet
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
    }

    // coinText'i g�ncellemek i�in yard�mc� fonksiyon
   public void UpdateCoinText()
    {
        coinText.text = coins.ToString();
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // E�er dokunulan nokta Canvas �zerinde de�ilse ve oyun hen�z ba�lamam��sa
            if (!IsPointerOverCanvas(Input.mousePosition) && !gameStarted)
            {
                // Oyunu ba�lat
                gameStarted = true;
                rb.velocity = new Vector3(0, 0, initialSpeed);
            }
        }

        if (gameStarted)
        {
            // Mumun s�rekli ileri gitmesi
            rb.velocity = new Vector3(0, 0, initialSpeed);

            // Dokunmatik ekran kontrol�
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                basla.SetActive(false);
                el.SetActive(false);
               
                shop.SetActive(false);
                

                if (touch.phase == TouchPhase.Moved)
                {
                    // X ekseninde hareket ettirme
                    float horizontalInput = touch.deltaPosition.x / Screen.width;
                    transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);
                }
            }

            // Mumun erimesi sadece oyun ba�lad���nda kontrol edilsin
            if (!finish.activeSelf) // E�er finish paneli aktif de�ilse
            {
                // Mumun erimesi
                float currentScaleY = transform.localScale.y;
                if (currentScaleY > minYScale)
                {
                    transform.localScale = new Vector3(1.35f, currentScaleY - scaleDecreaseRate * 2f * Time.deltaTime, 1.35f);
                }
                else
                {
                    alo.RestartButtonActive();
                    initialSpeed = 0f;
                    moveSpeed = 0f;
                    fire.SetActive(false);
                }
            }
            else
            {
                // Biti� noktas�na ula��ld���nda oyunun durmas�n� sa�la
                gameStarted = false;
                initialSpeed = 0f;
                alo.FinishScreen();
                rb.velocity = Vector3.zero;
               
            }
        

        // Canvas ��elerine dokunuldu�unda oyunu durdurma kontrol�
         if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (IsPointerOverCanvas(touch.position))
                    {
                        // E�er canvas ��elerine dokunulduysa oyunu durdur
                        gameStarted = false;
                        initialSpeed = 0f;
                        rb.velocity = Vector3.zero;
                    }
                }
            }
        }

        float closestDistanceToFire = float.MaxValue;
        foreach (Transform ates in atesList)
        {
            float distanceToFire = Vector3.Distance(transform.position, ates.position);
            if (distanceToFire < closestDistanceToFire)
            {
                closestDistanceToFire = distanceToFire;
            }
        }

        // En yak�n ate�e yakla��ld���nda
        if (closestDistanceToFire <= triggerDistance)
        {
            // Mumun erime h�z�n� art�r
            float newSpeed = scaleDecreaseRate * speedMultiplier;
            if (newSpeed > minYScale) // E�er yeni h�z minimum �l�ekten b�y�kse devam et
            {
                scaleDecreaseRate = newSpeed;
            }
            else
            {
                // E�er yeni h�z minimum �l�e�i ge�erse minimum �l�e�i kullan
                scaleDecreaseRate = minYScale;
            }
        }
        else
        {
            // Ate�e yakla��lmad���nda, normal erime h�z�n� kullan
            scaleDecreaseRate = 0.05f; // veya ba�ka bir varsay�lan h�z de�eri
        }

       
       
    }

    bool IsPointerOverCanvas(Vector2 screenPosition)
    {
        // Mouse pozisyonunu al
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

        // Pointer event'i olu�tur
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // Sonu�lardan herhangi biri canvasa aitse true d�nd�r
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponentInParent<Canvas>() != null)
            {
                return true;
            }
        }

        // Canvas �zerinde bir ��e yoksa false d�nd�r
        return false;
    }










    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CandleCollect"))
        {
            // �arpt���n�z nesnenin �l�e�ini art�r
            transform.localScale += new Vector3(0, 0.5f, 0);

            // �arpt���n�z nesneyi 0.5 saniyede kaybolmas�n� sa�la
            Destroy(other.gameObject, 0.01f);
        }
        else if (other.CompareTag("Booster"))
        {
            // Booster tag'ine ait bir nesneye �arparsan�z
            StartCoroutine(BoostSpeed(1.5f, 5f)); // H�z� 1.5 kat�na ��kar, 5 saniye sonra eski h�z�na d�ner

        }
        else if (other.CompareTag("Obstacle") && !obstacleHit)
        {
            // Obstacle tag'ine ait bir nesneye �arparsan�z
            obstacleHit = true;
            StartCoroutine(DecreaseScale(0.5f));// �l�e�i 0.5 azalt
            if (obstacleHitSound != null)
            {
                obstacleHitSound.Play();
            }
            if (PlayerPrefs.GetInt("Vibration") == 1)
            {
                Vibration.Vibrate(50);
            }
            else if (PlayerPrefs.GetInt("Vibration") == 2)
            {
                Debug.Log("no vibration");
            }

        }
        if (other.gameObject.CompareTag("Finish"))
        {
         
            // Finish b�lgesine ula��ld���nda coins de�i�kenini 200 art�r
            coins += 100;
            alo.FinishScreen();
          
            gameStarted = false;
            initialSpeed = 0f;
            rb.velocity = Vector3.zero;
            moveSpeed = 0f;
            background.Stop();
            UpdateCoinText();
            PlayerPrefs.SetInt("Coins", coins);
            PlayerPrefs.Save();
           
          





        }

    }

    IEnumerator BoostSpeed(float multiplier, float duration)
    {
        initialSpeed *= multiplier; // H�z� art�r
        yield return new WaitForSeconds(duration); // Belirtilen s�re boyunca beklet
        initialSpeed = originalSpeed; // H�z� eski haline getir
    }

    IEnumerator DecreaseScale(float decreaseAmount)
    {
        float currentScaleY = transform.localScale.y;

        // Sadece bir kez �al��mas�n� sa�lamak i�in
        bool hasDecreased = false;

        while (currentScaleY > minYScale && !hasDecreased)
        {
            transform.localScale = new Vector3(1.35f, currentScaleY - decreaseAmount, 1.35f);
            yield return null;
            currentScaleY = transform.localScale.y;

            // Coroutine'in bir kez �al��mas�n� sa�la
            hasDecreased = true;
        }

        obstacleHit = false; // Obstacle'a �arpma durumunu s�f�rla
    }
}
