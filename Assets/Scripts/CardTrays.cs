
using UnityEngine;

public class CardTrays : MonoBehaviour
{
    [SerializeField] private GameObject tray1Prefab;
    [SerializeField] private GameObject tray2Prefab;

    [SerializeField] private Transform tray1Transform;
    [SerializeField] private Transform tray2Transform;

    private GameObject tray;

    private void Start()
    {
        SpawnTrays();
        GameEvents.Instance.OnCardWasClicked += CardSelected;
    }



    private void OnDisable()
    {
        GameEvents.Instance.OnCardWasClicked -= CardSelected;
    }
    /*
     * Hard Coded Tray Spawning
     */
    private void SpawnTrays()
    {
        SpawnTray(tray1Prefab, tray1Transform, 0, -1);
        SpawnTray(tray2Prefab, tray1Transform, 1, -1);
        SpawnTray(tray2Prefab, tray2Transform, 0, +1);
        SpawnTray(tray1Prefab, tray2Transform, 1, +1);

    }

    private void SpawnTray(GameObject trayPrefab, Transform trayTransform, int index ,int x) // int x represents here -x or +x axis
    {
        tray = Instantiate(trayPrefab, trayTransform);
        tray.transform.localPosition = new Vector3(x * 0.75f, 2.2f + index * 1.6f, 0.9f + index * 0.35f);
        tray.transform.localRotation = Quaternion.Euler(90f, 90f, -90f);
        tray.transform.localScale = new Vector3(0.085f, 0.085f, 0.2f);
    }

    /*
     * Any time the card is selected make sure to update tray1 and tray2s color , 
     * update if matching tray is found and store its transform
     * if not found set variable back to false
     */
    private void CardSelected()
    {
        if (tray1Transform.childCount > 0)
            GameManager.Instance.tray1sColor = tray1Transform.GetChild(0).GetComponent<TrayData>().TrayColor;
        if(tray2Transform.childCount > 0) 
            GameManager.Instance.tray2sColor = tray2Transform.GetChild(0).GetComponent<TrayData>().TrayColor;

        if (tray1Transform.childCount > 0 && GameManager.Instance.selectedCardColor == GameManager.Instance.tray1sColor)
        {
            GameManager.Instance.matchingTrayFound = true;
            GameManager.Instance.matchingTrayTransform =  tray1Transform.GetChild(0);

        }
        else if (tray2Transform.childCount > 0 && GameManager.Instance.selectedCardColor == GameManager.Instance.tray2sColor)
        {
            GameManager.Instance.matchingTrayFound = true;
            GameManager.Instance.matchingTrayTransform = tray2Transform.GetChild(0);
        }
        else
        {
            GameManager.Instance.matchingTrayFound = false;
        }
    }
}
