using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] private Transform followTarget;
    [SerializeField] private float dampness = 20.0f;

    [Header("Shake")]
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeStrength;

    private Vector3 shakeOffset = Vector3.zero;

    private void Update()
    {
        if (followTarget == null) 
            return;

        Vector3 newPos = Vector3.Lerp(transform.position, followTarget.position, dampness * Time.deltaTime);
        newPos.x = 0;
        newPos.z = -10.0f;
        transform.position = newPos + shakeOffset;
    }

    public void ShakeScreen()
    {
        StartCoroutine(ScreenShaker());
    }

    private IEnumerator ScreenShaker()
    {
        float _shakeTimer = 0.0f;
        while (_shakeTimer < shakeDuration)
        {
            shakeOffset.x = Random.Range(-shakeStrength, shakeStrength);
            shakeOffset.y = Random.Range(-shakeStrength, shakeStrength);

            _shakeTimer += 1.0f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        shakeOffset = Vector3.zero;
    }
}
