using NUnit.Framework.Internal;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using System;
using TMPro;
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
        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.cardWasClicked)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.gameObject.tag == "card")
                {
                    GameManager.Instance.cardWasClicked = true;
                    
                    deck = hit.collider.transform.parent;

                    string topColor = deck.GetChild(0).GetComponent<CardData>().cardColor;
                    string clickedColor = hit.collider.GetComponent<CardData>().cardColor;
                    GameManager.Instance.selectedCardColor = clickedColor;

                    if (clickedColor != topColor) return;
                    else
                    {
                        selectedCards.Clear();
                        foreach (Transform card in deck)
                        {
                            if (card.GetComponent<CardData>().cardColor != clickedColor)
                            {
                                break;
                            }
                            else
                            {
                                selectedCards.Add(card.gameObject);
                            }

                        }
                        GameEvents.Instance.CardWasClicked();
                    }
                }

            }
        }
    }
}


//-1,-1,-5

//-5,1.8,-2

//-5,1.8,-2
//0,90,0 (rotation)

//-5,1.8,0.35