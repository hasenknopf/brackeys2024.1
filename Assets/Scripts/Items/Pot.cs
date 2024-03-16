using System;
using UnityEditor;
using UnityEngine;

public class Pot : MonoBehaviour
{

    //Manager
    private GameManager gameManager;
    private ShopManager shopManager;

    //Scale
    [SerializeField] private float smallAreaScale;
    [SerializeField] private float mouseOffset;

    //Drag and Drop in Scene
    private Vector3 originalPosition;
    private bool isDragging = false;

    //Drag and Drop from Shop
    private bool isPlacing = true;

    //Place on Holder
    public bool isOverHolder = false;
    private bool isPlaced = false;
    public PotHolder potHolder;
    public bool isOnSellHolder;

    //Spawn Plant in Pot
    [SerializeField] public Transform plantSpawnLocation;
    public bool isPotEmpty = true;
    public PlantSO plantInPot = null;
    private PotSO pot = null;
    private Animator plantAnimator;
    private SpriteRenderer plantSpriteRenderer;

    //Plant Management & Stats
    private int validDays;
    public bool isWatered;
    public bool isDried;
    public bool isGrown;
    [SerializeField] Sprite driedSprite;

    //Shop
    private int sellPrice = 1;
    public SellPlantHolder sellPlantHolder;

    [HideInInspector]public bool isPotOnShelf;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        shopManager = ShopManager.Instance;    
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
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    //Watering
        //    isWatered = true;
        //}
    }

    public void CheckForGrowing()
    {
        if(plantInPot != null && !isGrown && !isDried)
        {
            if (validDays >= plantInPot.daysToGrow)
            {
                Grow();
            }
        }
    }
    public void ChangeDay()
    {
        if (plantInPot != null)
        {
            if (isWatered)
            {
                validDays++;
            }
            else
            { 
                Dried();
            }
            isWatered = false;
        }
        CheckForGrowing();
    }

    private void Dried()
    {
        isDried = true;
        isGrown = false;
        plantAnimator.runtimeAnimatorController = null;
        plantSpriteRenderer.sprite = driedSprite;
        //plantInPot = null;
    }

    public void Watering()
    {
        if (!isWatered)
        {
            isWatered = true;
        }
    }

    public void ResetPot()
    {
        plantInPot = null;

        isPotEmpty = true;

        isWatered = false;
        isDried = false;
        isGrown = false;

        validDays = 0;

        if (plantAnimator != null)
        {
            plantAnimator.runtimeAnimatorController = null;
            plantAnimator = null;
        }

        if(plantSpriteRenderer != null)
        {
            plantSpriteRenderer.sprite = null;
            plantSpriteRenderer = null;
        }
    }

    private void Grow()
    {
        isGrown = true;
        plantSpawnLocation.GetComponent<SpriteRenderer>().sprite = plantInPot.grownSprite;
        if(plantSpawnLocation.GetComponent<Animator>().runtimeAnimatorController != null)
        {
            plantSpawnLocation.GetComponent<Animator>().SetTrigger("Grow");
        }
        
    }

    private void Clue()
    {
        if (plantSpawnLocation.GetComponent<Animator>().runtimeAnimatorController != null)
        {
            plantSpawnLocation.GetComponent<Animator>().SetTrigger("Clue");
        }
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
            transform.position = new Vector3(mousePosition.x, mousePosition.y - mouseOffset, transform.position.z);
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
                    ShopManager.Instance.BuyPot();
                    GameEvents.Instance.BuyItem.Invoke();
                    GameEvents.Instance.DropPot.Invoke();
                }
                else
                {
                    GameEvents.Instance.Error.Invoke();
                    Destroy(gameObject);
                }
            }
            else
            {
                GameEvents.Instance.Error.Invoke();
                Destroy(gameObject);  
            }
        }
    }

    public void PlantSeed(PlantSO plant)
    {
        GameEvents.Instance.SowPlant.Invoke();

        plantInPot = plant;
        plantSpriteRenderer = plantSpawnLocation.GetComponent<SpriteRenderer>();
        plantAnimator = plantSpawnLocation.GetComponent<Animator>();

        plantSpriteRenderer.sprite = plant.seedSprite;
        if (plant.animatorController != null)
        {
            plantAnimator.runtimeAnimatorController = plant.animatorController;
        }
    }

    private void OnMouseDown()
    {
        //Debug.Log("isDragging: " + isDragging);
        //Debug.Log("isPlaced: " + isPlaced);
        //Debug.Log("potHolder: " + potHolder);
        //Debug.Log("isHolderEmpty: " + potHolder.isHolderEmpty);
        GameEvents.Instance.DragPot.Invoke();

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
            isOverHolder = true;
            isPlaced = true;
            GameEvents.Instance.DropPot.Invoke();
        }
        else
        {
            transform.position = originalPosition;
            GameEvents.Instance.DropPot.Invoke();
        }
    }

    public void SetNewPotHolder(PotHolder holder)
    {
        transform.position = holder.transform.position;
        originalPosition = holder.transform.position;
        potHolder = holder;
        holder.isHolderEmpty = false;
        isOverHolder = true;

        GameEvents.Instance.DropPot.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PotHolder>())
        {
            isOverHolder = true;
            potHolder = other.gameObject.GetComponent<PotHolder>();
        }

        if (other.gameObject.GetComponent<SellPlantHolder>())
        {
            if(plantInPot != null)
            {
                sellPlantHolder = other.gameObject.GetComponent<SellPlantHolder>();
                SetPrice(plantInPot);
                sellPlantHolder.SetInfo(plantInPot.itemName, sellPrice);
                sellPlantHolder.SetPot(this);
                sellPlantHolder.OpenMenu();
                isOnSellHolder = true;
            }          

            isOverHolder = true;
            potHolder = other.gameObject.GetComponent<PotHolder>();
        }

        if (other.CompareTag("ShelfArea"))
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            isPotOnShelf = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PotHolder>())
        {
            isOverHolder = true;
            potHolder = other.gameObject.GetComponent<PotHolder>();
        }

        if (other.gameObject.GetComponent<SellPlantHolder>())
        {
            if (plantInPot != null)
            {
                sellPlantHolder = other.gameObject.GetComponent<SellPlantHolder>();
                SetPrice(plantInPot);
                sellPlantHolder.SetInfo(plantInPot.itemName, sellPrice);
                sellPlantHolder.SetPot(this);
                sellPlantHolder.OpenMenu();
                isOnSellHolder = true;
            }

            isOverHolder = true;
            potHolder = other.gameObject.GetComponent<PotHolder>();
        }

        if (other.CompareTag("ShelfArea"))
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            isPotOnShelf = true;
        }
    }

    private void SetPrice(PlantSO plant)
    {
        if(plant == null) return;

        if (!isGrown) sellPrice = plant.seedPrice;
        if (isGrown) sellPrice = plant.grownPrice;

        if (isDried) sellPrice = plant.dryPrice;
    }

    public void SellPlantInPot()
    {
        GameEvents.Instance.SellPlant.Invoke();

        shopManager.AddCoins(sellPrice);
        ResetPot();
        if(sellPlantHolder != null)
        {
            sellPlantHolder.CloseMenu();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PotHolder>())
        {
            isOverHolder = false;
            potHolder = null;
        }

        if (other.gameObject.GetComponent<SellPlantHolder>())
        {
            sellPlantHolder.CloseMenu();
            sellPlantHolder.SetPot(null);
            isOnSellHolder = false;
            isOverHolder = false;
            potHolder = null;
            sellPlantHolder = null;
        }

        if (other.CompareTag("ShelfArea"))
        {
            transform.localScale = new Vector3(smallAreaScale, smallAreaScale, smallAreaScale);
            isPotOnShelf = false;
        }
    }
    
}
