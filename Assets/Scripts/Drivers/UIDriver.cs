/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided “AS IS” WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/

/*
    Author:  Keith Wilcox
    About:  
    Manages the player menu and UI canvases.
    Built on the Oculus Framework DebugUIBuilder script. 
*/

using Malimbe.XmlDocumentationAttribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains core functionality for the UI system
/// </summary>
public class UIDriver : MonoBehaviour
{
    public enum PlayerMenuCamBehavior
    {
        StickToCamera,
        DoNotStick
    }

    #region Player Menu Settings
    [field: Header("Player Menu Settings"), DocumentedByXml]
    [SerializeField]
    public GameObject playerMenu = null;
    [SerializeField]
    public Transform menuPosition = null;
    public PlayerMenuCamBehavior menuBehavior;
    #endregion

    #region Oculus UI
    [field: Header("Oculus UI Settings"), DocumentedByXml]
    [SerializeField]
    public GameObject headsetAlias;
    [SerializeField]
    private GameObject uiHelpersToInstantiate = null;
    [HideInInspector]
    public LaserPointer lp;
    LineRenderer lr;
    public LaserPointer.LaserBeamBehavior laserBeamBehavior;
    #endregion

    
    #region Tool and Script References
    [field: Header("Tool and Script References"), DocumentedByXml]
    [SerializeField]
    public GameObject straightPointer;
    [SerializeField]
    private List<GameObject> toEnable = null;
    [SerializeField]
    public List<GameObject> toDisable = null;
    #endregion

    #region Old UI Builder Stuff
    public delegate void OnClick();
    public delegate void OnToggleValueChange(Toggle t);
    public delegate void OnSlider(float f);
    public delegate bool ActiveUpdate();
    #endregion

    #region Debugging
    public Tilia.Interactions.Interactables.Interactables.Operation.Extraction.InteractableFacadeExtractor IFExtractorScript;
    #endregion

    #region Interactor
    public Tilia.Interactions.Interactables.Interactors.InteractorFacade leftInteractor;
    #endregion

    [HideInInspector]
    public static bool isCanvasShowing = false;
    [HideInInspector]
    public static bool isPlayerMenuOpen = false;
    [HideInInspector]
    public static bool wasPointerEnabled = false;
    public static bool wasLocoEnabled = false;
    public static UIDriver instance = null;

    /// <summary>
    /// Sets up player menu, instantiates UI Helper from Oculus Framework, and adds laserpointer to enable list.  Configures OVRRaycaster script with laserpointer scene reference. Diables laserpointer since the menu is initially hidden.
    /// </summary>
    public void Awake()
    {
        // instance needs to be assigned to 'this' instance before any scripts try to dereference it
        instance = this;
        Debug.Assert(straightPointer != null);
        Debug.Assert(playerMenu != null);
        Debug.Assert(uiHelpersToInstantiate != null);
        Debug.Assert(menuPosition != null);

        // Start with the modules menu closed
        playerMenu.SetActive(false);

        // Set the behavior of the menu
        if (menuBehavior == PlayerMenuCamBehavior.StickToCamera)
        {
            playerMenu.transform.parent = headsetAlias.transform;
            playerMenu.transform.position = menuPosition.position;
        }

        // LaserPointer is a script attached to UIHelpers Prefab.
        lp = FindObjectOfType<LaserPointer>();
        if (!lp)
        {
            Debug.LogError("UIDriver requires use of a LaserPointer and will not function without it. Add one to your scene, or assign the UIHelpers prefab to the DebugUIBuilder in the inspector.");
            return;
        }
        lp.laserBeamBehavior = laserBeamBehavior;

        // Adds the laserpointer to the list
        if (!toEnable.Contains(lp.gameObject))
        {
            toEnable.Add(lp.gameObject);
        }

        playerMenu.GetComponent<OVRRaycaster>().pointer = lp.gameObject;
        lp.gameObject.SetActive(false);
        Debug.Log("UIDRIVER Started");
    }

    public void toggleItems(bool enableUIItems)
    {
        // Enables the user interface items (To Enable List) and disables the locomotion items (To Disable List) for when the menu is shown or Canvas is shown.
        if (enableUIItems)
        {
            lp.enabled = true;
            for (int i = 0; i < toDisable.Count; i++)
            {
                toDisable[i].SetActive(false);
            }

            for (int i = 0; i < toEnable.Count; i++)
            {
                toEnable[i].SetActive(true);
            }
        }
        else
        {
            lp.enabled = false;
            for (int i = 0; i < toDisable.Count; i++)
            {
                toDisable[i].SetActive(true);
            }

            for (int i = 0; i < toEnable.Count; i++)
            {
                toEnable[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Shows the player menu at the configured menu position
    /// </summary>
    public void ShowPlayerMenu()
    {
        wasPointerEnabled = lp.enabled;
        wasLocoEnabled = toDisable[0].activeInHierarchy;
        playerMenu.transform.position = menuPosition.position;
        lp.enabled = true;
        playerMenu.SetActive(true);

        // Calculate and set rotation of Menu.  It should face the player
        Vector3 newEulerRot = headsetAlias.transform.rotation.eulerAngles;
        newEulerRot.x = 0.0f;
        newEulerRot.z = 0.0f;
        playerMenu.transform.eulerAngles = newEulerRot;
        toggleItems(true);
        isPlayerMenuOpen = true;
    }

    /// <summary>
    /// Shows a passed UI Template prefab
    /// </summary>
    /// <param name="menu"></param>
    public void ShowCanvas(GameObject menu, bool enablePointer)
    {
        if (enablePointer)
        {
            toggleItems(true);
        }
        menu.SetActive(true);
        isCanvasShowing = true;
    }

    /// <summary>
    /// Hide player menu
    /// </summary>
    public void HidePlayerMenu()
    {
        // only hide if it isn't already showing
        if (isPlayerMenuOpen)
        {
            playerMenu.SetActive(false);
            // if pointer was enabled, 
            if (wasPointerEnabled)
            {
                toggleItems(wasPointerEnabled);
            }
            else if (wasLocoEnabled)
            {
                toggleItems(false);
            }
            else
            {
                ToggleLaserPointer(false);
            }
            isPlayerMenuOpen = false;
        }
    }

    
    /// <summary>
    /// Hides a passed UI Template prefab
    /// </summary>
    public void HideCanvas(GameObject menu, bool enableLocoAndDistance)
    {
        menu.SetActive(false);
        if (enableLocoAndDistance)
        {
            toggleItems(false);
        }
        
    }

    /// <summary>
    /// Enables/Disables the laserpointer
    /// </summary>
    /// <param name="isOn"></param>
    public void ToggleLaserPointer(bool isOn)
    {
      lp.enabled = isOn;
      toEnable[0].SetActive(isOn);
    }

    public void toggleMenu() 
    {
        if (isPlayerMenuOpen)
        {
            HidePlayerMenu();
        }
        else
        {
            ShowPlayerMenu();
        }
    }

    public void PrintDebugMessage()
    {   
        Debug.Log("Object touched/untouched");
    }

    public void GetInteractableFacade(Tilia.Interactions.Interactables.Interactables.InteractableFacade interactableFacadeComp)
    {
        Debug.Log("Debug Function called for " + interactableFacadeComp.name);
    }

    public void ToggleDistanceGrab()
    {
        StartCoroutine(ToggleDistanceGrabRoutine());
    }

    IEnumerator ToggleDistanceGrabRoutine()
    {
        yield return new WaitUntil(isPlayerNotGrabbing);
        GameState.instance.distanceGrabberObj.SetActive(!GameState.instance.distanceGrabberObj.activeInHierarchy);
    }

    public void EnableDistanceGrab()
    {
        StartCoroutine(EnableDistanceGrabRoutine());
    }

    IEnumerator EnableDistanceGrabRoutine()
    {
        yield return new WaitUntil(isPlayerNotGrabbing);
        GameState.instance.distanceGrabberObj.SetActive(true);
    }

    bool isPlayerNotGrabbing()
    {
      return (leftInteractor.GrabbedObjects.Count == 0);
    }

    public void EnableTeleporting()
    {
        for (int i = 0; i < 4; i++)
        {
            if(!toDisable[i].activeInHierarchy)
            {
                toDisable[i].SetActive(true);
            }
        }
    }

    public void DisableTeleporting() 
    { 
        StartCoroutine(DisableTeleportingRoutine());
    }
    
    IEnumerator DisableTeleportingRoutine()
    {
        for (int i = 0; i < 4; i++)
        {
            if(toDisable[i].activeInHierarchy)
            {
                toDisable[i].SetActive(false);
            }
        }
        yield return null;
    }

    public void DisableAllTools(bool disableDistanceGrab = true)
    {
        // disable laserpointer
        lp.enabled = false;
        foreach (var tool in toDisable)
        {
            tool.SetActive(false);
        }
        if (disableDistanceGrab)
        {
            ToggleDistanceGrab();
        }
    }

    public void EnableLaserPointer()
    {
        lp.enabled = true;
        toEnable[0].SetActive(true);
    }

    public void EnableMod3Tools()
    {
        StartCoroutine(EnableMod3ToolsRoutine());
    }

    IEnumerator EnableMod3ToolsRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        if (!IsDistanceGrabberEnabled())
        {
            EnableDistanceGrab();
        }
        ToggleLaserPointer(false);
        EnableTeleporting();
    }

    public void EnableMod2Tools()
    {
        StartCoroutine(EnableMod2ToolsRoutine());
    } 
    IEnumerator EnableMod2ToolsRoutine()
    { 
        yield return new WaitForSeconds(0.5f);
        if (!IsDistanceGrabberEnabled())
        {
            EnableDistanceGrab();
        }
        ToggleLaserPointer(false);
        DisableTeleporting();
    }


    bool IsDistanceGrabberEnabled()
    {
        return GameState.instance.distanceGrabberObj.activeInHierarchy;
    }
}
