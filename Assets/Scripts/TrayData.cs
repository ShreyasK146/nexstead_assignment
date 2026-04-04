using System.Collections.Generic;
using UnityEngine;

public class TrayData : MonoBehaviour
{
    public string TrayColor = "";
    public int slotsInTray = 6; // hardcoded
    public int slotsmatched = 0;
    public List<GameObject> matchedCards = new List<GameObject>();// added to just to have additional features working if needed 
}
