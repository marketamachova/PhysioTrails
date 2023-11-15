using System;
using UnityEngine;

namespace Interactions
{
    [RequireComponent(typeof(AudioSource))]
    public class VOPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip hitVo;
        [SerializeField] private AudioClip missVo;

        private AudioSource _audioSource;
        
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayHit()
        {
            _audioSource.PlayOneShot(hitVo);
        }
        
        public void PlayMiss()
        {
            _audioSource.PlayOneShot(missVo);
        }
    }
}
