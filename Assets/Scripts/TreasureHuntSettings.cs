using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;

public class TreasureHuntSettings : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] agents;
    [HideInInspector]
    public Arena[] listArena;

    public int totalScore;

    public Text scoreText;

    StatsRecorder m_Recorder;

    public void Awake()
    {
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
        m_Recorder = Academy.Instance.StatsRecorder;
    }

    void EnvironmentReset()
    {
        ClearObjects(GameObject.FindGameObjectsWithTag("gem"));
        ClearObjects(GameObject.FindGameObjectsWithTag("mutant"));

        agents = GameObject.FindGameObjectsWithTag("agent");
        listArena = FindObjectsOfType<Arena>();
        foreach (var fa in listArena)
        {
            fa.ResetArena(agents);
        }

        totalScore = 0;
    }

    void ClearObjects(GameObject[] objects)
    {
        foreach (var obj in objects)
        {
            Destroy(obj);
        }
    }

    /*
    public void Update()
    {
        scoreText.text = $"Score: {totalScore}";

        // Send stats via SideChannel so that they'll appear in TensorBoard.
        // These values get averaged every summary_frequency steps, so we don't
        // need to send every Update() call.
        if ((Time.frameCount % 100)== 0)
        {
            m_Recorder.Add("TotalScore", totalScore);
        }
    }
    */
}
