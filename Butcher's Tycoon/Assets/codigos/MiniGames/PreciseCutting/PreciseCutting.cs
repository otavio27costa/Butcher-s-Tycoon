using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PreciseCutMinigame : MonoBehaviour
{
    [Header("Referências de UI")]
    [SerializeField] private RectTransform meatArea;    // Image da carne
    [SerializeField] private RectTransform cutLine;     // Image fininha que será a linha do corte

    [Header("Textos de interface (TMP)")]
    [SerializeField] private TextMeshProUGUI totalWeightText;
    [SerializeField] private TextMeshProUGUI requestedWeightText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Dados do Pedido")]
    [SerializeField] private float totalWeightKg = 1.0f;
    [SerializeField] private float requestedWeightKg = 0.3f;

    [Header("Sistema de Corte")]
    [Tooltip("Erro máximo para ser considerado corte perfeito (0.03 = 3%)")]
    [SerializeField] private float perfectTolerance = 0.03f;

    [Tooltip("Erro máximo para ser considerado corte bom (0.10 = 10%)")]
    [SerializeField] private float goodTolerance = 0.10f;

    [Tooltip("Largura da linha de corte em pixels")]
    [SerializeField] private float cutLineWidth = 4f;

    [Tooltip("Tempo (segundos) que o jogador precisa esperar entre cortes")]
    [SerializeField] private float cooldownBetweenCuts = 0.5f;

    [Header("Timer do Minigame")]
    [Tooltip("Duração total do minigame em segundos")]
    [SerializeField] private float minigameDuration = 20f;

    [Header("Pontuação por tipo de corte")]
    public int scorePerfect = 100;
    public int scoreGood = 50;
    public int scoreBad = 10;

    [Header("Objetos de animação")]
    [Tooltip("GameObject que contém a animação da lootbox (Animator/Animation)")]
    [SerializeField] private GameObject lootboxAnimationObject;

    [Header("Eventos")]
    public UnityEvent onPerfectCut;
    public UnityEvent onGoodCut;
    public UnityEvent onBadCut;
    public UnityEvent onTimerEnd;

    [Header("Feedback na tela")]
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private Color perfectColor = Color.green;
    [SerializeField] private Color goodColor = Color.yellow;
    [SerializeField] private Color badColor = Color.red;

    private bool isDragging = false;
    private float currentCutNormalizedX = 0.5f;

    private float currentTime;
    private int currentScore = 0;
    private bool canCut = true;
    private bool isMinigameOver = false;

    public LootBoxUI lootBoxUI;

    private void Awake()
    {
        if (cutLine != null)
        {
            cutLine.gameObject.SetActive(false);
            cutLine.anchorMin = new Vector2(0.5f, 0.5f);
            cutLine.anchorMax = new Vector2(0.5f, 0.5f);
            cutLine.pivot = new Vector2(0.5f, 0.5f);
        }

        if (lootboxAnimationObject != null)
        {
            lootboxAnimationObject.SetActive(false); // começa escondida
        }

        currentTime = minigameDuration;
        UpdateWeightTexts();
        UpdateScoreUI();
        UpdateTimerUI();
        if (feedbackText != null)
            feedbackText.text = "";
    }

    private void Update()
    {
        HandleTimer();
        HandleInput();
    }

    // ----------------------
    // TIMER DO MINIGAME
    // ----------------------
    private void HandleTimer()
    {
        if (isMinigameOver) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            UpdateTimerUI();
            EndMinigame();
            return;
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = $"Tempo: {currentTime:0.0}s";
    }

    private void EndMinigame()
    {
        if (isMinigameOver) return;

        isMinigameOver = true;
        canCut = false;
        isDragging = false;
        EnableCutLine(false);

        onTimerEnd?.Invoke();
        PlayLootboxAnimation();
    }

    private void PlayLootboxAnimation()
    {
        if (lootboxAnimationObject != null)
        {
            lootboxAnimationObject.SetActive(true);
            Debug.Log("Iniciando animação da lootbox...");
        }
        else
        {
            Debug.LogWarning("LootboxAnimationObject não atribuído no Inspector.");
        }
    }

    // ----------------------
    // INPUT E CORTE
    // ----------------------
    private void HandleInput()
    {
        if (!canCut || isMinigameOver || currentTime <= 0f) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerInsideMeatArea(Input.mousePosition))
            {
                isDragging = true;
                UpdateCutPosition(Input.mousePosition);
                EnableCutLine(true);
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            UpdateCutPosition(Input.mousePosition);
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            EvaluateCut();
            EnableCutLine(false);
            StartCoroutine(CutCooldown());
        }
    }

    private System.Collections.IEnumerator CutCooldown()
    {
        canCut = false;
        yield return new WaitForSeconds(cooldownBetweenCuts);
        if (!isMinigameOver)
            canCut = true;
    }

    private bool IsPointerInsideMeatArea(Vector2 screenPos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(meatArea, screenPos, null);
    }

    private void UpdateCutPosition(Vector2 screenPos)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(meatArea, screenPos, null, out localPoint);

        Rect rect = meatArea.rect;

        float normalizedX = Mathf.InverseLerp(rect.xMin, rect.xMax, localPoint.x);
        currentCutNormalizedX = Mathf.Clamp01(normalizedX);

        UpdateCutLineVisual();
    }

    private void UpdateCutLineVisual()
    {
        if (cutLine == null) return;

        Rect rect = meatArea.rect;
        float xLocal = Mathf.Lerp(rect.xMin, rect.xMax, currentCutNormalizedX);

        cutLine.sizeDelta = new Vector2(cutLineWidth, rect.height);
        cutLine.anchoredPosition = new Vector2(xLocal, 0f);
    }

    private void EnableCutLine(bool enable)
    {
        if (cutLine != null)
            cutLine.gameObject.SetActive(enable);
    }

    // ----------------------
    // AVALIAÇÃO DO CORTE
    // ----------------------
    private void EvaluateCut()
    {
        float idealCutNormalizedX = Mathf.Clamp01(
            requestedWeightKg / Mathf.Max(totalWeightKg, 0.0001f)
        );

        float error = Mathf.Abs(currentCutNormalizedX - idealCutNormalizedX);

        if (error <= perfectTolerance)
        {
            currentScore += scorePerfect;

            // 🔹FEEDBACK NA TELA
            if (feedbackText != null)
            {
                feedbackText.text = "Corte perfeito!";
                feedbackText.color = perfectColor;
            }

            // Corte perfeito encerra o minigame
            currentTime = 0f;
            UpdateTimerUI();
            onPerfectCut?.Invoke();
            Debug.Log("Corte perfeito – MINIGAME ENCERRADO");

            EndMinigame();
        }
        else if (error <= goodTolerance)
        {
            currentScore += scoreGood;

            if (feedbackText != null)
            {
                feedbackText.text = "Bom corte!";
                feedbackText.color = goodColor;
            }

            onGoodCut?.Invoke();
        }
        else
        {
            currentScore += scoreBad;

            if (feedbackText != null)
            {
                feedbackText.text = "Corte ruim...";
                feedbackText.color = badColor;
            }

            onBadCut?.Invoke();
        }

        UpdateScoreUI();
    }

    // ----------------------
    // UI: SCORE E PESOS
    // ----------------------
    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {currentScore}";
    }

    private void UpdateWeightTexts()
    {
        if (totalWeightText != null)
            totalWeightText.text = $"Peso total: {totalWeightKg:0.###} kg";

        if (requestedWeightText != null)
            requestedWeightText.text = $"Pedido: {requestedWeightKg:0.###} kg";
    }

    // ----------------------
    // API PÚBLICA
    // ----------------------
    public void SetOrder(float requested, float total)
    {
        this.requestedWeightKg = requested;
        this.totalWeightKg = total;
        UpdateWeightTexts();
    }
}
