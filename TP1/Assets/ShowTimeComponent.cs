using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowTimeComponent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI min10;
    [SerializeField] TextMeshProUGUI min1;
    [SerializeField] TextMeshProUGUI Sec10;
    [SerializeField] TextMeshProUGUI Sec1;

    string currentTime;

    private void Start()
    {
        currentTime = PlayerPrefs.GetString("time");
        min10.text = currentTime[0].ToString();
        min1.text = currentTime[1].ToString();
        Sec10.text = currentTime[2].ToString();
        Sec1.text = currentTime[3].ToString();
    }
}
