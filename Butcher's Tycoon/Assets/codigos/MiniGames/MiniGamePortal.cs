using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGamePortal : MonoBehaviour
{
    [SerializeField] string miniGameName;
    public bool isNear = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isNear)
        {
            SceneManager.LoadScene(miniGameName);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        isNear = true;
    }

}
