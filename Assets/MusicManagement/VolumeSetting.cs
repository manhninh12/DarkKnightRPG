using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        // Kiểm tra xem người chơi đã từng chỉnh âm lượng chưa
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume(); // Sử dụng giá trị mặc định của Slider
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;

        // Quy đổi giá trị Slider (0.0001 -> 1) sang dải Decibel của Audio Mixer (-80dB -> 0dB)
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);

        // Lưu mức âm lượng vào máy
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    private void LoadVolume()
    {
        // Lấy âm lượng đã lưu và gán lại vào Slider
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SetMusicVolume();
    }
}