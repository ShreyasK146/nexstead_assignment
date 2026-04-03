using DG.Tweening;
using System;
using System.Collections;

using TMPro;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    [SerializeField] private CardSelection cardSelection;
    private int completedCards = 0;

    private Vector3[] trayslotFinal = {
    new Vector3(-0.75f, 1.796f, 1.16f),
    new Vector3(-0.75f, 1.995f, 1.16f),
    new Vector3(-0.75f, 2.242f, 1.16f),
    new Vector3(-0.75f, 2.456f, 1.16f),
    new Vector3(-0.75f, 2.676f, 1.16f),
    new Vector3(-0.75f, 2.904f, 1.16f),
};

    private Vector3[] trayslotEntry = {
    new Vector3(-0.75f, 2.430f, 0.05f),
    new Vector3(-0.75f, 2.628f, 0.05f),
    new Vector3(-0.75f, 2.876f, 0.05f),
    new Vector3(-0.75f, 3.090f, 0.05f),
    new Vector3(-0.75f, 3.304f, 0.05f),
    new Vector3(-0.75f, 3.534f, 0.05f),
};
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
                cardSelection.RefreshUnmatchedTray();
                cardSelection.RefreshDeck();

            }
        });



    }


    public void AnimateAllCardsOnBelt()
    {
        for (int i = 0; i < cardSelection.selectedCards.Count; i++)
        {
            int index = i;

            //no need animation for this 
            cardSelection.selectedCards[index].transform.localPosition = new Vector3(-4.4f, 1.28f, 0.35f);
            cardSelection.selectedCards[index].transform.localRotation = Quaternion.Euler(0, 90, 90);
            
            Debug.Log(GameManager.Instance.matchingTrayFound);
            if (!GameManager.Instance.matchingTrayFound)
            {
                
                
                DOVirtual.DelayedCall(i * 0.05f, () =>
                {
                    AnimateCardOnBelt(cardSelection.selectedCards[index]);
                });
            }
            else if (GameManager.Instance.matchingTrayFound)
            {
                GameManager.Instance.totalCardsMatched++;
                DOVirtual.DelayedCall(i * 0.05f, () =>
                {
                    AnimateCardOnBelt2(cardSelection.selectedCards[index], index);
                });
            }

        }
    }

    public void AnimateCardOnBelt(GameObject card)
    {

        Sequence seq2 = DOTween.Sequence();
        seq2.Append(card.transform.DOMove(new Vector3(4.4f, 1.28f, 0.35f), 2f).SetEase(Ease.Linear));

        int currentSlot = GameManager.Instance.unusedCardCount;
        GameManager.Instance.unusedCardCount++;

        seq2.Append(card.transform.DOMove(new Vector3(-3.2f + currentSlot * 0.34f, -0.75f, -0.1f), 0.4f)
            .SetEase(Ease.InCubic));
        cardSelection.RefreshUnmatchedTray();
        seq2.OnComplete(() =>
        {
            
            card.transform.SetParent(GameManager.Instance.unmatchedTrayTransform);
            completedCards++;
            if (completedCards == cardSelection.selectedCards.Count)
            {
                completedCards = 0;
                GameManager.Instance.cardsInBeltCount -= cardSelection.selectedCards.Count;
                GameEvents.Instance.CardsInBeltChanged(GameManager.Instance.cardsInBeltCount);

            }
        });

        GameManager.Instance.cardWasClicked = false;
        //just in case
        GameManager.Instance.matchingTrayTransform = null;
        GameManager.Instance.matchingTrayFound = false;

    }
    public void AnimateCardOnBelt2(GameObject card, int index)
    {
        float x = GameManager.Instance.matchingTrayTransform.position.x; // -0.75 or 0.75

        Vector3 entry = trayslotEntry[index];
        Vector3 final = trayslotFinal[index];

        // flip x if right side tray
        entry.x = x;
        final.x = x;

        Sequence seq2 = DOTween.Sequence();

        // move along belt to tray x position
        seq2.Append(card.transform.DOMove(new Vector3(x, 1.28f, 0.35f), 1f)
            .SetEase(Ease.Linear));

        // lift up and rotate
        seq2.Append(card.transform.DOMove(new Vector3(x, 1.28f, -5f), 0.3f)
            .SetEase(Ease.OutCubic));
        seq2.Join(card.transform.DORotate(new Vector3(60, 180, 0), 0.3f)
            .SetEase(Ease.OutCubic));

        // move to slot entry
        seq2.Append(card.transform.DOMove(entry, 0.3f)
            .SetEase(Ease.OutCubic));

        // push into slot
        seq2.Append(card.transform.DOMove(final, 0.25f)
            .SetEase(Ease.InCubic));
        seq2.OnComplete(() =>
        {
            completedCards++;
            if (completedCards == cardSelection.selectedCards.Count)
            {
                completedCards = 0;
                StartCoroutine(RemoveCompletedTray());
                GameManager.Instance.cardsInBeltCount -= cardSelection.selectedCards.Count;
                GameEvents.Instance.CardsInBeltChanged(GameManager.Instance.cardsInBeltCount);
            }
           
        });
    }

    private IEnumerator RemoveCompletedTray()
    {
        yield return new WaitForSeconds(1f);
        foreach(var card in cardSelection.selectedCards)
        {
            Destroy(card.gameObject);
        }
        Transform trayTransform = GameManager.Instance.matchingTrayTransform.parent;
        if(trayTransform.childCount > 1)
            trayTransform.GetChild(1).GetComponent<Transform>().position = GameManager.Instance.matchingTrayTransform.position;
        GameManager.Instance.matchingTrayTransform = null;
        Destroy(trayTransform.GetChild(0).gameObject);
        GameManager.Instance.cardWasClicked = false;
        GameManager.Instance.matchingTrayFound = false;
    }
}
