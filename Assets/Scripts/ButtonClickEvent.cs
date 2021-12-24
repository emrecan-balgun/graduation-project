using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonClickEvent : MonoBehaviour
{
    
    public void OnTapPlayButton()
    {
        SceneManager.LoadScene("TreasureHuntMainScene");
    }
    public void OnTapScoreBarButton(GameObject panel )
    {
        if (panel.gameObject.activeSelf)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
        }
    }
}
