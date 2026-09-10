using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Quiver : MonoBehaviour
{
    [SerializeField] private Transform arrowTransform;
    [SerializeField] private GameObject arrowPrefab;

    [SerializeField] private float rotateSpeed;
    [SerializeField] private Vector3 offset = new(0, 0.3f, 0.2f);

    private XRGrabInteractable currentArrow;

    private void Start()
    {
        ArrowGrabbed();
    }

    private void LateUpdate()
    {
        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);

        Quaternion rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        transform.position = Camera.main.transform.position + (rotation * offset);
        transform.rotation = rotation;
    }

    private void ArrowGrabbed()
    {
        if (currentArrow)
        {
            currentArrow.transform.localScale = arrowPrefab.transform.localScale;
            currentArrow.selectEntered.RemoveListener(OnArrowSelected);
        }

        GameObject clone = Instantiate(arrowPrefab, arrowTransform.position, arrowTransform.rotation, transform);
        clone.transform.localScale = arrowTransform.localScale;
        currentArrow = clone.GetComponent<XRGrabInteractable>();
        currentArrow.selectEntered.AddListener(OnArrowSelected);
    }

    private void OnArrowSelected(SelectEnterEventArgs args)
    {
        ArrowGrabbed();
    }
}
