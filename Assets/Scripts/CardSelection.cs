
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.cardWasClicked)// card click should not work when already clicked card exists
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // if the hit is card and parent is not unmatched tray transform then we clicked cards in deck
                if (hit.collider != null && hit.collider.gameObject.tag == "card" && hit.collider.gameObject.transform.parent != GameManager.Instance.unmatchedTrayTransform)
                {

                    
                    deck = hit.collider.transform.parent;

                    string topColor = deck.GetChild(0).GetComponent<CardData>().cardColor;
                    string clickedColor = hit.collider.GetComponent<CardData>().cardColor;
                    GameManager.Instance.selectedCardColor = clickedColor;

                    if (clickedColor != topColor) return;
                    else
                    {
                        int count = 0;
                        GameManager.Instance.cardWasClicked = true;
                        selectedCards.Clear();
                        foreach (Transform card in deck)
                        {
                            if (card.GetComponent<CardData>().cardColor != clickedColor && count <= 6) // this count <=6 is just cheeky because hardcoding 6 card at a time which can go to belt
                            {
                                break;
                            }
                            else
                            {
                                if(++count <= 6)
                                {
                                    selectedCards.Add(card.gameObject);
                                }

                            }

                        }

                        GameEvents.Instance.CardWasClicked();// inform gameevents on card click
                    }
                }
                // if the hit is card and parent is  unmatched tray transform then we clicked cards in unmatched tray transform
                else if (hit.collider != null && hit.collider.gameObject.tag == "card" && hit.collider.gameObject.transform.parent == GameManager.Instance.unmatchedTrayTransform) 
                {
                    deck = hit.collider.transform.parent;
                    string topColor = GameManager.Instance.unmatchedTrayTransform.GetChild(1).GetComponent<CardData>().cardColor; // getchild(0) is a sprite of unusedmatch transform so we skip it always 
                    string clickedColor = hit.collider.GetComponent<CardData>().cardColor;
                    GameManager.Instance.selectedCardColor = clickedColor;
                    if (clickedColor != topColor) return;
                    else
                    {
                        int count = 0;
                        GameManager.Instance.cardWasClicked = true;
                        selectedCards.Clear();
                        foreach (Transform card in deck)
                        {
                            if (count == 0)
                            {
                                ++count;continue; // because 1st guy is just a sprite so we skip him
                            }
                            if (card.GetComponent<CardData>().cardColor != clickedColor && count <= 6)  // this count <=6 is just cheeky because hardcoding 6 card at a time which can go to belt
                            {
                                break;
                            }
                            else
                            {
                                if(++count <= 7)
                                {
                                    GameManager.Instance.unusedCardCount--;
                                    selectedCards.Add(card.gameObject);
                                }

                            }

                        }
                        GameEvents.Instance.CardWasClicked(); // inform gameevents on card click
                        
                    }
                }

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
                if (!selectedCards.Contains(card.gameObject))
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


