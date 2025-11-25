using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{

    public static SceneController instance;
    

    private List<GameObject> tycoonRoots = new List<GameObject>();

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

    public void OpenMinigame(string miniGameSceneName)
    {
        // SALVA todos os objetos raiz da cena do Tycoon
        var roots = SceneManager.GetActiveScene().GetRootGameObjects();
        tycoonRoots.Clear();
        tycoonRoots.AddRange(roots);

        // DESATIVA tudo da cena do Tycoon
        foreach (var obj in tycoonRoots)
            obj.SetActive(false);

        // Carrega o minigame por cima
        SceneManager.LoadScene(miniGameSceneName, LoadSceneMode.Additive);
    }

    public void CloseMinigame(string miniGameSceneName)
    {
        // Fecha o minigame
        SceneManager.UnloadSceneAsync(miniGameSceneName);

        // REATIVA a cena do Tycoon intacta
        foreach (var obj in tycoonRoots)
            obj.SetActive(true);

      
    }
}
