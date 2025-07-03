using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] private Transform followTarget;
    [SerializeField] private float followDampness = 20.0f;
    private bool canFollow = false;
    [SerializeField] private float targetXPos = 0.0f;

    [Header("Shake")]
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeStrength;

    private float camSizeIngame = 5.0f;
    [SerializeField] private float resizeDampness;

    private Vector3 shakeOffset = Vector3.zero;

    private Camera mainCam;

    private void Start()
    {
        mainCam = GetComponent<Camera>();
        SetCanFollow(false);
        targetXPos = transform.position.x;
    }

    private void LateUpdate()
    {
        if (followTarget == null || !canFollow) 
            return;

        // Seguir verticalmente
        Vector3 newPos = new Vector3(
            targetXPos,
            followTarget.position.y,
            -10.0f);

        transform.position = Vector3.Lerp(transform.position, newPos, followDampness * Time.deltaTime) + shakeOffset;
    }

    public void PrepareIngameCam()
    {
        SetCanFollow(true);
        StartCoroutine(ExpandAndPositionCamera());
    }

    private IEnumerator ExpandAndPositionCamera()
    {
        while (camSizeIngame - mainCam.orthographicSize >= 0.01f)
        {
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, camSizeIngame, resizeDampness * Time.deltaTime);
            targetXPos = Mathf.Lerp(targetXPos, 0.0f, resizeDampness * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        mainCam.orthographicSize = camSizeIngame;
        targetXPos = 0.0f;
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

    private void SetCanFollow(bool state)
    {
        canFollow = state;
    }
}
