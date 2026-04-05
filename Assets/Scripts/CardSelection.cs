
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NUnit.Framework.Internal.Commands;

public class CardSelection : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private Transform deck;
    [HideInInspector] public List<GameObject> selectedCards = new List<GameObject>();



    private void Update()
    {
        HandleMouseClick();
    }

    private void HandleMouseClick()
    {

        if(Input.GetMouseButtonDown(0) && !GameManager.Instance.cardWasClicked)// card click should not work when already clicked card exists
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit) )
            {
                if (hit.collider == null || hit.collider.gameObject.tag != "card") return;

                bool isUnMatchedTray = (hit.collider.gameObject.transform.parent == GameManager.Instance.unmatchedTrayTransform);
                deck = hit.collider.transform.parent;

                // if it is unmatchedtray skip child 0 because its a sprite of tray not card
                string topColor = isUnMatchedTray? deck.GetChild(1).GetComponent<CardData>().cardColor:deck.GetChild(0).GetComponent<CardData>().cardColor;
                
                string clickedColor = hit.collider.GetComponent<CardData>().cardColor;
                GameManager.Instance.selectedCardColor = clickedColor;

                if (clickedColor != topColor) return;

                int count = 0;
                GameManager.Instance.cardWasClicked = true;
                selectedCards.Clear();

                foreach(Transform card in deck)
                {
                    if (isUnMatchedTray && count == 0)
                    {
                        count++;
                        continue; // same logic just skip 1st one
                    }

                    if(card.GetComponent<CardData>().cardColor != clickedColor) break;

                    int upperlimit = isUnMatchedTray ? 7 : 6; //hardcoding for 6

                    if (++count <= upperlimit)
                    {
                        if (isUnMatchedTray) GameManager.Instance.unusedCardCount--; // ok if unmatchedtray was clicked then card going from there means u reduce the card count in it
                        selectedCards.Add(card.gameObject);
                    }
                    else break;
                }
                GameEvents.Instance.CardWasClicked();
            }
        }
       
    }
    //move remaining card in deck to up visually 
    public void RefreshDeck()
    {
        List<Transform> remaining = new List<Transform>();
        if(deck != GameManager.Instance.unmatchedTrayTransform)
        {
            foreach (Transform card in deck)
            {
                if (!selectedCards.Contains(card.gameObject)) // select cards which are in selected cards list to move them to left
                {
                    remaining.Add(card);
                }
            }

            for (int i = 0; i < remaining.Count; i++)
            {
                remaining[i].DOLocalMove(new Vector3(0, 0.1f - i * 0.01f, 3.5f - i * 0.5f), 0.3f)
                    .SetEase(Ease.OutCubic);
            }
        }

       
    }
    //moving remaining cards in unmatched tray to begining (this case wont execute in our current level)
    public void RefreshUnmatchedTray()
    {
        List<Transform> remaining = new List<Transform>();
        int count = 0;
        foreach (Transform card in GameManager.Instance.unmatchedTrayTransform)
        {
            if (count++ == 0) continue; //skipping sprite guy
            if (!selectedCards.Contains(card.gameObject))
            {
                remaining.Add(card);
            }
        }

        for (int i = 0; i < remaining.Count; i++)
        {
            remaining[i].DOLocalMove(new Vector3(-3.2f + i * 0.34f, -0.75f, -0.1f), 0.3f)
                .SetEase(Ease.OutCubic);
        }

    }

}


