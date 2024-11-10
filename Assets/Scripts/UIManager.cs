using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIManager>();

            return instance;
        }
    }

    public Text ammoText;
    // public Text scoreText;
    // public Text waveText;
    public GameObject gameoverUI;

    public void UpdateAmmoText(int magAmmo, int ammoRemain)
    {
        ammoText.text = magAmmo + "/" + ammoRemain;
    }
    public void SetActiveGameOverUI(bool active) { gameoverUI.SetActive(true); }
    public void GameRestart() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }

}