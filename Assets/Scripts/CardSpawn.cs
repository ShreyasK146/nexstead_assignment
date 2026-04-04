using UnityEngine;

public class CardSpawn : MonoBehaviour
{
    [SerializeField] private GameObject redCard;
    [SerializeField] private GameObject blueCard;

    [SerializeField] private int redCardCount = 6;
    [SerializeField] private int blueCardCount = 6;

    [SerializeField] private Transform Deck1;
    [SerializeField] private Transform Deck2;

    GameObject cardPrefab;

    private void Start()
    {
        SpawnCards();
    }
    /*
     * Hard Coded Card Spawning in Deck
     */
    private void SpawnCards()
    {
        for (int i = 0; i < redCardCount*2; i++)
        {
            if (i < redCardCount)
            {
                cardPrefab = Instantiate(redCard, Deck1.transform);
                cardPrefab.transform.localPosition = new Vector3(0, 0.1f - i * 0.01f, 3.5f - i * 0.5f);
                cardPrefab.transform.localRotation = Quaternion.Euler(90, 0, 0);
                cardPrefab.transform.localScale = new Vector3(7.5f, 2.15f, 0.25f);
            }
            else
            {
                cardPrefab = Instantiate(blueCard, Deck1.transform);
                cardPrefab.transform.localPosition = new Vector3(0, 0.1f - i * 0.01f, 3.5f - i * 0.5f);
                cardPrefab.transform.localRotation = Quaternion.Euler(90, 0, 0);
                cardPrefab.transform.localScale = new Vector3(7.5f, 2.15f, 0.25f);
            }

            GameManager.Instance.totalCardCount++; // absoluetly not necessary to do this now. but for future if automation is done (we can just simply assingn totalcount = 24)
        }
        for (int i = 0; i < blueCardCount*2; i++)
        {
            if (i < blueCardCount)
            {
                cardPrefab = Instantiate(blueCard, Deck2.transform);
                cardPrefab.transform.localPosition = new Vector3(0, 0.1f - i * 0.01f, 3.5f - i * 0.5f);
                cardPrefab.transform.localRotation = Quaternion.Euler(90, 0, 0);
                cardPrefab.transform.localScale = new Vector3(7.5f, 2.15f, 0.25f);
            }
            else
            {
                cardPrefab = Instantiate(redCard, Deck2.transform);
                cardPrefab.transform.localPosition = new Vector3(0, 0.1f - i * 0.01f, 3.5f - i * 0.5f);
                cardPrefab.transform.localRotation = Quaternion.Euler(90, 0, 0);
                cardPrefab.transform.localScale = new Vector3(7.5f, 2.15f, 0.25f);
            }
            GameManager.Instance.totalCardCount++;
        }
    }


}
