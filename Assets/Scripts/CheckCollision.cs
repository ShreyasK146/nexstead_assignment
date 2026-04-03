using DG.Tweening;
using TMPro;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardsInBeltCount;
    int count = 0;

    

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "card")
    //    {
    //        cardsInBeltCount.text = ++count + "/" + 24;
    //        transform.DOPunchScale(Vector3.one * 0.11f, 0.1f, 5, 0.5f);
    //    }
    //}
}
