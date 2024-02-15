using UnityEditor.Animations;
using UnityEngine;

public class Pot : MonoBehaviour
{
    //Scale
    [SerializeField] private float smallAreaScale;

    //Drag and Drop in Scene
    private Vector3 originalPosition;
    private bool isDragging = false;

    //Drag and Drop from Shop
    private bool isPlacing = true;

    //Place on Holder
    private bool isOverHolder = false;
    private bool isPlaced = false;
    private PotHolder potHolder;
    
    //Spawn Plant in Pot
    [SerializeField] public Transform plantSpawnLocation;
    public bool isPotEmpty = true;
    public PlantSO plantInPot = null;
    private PotSO pot = null;

    //Plant Management & Stats
    public bool isWatered;

    private ShopManager shopManager;

    private void Awake()
    {
        shopManager = FindObjectOfType<ShopManager>();    
    }

    void Start()
    {
        originalPosition = transform.position;
        potHolder = null;
        transform.localScale = new Vector3(smallAreaScale, smallAreaScale, smallAreaScale);
    }

    void Update()
    {
        PlacePot();
        DragMovement();
    }

    public void SetPot(PotSO pot)
    {
        this.pot = pot;
    }

    private void DragMovement()
    {
        if (isDragging || isPlacing)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
    }

    private void PlacePot()
    {
        if (isPlacing && Input.GetMouseButtonUp(0))
        {
            isPlacing = false;
            if (isOverHolder && potHolder.isHolderEmpty)
            {
                if (shopManager.TryToBuySomething(pot.itemPrice))
                {
                    transform.position = potHolder.transform.position;
                    originalPosition = potHolder.transform.position;
                    potHolder.isHolderEmpty = false;
                    isPlaced = true;
                    Debug.Log("Pot bought for " + pot.itemPrice + " Coins.");
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void PlantSeed(PlantSO plant)
    {
        plantSpawnLocation.GetComponent<SpriteRenderer>().sprite = plant.seedSprite;
        plantSpawnLocation.GetComponent<Animator>().runtimeAnimatorController = plant.animatorController;
        plantInPot = plant;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        if (isPlaced)
        {
            potHolder.isHolderEmpty = true;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        if (isOverHolder && potHolder.isHolderEmpty)
        {
            transform.position = potHolder.transform.position;
            originalPosition = potHolder.transform.position;
            potHolder.isHolderEmpty = false;
            isPlaced = true;
        }
        else
        {
            transform.position = originalPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PotHolder>())
        {
            isOverHolder = true;
            potHolder = other.gameObject.GetComponent<PotHolder>();
        }

        if (other.CompareTag("ShelfArea"))
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PotHolder>())
        {
            isOverHolder = false;
            potHolder = null;
        }

        if (other.CompareTag("ShelfArea"))
        {
            transform.localScale = new Vector3(smallAreaScale, smallAreaScale, smallAreaScale);
        }
    }

    
}
