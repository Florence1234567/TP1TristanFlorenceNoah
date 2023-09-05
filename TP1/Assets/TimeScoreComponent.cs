using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeScoreComponent : MonoBehaviour
{
    //https://www.youtube.com/watch?v=27uKJvOpdYw

    [SerializeField] TextMeshProUGUI min10;
    [SerializeField] TextMeshProUGUI min1;
    [SerializeField] TextMeshProUGUI Sec10;
    [SerializeField] TextMeshProUGUI Sec1;
    float time = 0f;
    string currentTime = "";
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
        UpdateTimer();
    }

    void UpdateTimer()
    {
        time += Time.deltaTime;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        currentTime = string.Format("{00:00}{1:00}", minutes, seconds);
        min10.text = currentTime[0].ToString();
        min1.text = currentTime[1].ToString();
        Sec10.text = currentTime[2].ToString();
        Sec1.text = currentTime[3].ToString();
    }

    public void EndGame()
    {
        PlayerPrefs.SetString("time", currentTime);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game Over");
    }
}
