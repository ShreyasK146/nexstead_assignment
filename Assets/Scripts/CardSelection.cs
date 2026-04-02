using NUnit.Framework.Internal;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
public class CardSelection : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private Transform deck;
    private List<GameObject> selectedCards = new List<GameObject>();

    private void Start()
    {
        Invoke("TestCall", 5);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hit))
            {
                if (hit.collider != null && hit.collider.gameObject.tag == "card")
                {
                    deck = hit.collider.transform.parent;

                    string topColor = deck.GetChild(0).GetComponent<CardData>().cardColor;
                    string clickedColor = hit.collider.GetComponent<CardData>().cardColor;

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
                                Debug.Log(card.gameObject.name);
                            }
                                
                        }
                    }
                }
                
            }
        }
    }

    private void TestCall()
    {
        for (int i = 0; i < selectedCards.Count; i++)
        {
            int index = i; 
            DOVirtual.DelayedCall(i * 0.15f, () => {
                AnimateCardToBelt(selectedCards[index]);
            });
        }
    }

    public void AnimateCardToBelt(GameObject card)
    {
        card.transform.SetParent(null);
        Sequence seq = DOTween.Sequence();

        seq.Append(card.transform.DOMove(new Vector3(-1, -1, -5), 0.3f)
            .SetEase(Ease.OutCubic));
        seq.Append(card.transform.DOMove(new Vector3(-5, 1.8f, -2), 0.4f)
            .SetEase(Ease.InOutCubic));
        seq.Append(card.transform.DORotate(new Vector3(0, 90, 0), 0.2f)
            .SetEase(Ease.OutCubic));
        seq.Append(card.transform.DOMove(new Vector3(-5, 1.8f, 0.35f), 0.25f)
            .SetEase(Ease.InCubic));
    }
}


//-1,-1,-5

//-5,1.8,-2

//-5,1.8,-2
//0,90,0 (rotation)

//-5,1.8,0.35