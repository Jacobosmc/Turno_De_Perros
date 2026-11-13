using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{

    public GameObject mainCanvas;
    public GameObject tutorialCanvas;
    private bool tutorialActive = false;

    void Update()
    {
        if (tutorialActive && Input.GetKeyDown(KeyCode.Return))
        {
            CerrarTutorial();
        }
    }

    public void SalirJuego()
    {
        Application.Quit();
    }

    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void CargarTutorial()
    {
        mainCanvas.SetActive(false);
        tutorialCanvas.SetActive(true);
        tutorialActive = true;
    }

    public void CerrarTutorial()
    {
        mainCanvas.SetActive(true);
        tutorialCanvas.SetActive(false);
        tutorialActive = false;
    }
}