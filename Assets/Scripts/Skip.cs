using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Skip : MonoBehaviour
{
    [SerializeField] PlayableDirector playableDirector;

    void Update() {
        bool isInputFire = Input.GetButtonDown("Fire1");
        if (isInputFire)
        {
            float directorTime = (float)playableDirector.time;
            if (directorTime > 0f && directorTime < 6f)
            {
                playableDirector.time = 6f;
            }
            else if (directorTime > 6f && directorTime < 12f)
            {
                playableDirector.time = 12f;
            }
            else if (directorTime > 12f && directorTime < 18f)
            {
                playableDirector.time = 18f;
            }
            else if (directorTime > 18f && directorTime < 24f)
            {
                playableDirector.time = 24f;
            }
            else if (directorTime > 24f)
            {
                CootsGameManager.Instance.LoadScene("LudsApt");
            }

            playableDirector.Play();
        }
    }
}
