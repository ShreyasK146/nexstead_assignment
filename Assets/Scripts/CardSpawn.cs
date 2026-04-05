using UnityEngine;

public class CardSpawn : MonoBehaviour
{
    [SerializeField] private GameObject redCard;
    [SerializeField] private GameObject blueCard;

    [SerializeField] private int redCardCount = 6;
    [SerializeField] private int blueCardCount = 6;

    [SerializeField] private Transform Deck1;
    [SerializeField] private Transform Deck2;

    //GameObject cardPrefab;

    private void Start()
    {
        SpawnCards();
    }
    /*
     * Hard Coded Card Spawning in Deck
     */
    private void SpawnCards()
    {
        for (int i = 0; i < redCardCount; i++)
        {
            SpawnCard(redCard, Deck1, i);
        }
        for(int i = 0; i < redCardCount; i++)
        {
            SpawnCard(blueCard, Deck1, redCardCount + i);
        }
        for (int i = 0; i < blueCardCount; i++)
        {
            SpawnCard(blueCard, Deck2, i);
        }
        for (int i = 0; i < blueCardCount; i++)
        {
            SpawnCard(redCard, Deck2, blueCardCount + i);
        }
        GameManager.Instance.totalCardCount = 2 * (redCardCount + blueCardCount);

    }

    private void SpawnCard(GameObject cardPrefab, Transform deck, int index)
    {
        GameObject card = Instantiate(cardPrefab, deck);
        card.transform.localPosition = new Vector3(0, 0.1f - index * 0.01f, 3.5f - index * 0.5f);
        card.transform.localRotation = Quaternion.Euler(90, 0, 0);
        card.transform.localScale = new Vector3(7.5f, 2.15f, 0.25f);
    }

}
