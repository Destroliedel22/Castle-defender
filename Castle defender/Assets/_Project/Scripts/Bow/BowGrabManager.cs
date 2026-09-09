using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine;

public class BowGrabManager : MonoBehaviour, IXRSelectFilter
{
    public bool canProcess => true; // leave this as true, it just tells XRI the filter is active

    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        if (interactable.isSelected && !interactable.interactorsSelecting.Contains(interactor))
            return false;
        else return true;
    }
}