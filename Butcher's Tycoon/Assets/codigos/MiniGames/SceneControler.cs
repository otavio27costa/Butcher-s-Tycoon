using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{

    public string miniGameName;
    public static SceneController instance;
    public bool isNear = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isNear)
        {
            EnterMinigame();
            LoadMinigameRoutine();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNear = true;
        }     
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void EnterMinigame()
    {
        StartCoroutine(LoadMinigameRoutine());
    }

    IEnumerator LoadMinigameRoutine()
    {
        // desativar Tycoon
        Scene tycoon = SceneManager.GetSceneByName("Tycoon");
        foreach (var obj in tycoon.GetRootGameObjects())
            obj.SetActive(false);

        // carregar minigame additive
        yield return SceneManager.LoadSceneAsync(miniGameName, LoadSceneMode.Additive);
    }

    public void ExitMinigame()
    {
        StartCoroutine(UnloadMinigameRoutine());
    }

    IEnumerator UnloadMinigameRoutine()
    {
        // descarrega minigame
        yield return SceneManager.UnloadSceneAsync(miniGameName);

        // reativa Tycoon
        Scene tycoon = SceneManager.GetSceneByName("Tycoon");
        foreach (var obj in tycoon.GetRootGameObjects())
            obj.SetActive(true);
    }
}
