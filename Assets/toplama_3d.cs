using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.IO;

public class toplama_3d : MonoBehaviour
{
    private List<string> fileLines = new List<string>();
    public float displayDuration = 10f;
    private bool isDisplayingText = false;
    public TextMeshProUGUI Mesaj;
    public string csvFileName;
    public TextMeshProUGUI skor;
    public int sikke=0;


    private Dictionary<string, string> sceneFileMap = new Dictionary<string, string>
    {
        { "kirsehir", "ahi_evran.csv" },
        { "antalya_yivli", "antalya_yivli.csv" },
        { "antalya_alanya", "alanya.csv" },
        { "Ardahan", "ardahan.csv" },
        { "erzurum", "cifte_minareli.csv" },
        { "kayseri", "doner_kumbet.csv" },
        { "konya_halka_begus", "konya_halka_begus.csv" },
        { "konya_ince_minare", "konya_ince_minare.csv" }
    };

    IEnumerator Start()
    {
        Mesaj.text = "";
        skor.text = "Toplanan: "+ sikke;
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneFileMap.TryGetValue(sceneName, out csvFileName))
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, csvFileName);
            Debug.Log("Dosya yolu: " + filePath);

#if UNITY_ANDROID
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string csvContent = www.downloadHandler.text;
                Debug.Log("Dosya başarıyla okundu, içerik:");
                Debug.Log(csvContent);

                fileLines = new List<string>(csvContent.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                Debug.LogError("CSV dosyası okunamadı: " + www.error);
                fileLines = new List<string> { "Hata: Dosya okunamadı." };
            }
#else
            if (File.Exists(filePath))
            {
                fileLines = new List<string>(File.ReadAllLines(filePath));
                Debug.Log("Dosya başarıyla okundu (PC). Satır sayısı: " + fileLines.Count);
            }
            else
            {
                Debug.LogError("Dosya bulunamadı: " + filePath);
                fileLines = new List<string> { "Hata: Dosya yok." };
            }
#endif
        }
        else
        {
            Debug.LogError("Sahne adı tanınmadı: " + sceneName);
            fileLines = new List<string> { "Hata: Sahne adı tanımlı değil." };
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "toplanacak")
        {
            sikke++;
            skor.text = "Sikke: " + sikke;
            Destroy(collision.gameObject);

            if (fileLines != null && fileLines.Count > 0)
            {
                string randomLine = fileLines[Random.Range(0, fileLines.Count)];
                Debug.Log("Gösterilecek metin: " + randomLine);
                StartCoroutine(DisplayText(randomLine));
            }
            else
            {
                Debug.LogError("Metinler yüklenemediği için gösterilemiyor.");
            }
        }
    }

    private IEnumerator DisplayText(string message)
    {
        isDisplayingText = true;
        Mesaj.text = message;
        Mesaj.enabled = true;
        yield return new WaitForSeconds(displayDuration);
        Mesaj.enabled = false;
        isDisplayingText = false;
    }
}
