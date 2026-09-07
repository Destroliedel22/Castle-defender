using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ArrowPouch : MonoBehaviour
{
    public bool activate;

    [SerializeField] private Transform arrowTransform;
    [SerializeField] private GameObject arrowPrefab;

    private XRGrabInteractable currentArrow;

    private void Start()
    {
        ArrowGrabbed();
    }

    private void Update()
    {
        if (activate)
        {
            activate = false;
            ArrowGrabbed();
        }
    }

    private void ArrowGrabbed()
    {
        if(currentArrow)
            currentArrow.selectEntered.RemoveListener(OnArrowSelected);

        GameObject clone = Instantiate(arrowPrefab, arrowTransform.position, arrowTransform.rotation, transform);
        currentArrow = clone.GetComponent<XRGrabInteractable>();
        currentArrow.selectEntered.AddListener(OnArrowSelected);
    }

    private void OnArrowSelected(SelectEnterEventArgs args)
    {
        ArrowGrabbed();
    }
}
