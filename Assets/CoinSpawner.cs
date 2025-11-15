using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab; // Yerleştirilmek istenen sikke prefabı
    public GameObject targetPrefab; // Sikkelerin etrafına yerleştirileceği prefab
    public int numberOfCoins = 10; // Yerleştirilecek sikke sayısı
    public float spawnRadius = 5f; // Sikkelerin yerleştirileceği yarıçap

    void Start()
    {
        SpawnCoinsAroundTarget();
    }

    void SpawnCoinsAroundTarget()
    {
        if (coinPrefab == null || targetPrefab == null)
        {
            Debug.LogError("Coin prefab veya target prefab eksik!");
            return;
        }

        Vector3 targetPosition = targetPrefab.transform.position;

        for (int i = 0; i < numberOfCoins; i++)
        {
            // Rastgele bir açı ve mesafe belirle
            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(0f, spawnRadius);

            // X ve Z koordinatlarını hesapla (dairesel yerleştirme)
            float x = targetPosition.x + Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
            float z = targetPosition.z + Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

            // Terrain yüksekliğini hesapla
            float y = Terrain.activeTerrain != null 
                ? Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z)) 
                : targetPosition.y + 3; // Eğer terrain yoksa hedef yüksekliği kullanılır

            Vector3 spawnPosition = new Vector3(x, y+1, z);

            // Sikkeyi oluştur
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
