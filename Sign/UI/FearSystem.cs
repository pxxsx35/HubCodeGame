using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FearSystem : MonoBehaviour
{
    public PlayerStatus player;
    private float fearStatus;
    PostProcessVolume volume;
    private ColorGrading colorGrading;
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGetSettings(out colorGrading);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (colorGrading == null) return;


        fearStatus = player.fear / player.maxFear;


        colorGrading.saturation.value = Mathf.Lerp(0, -80, fearStatus);

        if(player.isDream)
        {
            colorGrading.postExposure.value = -6f;
            colorGrading.contrast.value = 50f;

        }
        else
        {
            colorGrading.postExposure.value = 0f;
            colorGrading.contrast.value = 0f;

        }

    }
}
