using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soda : MonoBehaviour
{
    //Scale
    [SerializeField] private float smallAreaScale;
    [SerializeField] private Vector3 shelfOffset;
    [SerializeField] private Vector3 noShelfOffset;
    [SerializeField] private ParticleSystem sodaSprayParticleSystem;

    //Drag and Drop in Scene
    private Vector3 originalPosition;
    private bool isDragging = false;

    private bool isOverPot = false;
    private Pot pot;

    private bool isWatering;

    private void Start()
    {
        originalPosition = transform.position;
        pot = null;
    }

    private void Update()
    {
        DragMovement();
    }

    private void DragMovement()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        if (isOverPot && !pot.isWatered)
        {
            //Watering
            StartCoroutine(WateringPlant());
        }
        else
        {
            transform.position = originalPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Pot>())
        {
            isOverPot = true;
            pot = other.gameObject.GetComponent<Pot>();
        }


        if (other.CompareTag("ShelfArea"))
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Pot>())
        {
            isOverPot = false;
            pot = null;
        }

        if (other.CompareTag("ShelfArea") && !isWatering)
        {
            transform.localScale = new Vector3(smallAreaScale, smallAreaScale, smallAreaScale);
        }
    }

    private IEnumerator WateringPlant()
    {
        GameEvents.Instance.WaterPlant.Invoke();

        pot.isWatered = true;
        isWatering = true;
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z += -120f;
        transform.eulerAngles = currentRotation;

        if(pot.isPotOnShelf)
        {
            transform.position = pot.transform.position + shelfOffset;
        } else
        {
            transform.position = pot.transform.position + noShelfOffset;
        }
        sodaSprayParticleSystem.Play();

        yield return new WaitForSeconds(1f);
        isWatering = false;
        transform.position = originalPosition;
        transform.eulerAngles = new Vector3(0f,0f,0f);
    }
}
