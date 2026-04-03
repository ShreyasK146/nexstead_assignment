using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool cardWasClicked = false;
    public string selectedCardColor = "";
    public string tray1sColor = "";
    public string tray2sColor = "";
    private void Awake()
    {
        Instance = this;
    }


}
