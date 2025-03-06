using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    [Header("Prefabs a spawnear (square, square(1), square(2), circle, etc.)")]
    public GameObject[] objectPrefabs;

    [Header("Cantidad de objetos a spawnear")]
    public int numberToSpawn = 20;

    [Header("Área de spawn (mínimo y máximo)")]
    public Vector2 spawnAreaMin = new Vector2(-10, -10);
    public Vector2 spawnAreaMax = new Vector2(10, 10);

    [Header("Distancia mínima entre objetos")]
    public float minDistance = 1.0f;

    // Lista para almacenar posiciones ya usadas
    private List<Vector3> usedPositions = new List<Vector3>();

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        if (objectPrefabs == null || objectPrefabs.Length == 0)
        {
            Debug.LogWarning("No se han asignado prefabs para spawnear.");
            return;
        }

        int spawned = 0;
        int maxGlobalAttempts = numberToSpawn * 10; // límite global para evitar bucles infinitos
        int globalAttempts = 0;

        while (spawned < numberToSpawn && globalAttempts < maxGlobalAttempts)
        {
            globalAttempts++;

            // Genera una posición aleatoria
            Vector3 spawnPos = GetRandomPosition();

            // Verifica que la posición esté lo suficientemente alejada de las ya usadas
            bool valid = true;
            foreach (Vector3 pos in usedPositions)
            {
                if (Vector3.Distance(spawnPos, pos) < minDistance)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                // Seleccionar aleatoriamente uno de los prefabs
                int index = Random.Range(0, objectPrefabs.Length);
                GameObject prefab = objectPrefabs[index];

                // Instanciar el objeto en la posición generada
                Instantiate(prefab, spawnPos, Quaternion.identity);

                usedPositions.Add(spawnPos);
                spawned++;
            }
        }

        if (globalAttempts >= maxGlobalAttempts)
        {
            Debug.LogWarning("Se alcanzó el límite de intentos para posicionar objetos sin superposición.");
        }
    }

    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector3(randomX, randomY, 0);
    }
}
