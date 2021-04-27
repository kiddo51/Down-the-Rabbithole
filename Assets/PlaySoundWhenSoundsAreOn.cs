using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundWhenSoundsAreOn : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlaySound() {
        if (Generate.soundsOn) {
            audioSource.Play();
        }
    }
}
