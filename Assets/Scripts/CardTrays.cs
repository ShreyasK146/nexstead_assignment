using System;
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
    }
    
    private void SpawnTrays()
    {
        for(int i = 0; i < 2; i++)
        {
            if(i == 0)
            {
                tray = Instantiate(tray1Prefab, tray1Transform);
                tray.transform.localPosition = new Vector3(-0.75f, 2.2f + i * 1.6f, 0.9f + i * 0.35f);
                tray.transform.localRotation = Quaternion.Euler(90f, 90f, -90f);
                tray.transform.localScale = new Vector3(0.085f, 0.085f, 0.2f);
                GameManager.Instance.tray1sColor = tray.GetComponent<TrayData>().TrayColor;
            }

            else if (i == 1)
            {
                tray = Instantiate(tray2Prefab, tray1Transform);
                tray.transform.localPosition = new Vector3(-0.75f, 2.2f + i * 1.6f, 0.9f + i * 0.35f);
                tray.transform.localRotation = Quaternion.Euler(90f, 90f, -90f);
                tray.transform.localScale = new Vector3(0.085f, 0.085f, 0.2f);
            }
        }
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                tray = Instantiate(tray2Prefab, tray2Transform);
                tray.transform.localPosition = new Vector3(0.75f, 2.2f + i * 1.6f, 0.9f + i * 0.35f);
                tray.transform.localRotation = Quaternion.Euler(90f, 90f, -90f);
                tray.transform.localScale = new Vector3(0.085f, 0.085f, 0.2f);
                GameManager.Instance.tray2sColor = tray.GetComponent<TrayData>().TrayColor;
            }

            else if (i == 1)
            {
                tray = Instantiate(tray1Prefab, tray2Transform);
                tray.transform.localPosition = new Vector3(0.75f, 2.2f + i * 1.6f, 0.9f + i * 0.35f);
                tray.transform.localRotation = Quaternion.Euler(90f, 90f, -90f);
                tray.transform.localScale = new Vector3(0.085f, 0.085f, 0.2f);
            }
        }
    }
}

//class Trays
//{
//    public int Slot { get; set; }
//    public string SlotColor { get; set; }
//}