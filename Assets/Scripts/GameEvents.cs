using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;

    /*
     * Managing oncardclick , oncardsinbelt and oncardcount changed events
     * 
     */

    public event Action OnCardWasClicked;

    public event Action<int> OnCardsInBeltChangedd;

    public event Action OnCardCountChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void CardWasClicked()
    {
        OnCardWasClicked?.Invoke();
    }

    public void CardsInBeltChanged(int count)
    {
        OnCardsInBeltChangedd?.Invoke(count);
    }

    public void CardCountChanged()
    {
        OnCardCountChanged?.Invoke();
    }


}
