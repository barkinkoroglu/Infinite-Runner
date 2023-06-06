using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    void Start()
    {
        if(!PlayerPrefs.HasKey("musicVolume")) {
            PlayerPrefs.SetFloat("musicVolume",1);
        }

        else{
            Load();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeVolume() {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load(){
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save(){
        PlayerPrefs.SetFloat("musicVolume",volumeSlider.value);
    }

    public void BacktotheMenu()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
        
    }

    public void SetQuality (int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex+1);

    }

    public void SetFullscreen (bool isFullscreen){
        Screen.fullScreen = isFullscreen;

    }
}
