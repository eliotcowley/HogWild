using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaleAnimation : MonoBehaviour
{
    [SerializeField]
    private Vector3 scaleTo = new Vector3(2f, 2f, 2f);

    [SerializeField]
    private float scaleTime = 1f;

    private Vector3 originalScale;

    private void Start()
    {
        this.originalScale = this.transform.localScale;
        //StartCoroutine(ScaleAnimation());
        LeanTween.scale(this.gameObject, this.scaleTo, this.scaleTime).setLoopPingPong().setEaseInOutCirc();
    }

    private IEnumerator ScaleAnimation()
    {
        LeanTween.scale(this.gameObject, this.scaleTo, this.scaleTime);
        yield return new WaitForSeconds(this.scaleTime);

        LeanTween.scale(this.gameObject, this.originalScale, this.scaleTime);
        yield return new WaitForSeconds(this.scaleTime);

        StartCoroutine(ScaleAnimation());
    }
}
