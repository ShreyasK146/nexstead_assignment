using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool cardWasClicked = false;
    public string selectedCardColor = "";
    public string tray1sColor = "";
    public string tray2sColor = "";
    public Transform matchingTrayTransform;
    public bool matchingTrayFound = false;
    public Transform unmatchedTrayTransform;
    public int cardsInBeltCount = 0;

    public int totalCardsMatched = 0;
    public int totalCardCount = 0;
    public int unusedCardCount = 0;
    [SerializeField] private TextMeshProUGUI endGameText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InvokeRepeating("CheckForGameEnd", 1, 1);
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
        yield return new WaitForSeconds(4);
        endGameText.text = message;
        endGameText.color = color;
        Time.timeScale = 0f;
    }
}
