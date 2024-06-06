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
    private float originalSpeed; // Orjinal hýzý saklamak için

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
    public TMP_Text coinText; // coins deðiþkenini ekledik
    public int coins;
    public AudioSource obstacleHitSound;
    public AudioSource background;
    public GameObject canvas;
    public List<Transform> atesList; // Ateþ objesini atayacaðýnýz deðiþken
    public float speedMultiplier = 2f; // Mumun erime hýzýný kaç kat artýracaðýnýzý belirleyen deðiþken
    public float triggerDistance = 2f; // Ateþe ne kadar yaklaþýlýnca hýzýn artýrýlacaðýný belirleyen deðiþken
    


    private bool obstacleHit = false; // Obstacle'a çarpýlýp çarpýlmadýðýný kontrol etmek için

    void Start()
    {
        // Daha önce kaydedilmiþ coins deðerini yükle, eðer yoksa varsayýlan deðeri 0 olarak al
        coins = PlayerPrefs.GetInt("Coins", 0);
       
        rb = GetComponent<Rigidbody>();
        originalSpeed = initialSpeed; // Orjinal hýzý baþlangýçta sakla

        // coinText'i baþlangýçta güncelle
        UpdateCoinText();
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
        
    }
    void FixedUpdate()
    {
        if (!gameStarted)
        {
            initialSpeed = originalSpeed; // initialSpeed'i orijinal hýz deðerine ayarla
        }
    }

    public void OnDestroy()
    {
        // Seviye deðiþtiðinde veya oyundan çýkýldýðýnda coins deðerini kaydet
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
    }

    // coinText'i güncellemek için yardýmcý fonksiyon
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
            // Eðer dokunulan nokta Canvas üzerinde deðilse ve oyun henüz baþlamamýþsa
            if (!IsPointerOverCanvas(Input.mousePosition) && !gameStarted)
            {
                // Oyunu baþlat
                gameStarted = true;
                rb.velocity = new Vector3(0, 0, initialSpeed);
            }
        }

        if (gameStarted)
        {
            // Mumun sürekli ileri gitmesi
            rb.velocity = new Vector3(0, 0, initialSpeed);

            // Dokunmatik ekran kontrolü
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

            // Mumun erimesi sadece oyun baþladýðýnda kontrol edilsin
            if (!finish.activeSelf) // Eðer finish paneli aktif deðilse
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
                // Bitiþ noktasýna ulaþýldýðýnda oyunun durmasýný saðla
                gameStarted = false;
                initialSpeed = 0f;
                alo.FinishScreen();
                rb.velocity = Vector3.zero;
               
            }
        

        // Canvas öðelerine dokunulduðunda oyunu durdurma kontrolü
         if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (IsPointerOverCanvas(touch.position))
                    {
                        // Eðer canvas öðelerine dokunulduysa oyunu durdur
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

        // En yakýn ateþe yaklaþýldýðýnda
        if (closestDistanceToFire <= triggerDistance)
        {
            // Mumun erime hýzýný artýr
            float newSpeed = scaleDecreaseRate * speedMultiplier;
            if (newSpeed > minYScale) // Eðer yeni hýz minimum ölçekten büyükse devam et
            {
                scaleDecreaseRate = newSpeed;
            }
            else
            {
                // Eðer yeni hýz minimum ölçeði geçerse minimum ölçeði kullan
                scaleDecreaseRate = minYScale;
            }
        }
        else
        {
            // Ateþe yaklaþýlmadýðýnda, normal erime hýzýný kullan
            scaleDecreaseRate = 0.05f; // veya baþka bir varsayýlan hýz deðeri
        }

       
       
    }

    bool IsPointerOverCanvas(Vector2 screenPosition)
    {
        // Mouse pozisyonunu al
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

        // Pointer event'i oluþtur
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // Sonuçlardan herhangi biri canvasa aitse true döndür
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponentInParent<Canvas>() != null)
            {
                return true;
            }
        }

        // Canvas üzerinde bir öðe yoksa false döndür
        return false;
    }










    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CandleCollect"))
        {
            // Çarptýðýnýz nesnenin ölçeðini artýr
            transform.localScale += new Vector3(0, 0.5f, 0);

            // Çarptýðýnýz nesneyi 0.5 saniyede kaybolmasýný saðla
            Destroy(other.gameObject, 0.01f);
        }
        else if (other.CompareTag("Booster"))
        {
            // Booster tag'ine ait bir nesneye çarparsanýz
            StartCoroutine(BoostSpeed(1.5f, 5f)); // Hýzý 1.5 katýna çýkar, 5 saniye sonra eski hýzýna döner

        }
        else if (other.CompareTag("Obstacle") && !obstacleHit)
        {
            // Obstacle tag'ine ait bir nesneye çarparsanýz
            obstacleHit = true;
            StartCoroutine(DecreaseScale(0.5f));// Ölçeði 0.5 azalt
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
         
            // Finish bölgesine ulaþýldýðýnda coins deðiþkenini 200 artýr
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
        initialSpeed *= multiplier; // Hýzý artýr
        yield return new WaitForSeconds(duration); // Belirtilen süre boyunca beklet
        initialSpeed = originalSpeed; // Hýzý eski haline getir
    }

    IEnumerator DecreaseScale(float decreaseAmount)
    {
        float currentScaleY = transform.localScale.y;

        // Sadece bir kez çalýþmasýný saðlamak için
        bool hasDecreased = false;

        while (currentScaleY > minYScale && !hasDecreased)
        {
            transform.localScale = new Vector3(1.35f, currentScaleY - decreaseAmount, 1.35f);
            yield return null;
            currentScaleY = transform.localScale.y;

            // Coroutine'in bir kez çalýþmasýný saðla
            hasDecreased = true;
        }

        obstacleHit = false; // Obstacle'a çarpma durumunu sýfýrla
    }
}
