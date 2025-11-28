using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalCutting : MonoBehaviour
{
    [Header("Nomes das cenas")]
    [Tooltip("Cena do minigame (ex: PreciseCutting)")]
    public string miniGameSceneName = "PreciseCutting";

    [Tooltip("Cena principal / Tycoon (ex: Tycoon)")]
    public string mainSceneName = "Tycoon";

    [Header("Interação no mapa (opcional)")]
    [Tooltip("Tecla usada para entrar no minigame quando estiver perto do portal")]
    public KeyCode interactKey = KeyCode.E;

    [Tooltip("Tag do Player para detecção no trigger")]
    public string playerTag = "Player";

    private bool _playerInside = false;

    // ---------- ENTRAR NO MINIGAME (usado na cena principal) ----------
    private void Update()
    {
        // Entrada por tecla + trigger (opcional)
        if (_playerInside && Input.GetKeyDown(interactKey))
        {
            EnterMinigame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            _playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            _playerInside = false;
        }
    }

    /// <summary>
    /// Carrega a cena do minigame.
    /// Pode ser chamado por tecla, trigger ou botão de UI.
    /// </summary>
    public void EnterMinigame()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(miniGameSceneName))
        {
            SceneManager.LoadScene(miniGameSceneName);
        }
        else
        {
            Debug.LogError("PortalCutting: miniGameSceneName não definido!");
        }
    }

    // ---------- SAIR DO MINIGAME (usado na cena do minigame/lootbox) ----------
    /// <summary>
    /// Volta para a cena principal.
    /// Este método deve ser chamado por um Animation Event no final da lootbox.
    /// </summary>
    public void LeaveMinigame()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(mainSceneName))
        {
            SceneManager.LoadScene(mainSceneName);
        }
        else
        {
            Debug.LogError("PortalCutting: mainSceneName não definido!");
        }
    }
}
