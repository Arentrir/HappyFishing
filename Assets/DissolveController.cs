using UnityEngine;

public class DissolveController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Material material;
    public InventoryStorage inventoryStorage;

    private int dissolveAmount = Shader.PropertyToID("_DissolveAmount");
    public float dissolveTimer;
    public float dissolveDuration;
    public float dissolveStartTime;
    private float dissolveStartTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        dissolveTimer = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            if (dissolveTimer > dissolveDuration)
            {
                StartDissolve();
            }
        }
        if (Input.touchCount > 0)
        {
            if (dissolveTimer > dissolveDuration)
            {
                StartDissolve();
            }  
        }
        dissolveStartTimer += Time.deltaTime;
        if (dissolveStartTimer > dissolveStartTime) 
        {
            StartDissolve();
            dissolveStartTime = 9999;
        }
        if (dissolveTimer < dissolveDuration) 
        {
            dissolveTimer += Time.deltaTime;
            float t = Mathf.Clamp01(dissolveTimer / dissolveDuration);

         
            material.SetFloat(dissolveAmount, Mathf.Lerp(1.1f, 0, t));
            if (dissolveTimer >= dissolveDuration) 
            {
                this.gameObject.SetActive(false);   
            }
        }
    }
    public void StartDissolve()
    {
        dissolveTimer = 0.15f;
        inventoryStorage.LoadGameProfile(PlayerPrefs.GetInt("CurrentGameProfile", 1));
    }
}
