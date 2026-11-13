using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static float points = 0f;
    public static int strikes = 0;
    public static bool isOver = false;

    public static int currentHardness = 1;
    public List<float> PointsPerLevel = new List<float> {0f,10f,25f,45f,70f};
    public List<int> MaxOrdersPerLevl = new List<int> {1,2,3,4,5};

    public TMP_Text pointsText;
    public List<GameObject> strikeImages = new List<GameObject>();

    public GameObject finalCanvas;
    public TMP_Text finalScore;

    void Awake()
    {
        points = 0f;
        strikes = 0;
        isOver = false;
        currentHardness = 1;
        finalCanvas.SetActive(false);
    }

    void Update()
    {
        if (isOver) return;

        UpdateHardness();
        UpdatePoints();
        UpdateStrikes();

        if (strikes >= 3)
        {
            GameOver();
        }
    }

    public void UpdateHardness()
    {
        int actualHardness = 1;
        for (int i = 0; i < PointsPerLevel.Count; i++)
        {
            if (points >= PointsPerLevel[i])
            {
                actualHardness = i + 1;
            }
        }

        currentHardness = Mathf.Clamp(actualHardness,1,5);
    }

    public static int GetMaxOrders()
    {
        int index = Mathf.Clamp(currentHardness - 1, 0, 4);
        
        return index < 4 ? (1 + index) : 5;
    }

    public void UpdatePoints()
    {
        if (pointsText != null)
        {
            pointsText.text = points.ToString("F0");
        }
    }

    public void UpdateStrikes()
    {
        for (int i = 0; i < 3; i++)
        {
            strikeImages[i].SetActive(i < strikes);
        }
    }

    public static void AddStrikes()
    {
        if (strikes < 3)
        {
            strikes++;
        }
    }

    public void GameOver()
    {
        isOver = true;
        finalCanvas.SetActive(true);
        finalScore.text = points.ToString("F0");
    }
}
