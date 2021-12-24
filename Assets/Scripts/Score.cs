using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public GameObject[] Agents;

    public Text []scoreText;
    
    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (var agent in Agents)
        {
            scoreText[i].text = agent.GetComponent<TreasureHuntAgent>().m_TreasureHuntSettings.totalScore.ToString();
            i++;
        }
        
         
    }
}
