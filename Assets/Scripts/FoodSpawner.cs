using UnityEngine;
using System.Linq;

public class FoodSpawner : MonoBehaviour
{
    [System.Serializable]
    public class CategoryFood
    {
        public string category;
        public GameObject[] prefabs;
    }

    [SerializeField] private CategoryFood[] foodCategories;
    [SerializeField] private float spawnXMin = -4f;
    [SerializeField] private float spawnXMax = 4f;
    [SerializeField] private float spawnY = -4.5f;
    [Range(0f, 1f)] [SerializeField] private float incorrectFoodChance = 0.3f; // 30% de probabilidad

    private float nextSpawnTime;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        CalculateSpawnBounds();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && gameManager != null)
        {
            SpawnFood();
            nextSpawnTime = Time.time + gameManager.GetSpawnInterval();
        }
    }

    GameObject GetRandomFoodPrefab(bool shouldBeIncorrect)
    {
        if (shouldBeIncorrect)
        {
            // Obtener todas las categorías excepto la actual
            var incorrectCategories = foodCategories
                .Where(c => c.category != gameManager.GetTargetCategory())
                .ToArray();
            
            if (incorrectCategories.Length > 0)
            {
                var randomCategory = incorrectCategories[Random.Range(0, incorrectCategories.Length)];
                return randomCategory.prefabs[Random.Range(0, randomCategory.prefabs.Length)];
            }
        }
        
        // Si no debe ser incorrecto o no hay categorías incorrectas, devolver una de la categoría actual
        var currentCategory = foodCategories.FirstOrDefault(c => c.category == gameManager.GetTargetCategory());
        return currentCategory?.prefabs[Random.Range(0, currentCategory.prefabs.Length)];
    }

    void SpawnFood()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(spawnXMin, spawnXMax),
            spawnY,
            0
        );

        bool spawnIncorrect = Random.value < incorrectFoodChance;
        GameObject foodPrefab = GetRandomFoodPrefab(spawnIncorrect);
        
        if (foodPrefab != null)
        {
            GameObject food = Instantiate(foodPrefab, spawnPos, Quaternion.identity);
            
            // Debug para verificar categorías
            FoodType foodType = food.GetComponent<FoodType>();
            // Debug.Log($"Spawned food: {food.name}, Category: {foodType.category}, " +
            //          $"Correct: {foodType.category = gameManager.GetTargetCategory()}");
        }
    }

    void CalculateSpawnBounds()
    {
        float camSize = Camera.main.orthographicSize;
        float aspect = Camera.main.aspect;
        spawnXMin = -camSize * aspect + 0.5f;
        spawnXMax = camSize * aspect - 0.5f;
    }
}