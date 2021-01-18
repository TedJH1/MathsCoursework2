using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenMainMenu() => SceneManager.LoadScene(0);

    public void OpenDiamondSquare() => SceneManager.LoadScene(1);

    public void OpenMidpointDisplacement() => SceneManager.LoadScene(2);
}
