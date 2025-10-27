using UnityEngine;
using System.Collections;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform goalkeeper;

    [Header("Spawn")]
    [SerializeField] private float spawnInterval = 1.5f;

    [Header("Alvo dentro do gol (mesmos limites do player)")]
    [SerializeField] private Vector2 targetRangeX = new Vector2(-3.5f, 3.5f);
    [SerializeField] private Vector2 targetRangeY = new Vector2(-2f, 2f);

    [Header("Animação da bola")]
    [SerializeField] private float growDuration = 1.5f;
    [SerializeField] private float startScale = 0.1f;
    [SerializeField] private float endScale = 1.5f;
    [SerializeField] private float moveFactor = 0.3f; // quanto ela “anda” em direção ao alvo

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnOne();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnOne()
    {
        var go = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        var ball = go.GetComponent<Ball>();
        // Passa TUDO para a bola controlar (uma única fonte da verdade)
        ball.Init(
            goalkeeper,
            targetRangeX,
            targetRangeY,
            growDuration,
            startScale,
            endScale,
            moveFactor
        );
    }
}