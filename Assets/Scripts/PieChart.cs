using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PieChart : MonoBehaviour
{
    public GameObject[] pieChart;

    public void SetValues(List<int> values, List<string> labels)
    {
        int total = 0;
        foreach (int value in values)
        {
            total += value;
        }
        int i = 0;
        int cumsum = 0;
        foreach (int value in values)
        {
            float prevCumsum = cumsum;
            cumsum += value;
            pieChart[i].GetComponent<Image>().fillAmount = (float)cumsum / (float)total;
            float angle = (float)(cumsum + prevCumsum) / (float)total * Mathf.PI;
            float magnitude = pieChart[i].GetComponent<RectTransform>().rect.width / 2.0f * 0.7f;
            float x = Mathf.Sin(angle) * magnitude + pieChart[i].transform.position.x;
            float y = Mathf.Cos(angle) * magnitude + pieChart[i].transform.position.y;
            TextMeshProUGUI label = pieChart[i].transform.Find("Label").GetComponent<TextMeshProUGUI>();
            label.text = labels[i];
            label.transform.position = new Vector2(x, y);
            i++;
        }
    }
}
