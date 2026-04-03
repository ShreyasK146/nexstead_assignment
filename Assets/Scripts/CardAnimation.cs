using DG.Tweening;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    [SerializeField] private CardSelection cardSelection;
    private int completedCards = 0;


    private void Start()
    {
        GameEvents.Instance.OnCardWasClicked += CardSelected;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnCardWasClicked -= CardSelected;
    }

    private void CardSelected()
    {
        
        for (int i = 0; i < cardSelection.selectedCards.Count; i++)
        {
            int index = i;

            DOVirtual.DelayedCall(i * 0.25f, () => {
                AnimateCardToBelt(cardSelection.selectedCards[index]);
            });
        }
        //GameManager.Instance.cardWasClicked = false;
    }

    public void AnimateCardToBelt(GameObject card)
    {
        card.transform.SetParent(null);
        Sequence seq = DOTween.Sequence();

        seq.Append(card.transform.DOMove(new Vector3(card.transform.position.x, -1, -5), 0.3f)
            .SetEase(Ease.OutCubic));
        seq.Append(card.transform.DOMove(new Vector3(-5, 1.8f, -2), 0.5f)
            .SetEase(Ease.OutCubic));
        seq.Append(card.transform.DORotate(new Vector3(0, 90, 0), 0.3f) //rotate him to align with slot
            .SetEase(Ease.Linear));
        seq.Append(card.transform.DOMove(new Vector3(-5, 1.8f, 0.35f), 0.4f)
            .SetEase(Ease.InCubic));
        seq.OnComplete(() =>
        {
            completedCards++;
            if (completedCards == cardSelection.selectedCards.Count)
            {
                completedCards = 0;
                AnimateAllCardsOnBelt();
            }
        });
        


    }


    public void AnimateAllCardsOnBelt()
    {
        for (int i = 0; i < cardSelection.selectedCards.Count; i++)
        {
            int index = i;
            cardSelection.selectedCards[index].transform.localPosition = new Vector3(-4.4f, 1.28f, 0.35f);
            cardSelection.selectedCards[index].transform.localRotation = Quaternion.Euler(0, 90, 90);
            DOVirtual.DelayedCall(i * 0.05f, () => {
                AnimateCardOnBelt(cardSelection.selectedCards[index]);
            });
        }
    }

    public void AnimateCardOnBelt(GameObject card)
    {

        Sequence seq2 = DOTween.Sequence();
        seq2.Append(card.transform.DOMove(new Vector3(4.4f, 1.28f, 0.35f), 2f).SetEase(Ease.Linear));
    }
}
/*
 * some hardcode values
 * -4.4,1.28,0.35 with rotation 0,90,90
 * -0.75,1.28,0.35 or 0.75,1.28,0.35 
 * -0.75,1.28,-5
 * rotate 60,180,0
 * 
 * Vector3(-0.75,1.796,1.16) - Vector3(-0.75,2.430,0.05)
 * Vector3(-0.75,1.995,1.16) - Vector3(-0.75,2.628,0.05)
 * Vector3(-0.75,2.242,1.16) - Vector3(-0.75,2.876,0.05)
 * Vector3(-0.75,2.456,1.16) - Vector3(-0.75,3.090,0.05)
 * Vector3(-0.75,2.676,1.16) - Vector3(-0.75,3.304,0.05)
 * Vector3(-0.75,2.904,1.16) - Vector3(-0.75,3.534,0.05)
 * 
 * Vector3(-0.75,2.430,0.05)
 * Vector3(-0.75,2.628,0.05)
 * Vector3(-0.75,2.876,0.05)
 * Vector3(-0.75,3.090,0.05)
 * Vector3(-0.75,3.304,0.05)
 * Vector3(-0.75,3.534,0.05)
 * 
 * 4.4,1.28,0.35 with rotation 0,90,90
 * 5,1.575,-0.5
 * Vector3(-3.2,-0.75,-0.1) alway do x-0.34 
 */