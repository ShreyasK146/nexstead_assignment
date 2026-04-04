using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;

    /*
     * Managing oncardclick and oncardsinbelt changed events
     * 
     */

    public event Action OnCardWasClicked;

    public event Action<int> OnCardsInBeltChangedd;

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
}
