using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] freshMeats;
    [SerializeField] private GameObject[] rottenMeats;
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private float maxForce = 10f;
    [SerializeField] private float minForce = 5f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= spawnTime)
        {
            SpawnMeat();
            timer = 0f;
        }
    }

    void SpawnMeat()
    {
        bool isRotten = Random.value < 0.4f;
        GameObject prefab = isRotten
            ? rottenMeats[Random.Range(0, rottenMeats.Length)]
            : freshMeats[Random.Range(0, freshMeats.Length)];

        Vector3 spawnPos = new Vector3(Random.Range(-3f, 3f), -6f, 0f);
        GameObject meat = Instantiate(prefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = meat.GetComponent<Rigidbody2D>();
        float force = Random.Range(minForce,maxForce);
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
