using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
public class Ball : MonoBehaviour
{
    [Header("Referências")]
    private Transform goalkeeper;

    [Header("Parâmetros de alvo")]
    private Vector2 targetRangeX;
    private Vector2 targetRangeY;

    [Header("Animação")]
    private float growDuration = 1.5f;
    private float startScale = 0.1f;
    private float endScale = 1.5f;
    private float moveFactor = 0.3f; // 0 = só cresce no centro, 1 = vai até o alvo

    [Header("Detecção")]
    [SerializeField] private float triggerWindow = 0.15f; // tempo pro Unity processar o trigger

    private CircleCollider2D circle;
    private bool resultDecided = false;

    // CHAME isso logo após instanciar
    public void Init(Transform goalkeeper,
                     Vector2 targetRangeX,
                     Vector2 targetRangeY,
                     float growDuration,
                     float startScale,
                     float endScale,
                     float moveFactor)
    {
        this.goalkeeper = goalkeeper;
        this.targetRangeX = targetRangeX;
        this.targetRangeY = targetRangeY;
        this.growDuration = growDuration;
        this.startScale = startScale;
        this.endScale = endScale;
        this.moveFactor = moveFactor;

        StartCoroutine(AnimateAndCheck());
    }

    private void Awake()
    {
        circle = GetComponent<CircleCollider2D>();
        circle.isTrigger = true;
        circle.enabled = false; // só ativa quando “chega”
    }

    private IEnumerator AnimateAndCheck()
    {
        // escolhe o ponto alvo dentro do gol
        Vector3 target = new Vector3(
            Random.Range(targetRangeX.x, targetRangeX.y),
            Random.Range(targetRangeY.x, targetRangeY.y),
            0f
        );

        transform.position = Vector3.zero;
        transform.localScale = Vector3.one * startScale;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / growDuration;
            float eased = Mathf.SmoothStep(0f, 1f, t);

            // move levemente em direção ao alvo
            transform.position = Vector3.Lerp(Vector3.zero, target * moveFactor, eased);

            // cresce simulando aproximação
            transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, eased);

            yield return null;
        }

        // "Chegou": ativa o trigger e dá tempo pro OnTriggerEnter2D acontecer
        circle.enabled = true;
        yield return new WaitForSeconds(triggerWindow);

        if (!resultDecided)
        {
            resultDecided = true;
            Debug.Log("⚽ GOL!");
        }

        Destroy(gameObject, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (resultDecided) return;

        if (other.CompareTag("Player"))
        {
            resultDecided = true;
            Debug.Log("🧤 DEFENDEU!");
            Destroy(gameObject, 0.1f);
        }
    }
}
