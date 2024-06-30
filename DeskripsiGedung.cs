using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Untuk menggunakan Text dan Button
using UnityEngine.SceneManagement; // Untuk menggunakan SceneManager

public class DeskripsiGedung : MonoBehaviour
{
    public Text textNamaGedung; // Referensi untuk UI Text yang menampilkan nama gedung
    public Text textDeskripsiGedung; // Referensi untuk UI Text yang menampilkan deskripsi gedung
    public GameObject panelDeskripsi; // Referensi untuk panel deskripsi yang akan diaktifkan atau dinonaktifkan
    public string[] keteranganObject; // Array untuk menyimpan deskripsi objek
    public GameObject[] objectAktif; // Array untuk menyimpan objek aktif
    public string[] sceneNames; // Array untuk menyimpan nama-nama scene

    private Texture2D texture; // Referensi untuk tekstur dari objek target
    private List<Vector2> keypoints; // Daftar untuk menyimpan titik-titik kunci yang terdeteksi

    // Fungsi Start dipanggil sebelum frame pertama di-update
    void Start()
    {
        // Validasi panjang array
        if (objectAktif.Length != sceneNames.Length)
        {
            Debug.LogError("Panjang array objectAktif dan sceneNames harus sama!");
            return;
        }

        keypoints = new List<Vector2>(); // Inisialisasi daftar titik kunci

        // Inisialisasi atau pengaturan awal bisa dilakukan di sini
    }

    // Fungsi Update dipanggil sekali per frame
    void Update()
    {
        // Logika yang perlu dicek setiap frame bisa dilakukan di sini
    }

    // Fungsi dipanggil saat target ditemukan
    public void OnTargetFound(GameObject target)
    {
        panelDeskripsi.SetActive(true); // Mengaktifkan panel deskripsi

        textNamaGedung.text = target.name; // Menetapkan nama objek target ke UI Text

        for (int i = 0; i < objectAktif.Length; i++) // Loop melalui semua objek aktif
        {
            if (target.name == objectAktif[i].name) // Memeriksa apakah nama target sama dengan nama objek aktif
            {
                textDeskripsiGedung.text = keteranganObject[i]; // Menetapkan deskripsi gedung ke UI Text
                break; // Keluar dari loop setelah menemukan kecocokan
            }
        }

        // Menampilkan fitur dengan metode deteksi sederhana
        DetectFeatures(target);
    }

    // Fungsi dipanggil saat target hilang
    public void OnTargetLost()
    {
        panelDeskripsi.SetActive(false); // Menonaktifkan panel deskripsi
    }

    // Fungsi untuk menampilkan detail dan mengalihkan ke scene yang sesuai
    public void Detail()
    {
        string targetName = textNamaGedung.text; // Mendapatkan nama target dari teks

        for (int i = 0; i < objectAktif.Length; i++) // Loop melalui semua objek aktif
        {
            if (targetName == objectAktif[i].name) // Memeriksa apakah nama target sama dengan nama objek aktif
            {
                if (i < sceneNames.Length) // Memastikan indeks berada dalam batas array sceneNames
                {
                    SceneManager.LoadScene(sceneNames[i]); // Mengalihkan ke scene yang sesuai
                }
                else
                {
                    Debug.LogError("Indeks " + i + " berada di luar batas array sceneNames!");
                }
                break; // Keluar dari loop setelah menemukan kecocokan
            }
        }
    }

    // Fungsi untuk mendeteksi fitur pada target
    private void DetectFeatures(GameObject target)
    {
        Renderer renderer = target.GetComponent<Renderer>();
        texture = renderer.material.mainTexture as Texture2D;
        if (texture == null)
        {
            Debug.LogError("No texture found on target!");
            return;
        }

        // Konversi tekstur ke grayscale
        Color32[] pixels = texture.GetPixels32();
        int width = texture.width;
        int height = texture.height;
        byte[] grayPixels = new byte[width * height];

        for (int i = 0; i < pixels.Length; i++)
        {
            grayPixels[i] = (byte)(0.299f * pixels[i].r + 0.587f * pixels[i].g + 0.114f * pixels[i].b);
        }

        // Terapkan deteksi tepi sederhana
        keypoints.Clear();
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                int idx = y * width + x;
                int gx = grayPixels[idx - 1] - grayPixels[idx + 1];
                int gy = grayPixels[idx - width] - grayPixels[idx + width];
                int gradient = Mathf.Abs(gx) + Mathf.Abs(gy);

                if (gradient > 128) // Threshold untuk deteksi tepi
                {
                    keypoints.Add(new Vector2(x, y));
                }
            }
        }

        Debug.Log("Number of keypoints detected: " + keypoints.Count);

        // Menampilkan hasil (untuk keperluan debugging)
        Texture2D outputTexture = new Texture2D(width, height);
        outputTexture.SetPixels32(pixels);

        foreach (Vector2 kp in keypoints)
        {
            outputTexture.SetPixel((int)kp.x, (int)kp.y, Color.red);
        }

        outputTexture.Apply();
        GameObject.Find("Canvas/ResultImage").GetComponent<Image>().sprite = Sprite.Create(outputTexture, new Rect(0, 0, outputTexture.width, outputTexture.height), new Vector2(0.5f, 0.5f));
    }
}
