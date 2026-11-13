using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Botones : MonoBehaviour
{
    public void SalirJuego()
    {
        Application.Quit();
    }

    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }
}