using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{

    //Scale size of plant
    [SerializeField] private float smallAreaScale;

    //Drag and Drop from Shop
    private bool isPlacing = true;
    private bool isOverPot = false;
    private Pot plantPot;

    private SpriteRenderer spriteRenderer;
    private PlantSO plant;

    private ShopManager shopManager;

    private void Awake()
    {
        shopManager = FindObjectOfType<ShopManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        transform.localScale = new Vector3(smallAreaScale, smallAreaScale, smallAreaScale);
    }

    public void SetInfo(PlantSO plant)
    {
        spriteRenderer.sprite = plant.seedSprite;
        this.plant = plant;
    }

    void Update()
    {
        if (isPlacing && Input.GetMouseButtonUp(0))
        {
            isPlacing = false;
            if (isOverPot && plantPot.isPotEmpty)
            { 
                if (shopManager.TryToBuySomething(plant.itemPrice))
                {
                    plantPot.isPotEmpty = false;
                    plantPot.PlantSeed(plant);
                    GameEvents.Instance.BuyItem.Invoke();

                    Destroy(gameObject);
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

        if (isPlacing)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Pot>())
        {
            isOverPot = true;
            plantPot = other.gameObject.GetComponent<Pot>();
        }
        if (other.CompareTag("ShelfArea"))
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Pot>())
        {
            isOverPot = false;
            plantPot = null;
        }

        if (other.CompareTag("ShelfArea"))
        {
            transform.localScale = new Vector3(smallAreaScale, smallAreaScale, smallAreaScale);
        }
    }
}
