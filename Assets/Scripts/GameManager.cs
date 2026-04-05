using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool cardWasClicked = false;
    public bool matchingTrayFound = false;
    public int cardsInBeltCount = 0;
    public int totalCardsMatched = 0;
    public int totalCardCount = 0;
    public int unusedCardCount = 0;
    public string selectedCardColor = "";
    public string tray1sColor = "";
    public string tray2sColor = "";
    public Transform matchingTrayTransform;
    public Transform unmatchedTrayTransform;
    [SerializeField] private TextMeshProUGUI endGameText;

    private void Awake()
    {
        Instance = this;
    }

    /*
     * just to showcase if player won or lost 
     */
    private void Start()
    {
        Invoke("SubscribeToEvent", 2);
        
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnCardCountChanged -= CheckForGameEnd;
    }
    private void SubscribeToEvent()
    {
        GameEvents.Instance.OnCardCountChanged += CheckForGameEnd; // subscribing to event with a small delay to avoid null ref exception
    }
    private void CheckForGameEnd()
    {
        if (totalCardsMatched == totalCardCount)
        {
            StartCoroutine(WaitForMessageToBeDisplayed("YOU WIN! CONGRATULATIONS", Color.green));
        }
        else if (unusedCardCount >= 20)
        {
            StartCoroutine(WaitForMessageToBeDisplayed("YOU LOSE! BETTER LUCK NEXT TIME", Color.red));
        }
    }

    private IEnumerator WaitForMessageToBeDisplayed(string message, Color color)
    {
        yield return new WaitForSeconds(2);
        endGameText.text = message;
        endGameText.color = color;
        Time.timeScale = 0f;
    }
}
