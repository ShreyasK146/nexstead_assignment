using DG.Tweening;
using System;
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

    private void UpdateText(int count)
    {
        cardsInBeltCount.text = count + "/" + 24;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "card")
        {
            GameEvents.Instance.CardsInBeltChanged(++GameManager.Instance.cardsInBeltCount);
            transform.DOPunchScale(Vector3.one * 0.11f, 0.1f, 5, 0.5f);
        }
    }
}
