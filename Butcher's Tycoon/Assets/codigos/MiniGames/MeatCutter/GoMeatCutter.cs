using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMeatCutter : MonoBehaviour
{
    public void GoMeat()
    {
        SceneManager.LoadScene("MeatCutter");
        Debug.Log("minigame");
    }
}
