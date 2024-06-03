using UnityEngine;
using TMPro;

public class ObjectPlacementManager : MonoBehaviour
{
    public GameObject burger;         // Reference to the Burger GameObject
    public GameObject cola;           // Reference to the Cola GameObject
    public GameObject fries;          // Reference to the Fries GameObject

    public Transform burgerTarget;    // Target position for Burger
    public Transform colaTarget;      // Target position for Cola
    public Transform friesTarget;     // Target position for Fries

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
        if (other.gameObject == burger)
        {
            if (IsCloseToTarget(burger, burgerTarget))
            {
                burgerPlaced = true;
                burger.transform.position = burgerTarget.position;
                burger.GetComponent<Renderer>().material.color = Color.green; // Change color to green
                feedbackText.text = "Burger placed correctly!";
                Debug.Log("Burger placed correctly!");
            }
            else
            {
                feedbackText.text = "Burger placed incorrectly!";
                Debug.Log("Burger placed incorrectly!");
            }
        }
        else if (other.gameObject == cola)
        {
            if (IsCloseToTarget(cola, colaTarget))
            {
                colaPlaced = true;
                cola.transform.position = colaTarget.position;
                cola.GetComponent<Renderer>().material.color = Color.green; // Change color to green
                feedbackText.text = "Cola placed correctly!";
                Debug.Log("Cola placed correctly!");
            }
            else
            {
                feedbackText.text = "Cola placed incorrectly!";
                Debug.Log("Cola placed incorrectly!");
            }
        }
        else if (other.gameObject == fries)
        {
            if (IsCloseToTarget(fries, friesTarget))
            {
                friesPlaced = true;
                fries.transform.position = friesTarget.position;
                fries.GetComponent<Renderer>().material.color = Color.green; // Change color to green
                feedbackText.text = "Fries placed correctly!";
                Debug.Log("Fries placed correctly!");
            }
            else
            {
                feedbackText.text = "Fries placed incorrectly!";
                Debug.Log("Fries placed incorrectly!");
            }
        }

        CheckWinCondition();
    }

    private bool IsCloseToTarget(GameObject obj, Transform target)
    {
        float distance = Vector3.Distance(obj.transform.position, target.position);
        return distance < 1.0f; // You can adjust this threshold as needed
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
