using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class DominoController : MonoBehaviour
{

    private bool fallen = false;
    private AudioSource dominoSound;

    void Start()
    {
        dominoSound = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision c)
    {
        if (!fallen && c.rigidbody != null && c.rigidbody.gameObject.CompareTag("domino"))
        {
            dominoSound.Play();
            // One a piece was hit by a domino, mark it as fallen, which will disable the noise, so that
            // if the pieces vibrate and collide again they don't make noise
            fallen = true;
        }
    }
}
