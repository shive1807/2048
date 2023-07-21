using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VFX : MonoBehaviour
{
    [SerializeField] ParticleSystem[] CelebartionFX;
    [SerializeField] ParticleSystem breakFX;
    public void PlayCelebrationVfx()
    {
        foreach(ParticleSystem ps in CelebartionFX)
        {
            if (ps != null && !ps.isPlaying)
            {
                ps.Play();
            }
        }
    }
    public void PlayBreakingFX(Element e)
    {
        var mainModule = breakFX.main;

        mainModule.startColor = new ParticleSystem.MinMaxGradient(e.image.color);

    }
}
