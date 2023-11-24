using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactions.ObjectFinding
{
    public class FindableObjectVisual : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<Animator> animators;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private Renderer overlayRenderer;
        
        [Header("Settings")]
        [SerializeField] private Material overlayMaterialCorrect;
        [SerializeField] private Material overlayMaterialIncorrect;
        [SerializeField] private float fadeDuration = 2f;
        [SerializeField] private float defaultAlpha = 1f;
        [SerializeField] private float scaleFactor = 2.36f;

        private void Start()
        {
            AdjustOverlayScaleAndShiftMesh();
            StartCoroutine(Fade(false));
            StartCoroutine(Fade(false));
        }

        public void DisplayHighlight(bool display)
        {
            overlayRenderer.enabled = display;
        }
        
        [ContextMenu("HighlightCorrect")]
        public void HighlightCorrect()
        {
            DisplayHighlight(true);
            overlayRenderer.material = overlayMaterialCorrect;
            StartCoroutine(Fade(true));

            animators.ForEach(anim => anim.Play("CorrectAnim"));
        }
        
        [ContextMenu("HighlightIncorrect")]
        public void HighlightIncorrect()
        {
            DisplayHighlight(true);
            overlayRenderer.material = overlayMaterialIncorrect;
            StartCoroutine(Fade(true));
            
            animators.ForEach(anim => anim.Play("IncorrectAnim"));
        }

        public void Reset()
        {
            DisplayHighlight(false);
            animators.ForEach(anim => anim.Play("Idle"));
        }
        
        [ContextMenu("FadeIn")]
        public void FadeIn()
        {
            StartCoroutine(Fade(true));
        }
        
        [ContextMenu("FadeOut")]
        public void FadeOut()
        {
            StartCoroutine(Fade(false));
        }

        private IEnumerator Fade(bool fadeIn)
        {
            float startAlpha = fadeIn ? 0f : defaultAlpha;
            float targetAlpha = fadeIn ? defaultAlpha : 0f;
            
            overlayRenderer.material.SetFloat("_Alpha", startAlpha);

            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                yield return null;
                elapsedTime += Time.deltaTime;

                float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                overlayRenderer.material.SetFloat("_Alpha", alpha);
            }

            overlayRenderer.material.SetFloat("_Alpha", targetAlpha);
        }

        [ContextMenu("adjust scale")]
        private void AdjustOverlayScaleAndShiftMesh()
        {
            float meshHeight = meshFilter.mesh.bounds.size.y;

            var transform1 = transform;
            var scale = Vector3.one * scaleFactor * meshHeight;
            transform1.localScale = scale;

            var newOverlayPos = meshHeight / 2f;
            transform1.localPosition = new Vector3(0, newOverlayPos, 0);
        }
    }
}
