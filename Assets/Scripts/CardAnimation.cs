using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    [SerializeField] private CardSelection cardSelection;
    
    private int completedCards = 0;

    //hardcoded values for cards to go into matched tray
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
            //each call should have some delay to showcase the animation of different cards
            DOVirtual.DelayedCall(i * 0.25f, () => {
                AnimateCardToBelt(cardSelection.selectedCards[index]);
            });
        }
    }

    //aniamte cards to to the begining of belt
    public void AnimateCardToBelt(GameObject card)
    {
        card.transform.SetParent(null);
        Sequence seq = DOTween.Sequence();//sequencee is used to hold multiple tweens which plays one after another

        seq.Append(card.transform.DOMove(new Vector3(card.transform.position.x, -1, -5), 0.3f) // 0.3f represents how fast/duration animation should play
            .SetEase(Ease.OutCubic));
        seq.Append(card.transform.DOMove(new Vector3(-5, 1.8f, -2), 0.5f)
            .SetEase(Ease.InOutCubic));
        seq.Append(card.transform.DORotate(new Vector3(0, 90, 0), 0.3f) //rotate him to align with slot
            .SetEase(Ease.OutCubic));
        seq.Append(card.transform.DOMove(new Vector3(-5, 1.8f, 0.35f), 0.4f)
            .SetEase(Ease.InCubic));
        seq.OnComplete(() =>
        {
            completedCards++;
            //call this once all tweens completed
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

            //no need animation for this because z is behind other object
            cardSelection.selectedCards[index].transform.localPosition = new Vector3(-4.4f, 1.28f, 0.35f);
            cardSelection.selectedCards[index].transform.localRotation = Quaternion.Euler(0, 90, 90);
            
            
            if (!GameManager.Instance.matchingTrayFound)
            {
               
                DOVirtual.DelayedCall(i * 0.05f, () =>
                {
                    AnimateCardOnBeltToUnmatchedTray(cardSelection.selectedCards[index]);
                });
            }
            else if (GameManager.Instance.matchingTrayFound)
            {
                GameManager.Instance.totalCardsMatched++;
                
                DOVirtual.DelayedCall(i * 0.05f, () =>
                {
                    // index + thing.. this is just hard coding for another scenario which was not necessary for current level but now its need because slotsmatched is needed to destroy cards
                    AnimateCardToMatchingTray(cardSelection.selectedCards[index], index + GameManager.Instance.matchingTrayTransform.GetComponent<TrayData>().slotsmatched); 

                });
            }

        }
    }

    public void AnimateCardOnBeltToUnmatchedTray(GameObject card)
    {

        Sequence seq2 = DOTween.Sequence();
        seq2.Append(card.transform.DOMove(new Vector3(4.4f, 1.28f, 0.35f), 2f).SetEase(Ease.Linear)); // animate card to end of belt

        int currentSlot = GameManager.Instance.unusedCardCount;
        GameManager.Instance.unusedCardCount++; // keep track of unusedCardCount

        //so each card should be seperated via certain x axis on unusedcardtransform(hardcoding)
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
                GameEvents.Instance.CardsInBeltChanged(GameManager.Instance.cardsInBeltCount); // inform gameevents on cardsinbeltchange

            }
        });

        GameManager.Instance.cardWasClicked = false;

        //just in case
        GameManager.Instance.matchingTrayTransform = null;
        GameManager.Instance.matchingTrayFound = false;

    }
    public void AnimateCardToMatchingTray(GameObject card, int index)
    {
        float x = GameManager.Instance.matchingTrayTransform.position.x; // -0.75 or 0.75 (hard coded either left or right tray x pos)

        Vector3 entry = trayslotEntry[index];
        Vector3 final = trayslotFinal[index];

        // flip x if right side tray (left side no need to worry hardcoded comes with -0.75 x value)
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
            GameManager.Instance.matchingTrayTransform.GetComponent<TrayData>().matchedCards.Add(card); // fill up matched cards 
            if (completedCards == cardSelection.selectedCards.Count)
            {
                GameManager.Instance.matchingTrayTransform.GetComponent<TrayData>().slotsmatched += completedCards;
                Debug.Log(GameManager.Instance.matchingTrayTransform.GetComponent<TrayData>().slotsmatched);
                completedCards = 0;
                StartCoroutine(RemoveCompletedTray()); // remove the mathcing tray if slot is full and update transform and other things
                GameManager.Instance.cardsInBeltCount -= cardSelection.selectedCards.Count;
                GameEvents.Instance.CardsInBeltChanged(GameManager.Instance.cardsInBeltCount);
            }
           
        });
    }

    private IEnumerator RemoveCompletedTray()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log(GameManager.Instance.matchingTrayTransform.GetComponent<TrayData>().slotsInTray);
        if (GameManager.Instance.matchingTrayTransform.GetComponent<TrayData>() != null && 
            GameManager.Instance.matchingTrayTransform.GetComponent<TrayData>().slotsInTray == GameManager.Instance.matchingTrayTransform.GetComponent<TrayData>().slotsmatched) // only delete if matched tray slots is full
        {
            var trayData = GameManager.Instance.matchingTrayTransform.GetComponent<TrayData>();
            foreach (var card in trayData.matchedCards) 
            {
                Destroy(card.gameObject); // delete all the cards in the filled up matched tray
            }
            trayData.matchedCards.Clear();

            Transform trayTransform = GameManager.Instance.matchingTrayTransform.parent;

            if (trayTransform.childCount > 1) // check if child count is more than 1 then only we need to move the upper once to down 
                trayTransform.GetChild(1).GetComponent<Transform>().position = GameManager.Instance.matchingTrayTransform.position;


            Destroy(trayTransform.GetChild(0).gameObject);//destroy the fully matched tray
           
        }

        GameManager.Instance.matchingTrayTransform = null;
        GameManager.Instance.cardWasClicked = false;
        GameManager.Instance.matchingTrayFound = false;
    }
}
