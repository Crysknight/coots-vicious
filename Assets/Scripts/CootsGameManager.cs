using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CootsGameManager : MonoBehaviour
{
    public static CootsGameManager Instance;

    [SerializeField] private GameObject loaderCanvas;
    [SerializeField] private Image progressBar;
    [SerializeField] List<Sound> musicSounds, sfxSounds;
    [SerializeField] AudioSource musicSource, sfxSource;

    public float soundVolume = 0.5f;

    void Awake() {
        if (Instance == null)
        {
            SetSoundVolume(0.5f);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSoundVolume(float value) {
        soundVolume = value;

        musicSource.volume = soundVolume;
        sfxSource.volume = soundVolume;
    }

    public async void LoadScene(string sceneName) {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loaderCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
            progressBar.fillAmount = scene.progress;
        } while (scene.progress < 0.9f);

        loaderCanvas.SetActive(false);
        scene.allowSceneActivation = true;
    }

    public void PlayMusic(string name) {
        Sound music = musicSounds.Find(sound => sound.name == name);

        if (music != null)
        {
            musicSource.clip = music.clip;
            musicSource.Play();
        }
    }

    public void PlaySfx(string name) {
        Sound sfx = sfxSounds.Find(sound => sound.name == name);

        if (sfx != null)
        {
            sfxSource.clip = sfx.clip;
            sfxSource.Play();
        }
    }
}
