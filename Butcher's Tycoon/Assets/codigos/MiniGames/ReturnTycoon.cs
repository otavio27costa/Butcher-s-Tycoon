using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnTycoon : MonoBehaviour
{
    public void ReturnToTycoon()
    {
        SceneManager.LoadScene("Tycoon");
    }
}
