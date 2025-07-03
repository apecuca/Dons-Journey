using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] private float maxInitialRotation;

    private void OnEnable()
    {
        transform.eulerAngles = new Vector3(
            0,
            0,
            Random.Range(-maxInitialRotation, maxInitialRotation));
    }
}
