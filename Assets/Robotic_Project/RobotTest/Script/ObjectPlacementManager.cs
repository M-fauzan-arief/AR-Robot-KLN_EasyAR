using UnityEngine;
using TMPro;

public class ObjectPlacementManager : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject burger;         // Reference to the Burger GameObject
    public GameObject cola;           // Reference to the Cola GameObject
    public GameObject fries;          // Reference to the Fries GameObject

    [Header("Targets")]
    public Transform burgerTarget;    // Target position and rotation for Burger
    public Transform colaTarget;      // Target position and rotation for Cola
    public Transform friesTarget;     // Target position and rotation for Fries

    [Header("Colliders")]
    public Trigger burgerCollider;   // Trigger collider for Burger target
    public Trigger colaCollider;     // Trigger collider for Cola target
    public Trigger friesCollider;    // Trigger collider for Fries target

    public TextMeshProUGUI feedbackText;  // UI Text for feedback

    private bool burgerPlaced = false;
    private bool colaPlaced = false;
    private bool friesPlaced = false;

    void Start()
    {
        feedbackText.text = "";
        Debug.Log("ObjectPlacementManager initialized. Ready to place objects.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == burger && other == burgerCollider)
        {
            PlaceObject(burger, burgerTarget);
            feedbackText.text = "Burger placed correctly!";
            Debug.Log("Burger placed correctly!");
        }
        else if (other.gameObject == cola && other == colaCollider)
        {
            PlaceObject(cola, colaTarget);
            feedbackText.text = "Cola placed correctly!";
            Debug.Log("Cola placed correctly!");
        }
        else if (other.gameObject == fries && other == friesCollider)
        {
            PlaceObject(fries, friesTarget);
            feedbackText.text = "Fries placed correctly!";
            Debug.Log("Fries placed correctly!");
        }
        else
        {
            feedbackText.text = "Object placed incorrectly!";
            Debug.Log("Object placed incorrectly!");
        }

        CheckWinCondition();
    }

    private void PlaceObject(GameObject obj, Transform target)
    {
        obj.transform.position = target.position;
        obj.transform.rotation = target.rotation;
        obj.GetComponent<Renderer>().material.color = Color.green;
        obj.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
        obj.transform.SetParent(target); // Attach to the target
    }

    private void CheckWinCondition()
    {
        if (burgerPlaced && colaPlaced && friesPlaced)
        {
            feedbackText.text = "You win! All objects placed correctly! 3 Stars!";
            Debug.Log("You win! All objects placed correctly! 3 Stars!");
            // Additional win logic such as awarding points or stars can go here
        }
    }
}
