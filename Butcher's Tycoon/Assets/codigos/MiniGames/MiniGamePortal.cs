using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGamePortal : MonoBehaviour
{
    private bool inside = false;
    public string miniGame;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inside)
        {
            // carregar cena additive
            SceneManager.LoadScene(miniGame, LoadSceneMode.Additive);

            // Pausar o tycoon (opcional)
            Time.timeScale = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inside = true;
        }
    }

    public void Leave()
    {
        // Remover apenas o minigame
        SceneManager.UnloadSceneAsync(miniGame);

        // Voltar o tempo à normalidade
        Time.timeScale = 1;
    }
}
