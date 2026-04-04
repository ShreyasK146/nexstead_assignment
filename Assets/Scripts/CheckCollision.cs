using DG.Tweening;
using TMPro;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardsInBeltCount;

    private void Start()
    {
        GameEvents.Instance.OnCardsInBeltChangedd += UpdateText;
    }
    private void OnDisable()
    {
        GameEvents.Instance.OnCardsInBeltChangedd -= UpdateText;
    }

    //if card goes inside belt then do a small animation effect and ask the gamevents to invoke the cardsinbeltchangedevent
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "card")
        {
            GameEvents.Instance.CardsInBeltChanged(++GameManager.Instance.cardsInBeltCount);
            transform.DOPunchScale(Vector3.one * 0.11f, 0.1f, 2, 0.5f); 
        }
    }
    // if cardsinbelt event is invoked update the text
    private void UpdateText(int count)
    {
        cardsInBeltCount.text = count + "/" + 24;
    }

}
