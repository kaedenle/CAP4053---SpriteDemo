using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class VolumeController : MonoBehaviour
{
    public static VolumeController instance;
    public float volume = 0.5f;
    public AudioSource audioSource;
    private string volumeKey = "Volume";
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private TMP_Text volumeTextUI = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey(volumeKey))
        {
            volume = PlayerPrefs.GetFloat(volumeKey);
            audioSource.volume = volume;
        }
    }
    public void VolumeSlider(float volume)
    {
        volumeTextUI.text = volume.ToString("0.0");
    }

    void Update()
    {
        audioSource.volume = volume;
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat(volumeKey, volume);
    }
}
