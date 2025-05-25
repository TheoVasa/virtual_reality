using UnityEngine;
using System.Collections.Generic; // Required for using List
using TMPro; // Required for TMP_InputField

public class Project_Manager_Controller : MonoBehaviour
{
    [Header("Common Right Panel Objects")]
    public GameObject enterPasswordObject;
    public TMP_InputField inputFieldObject;
    // public GameObject touchpadFlatObject; // Old touchpad reference
    public GameObject notesPannelObject;

    [Header("UI Control")] // New Header for the main UI parent
    public GameObject mainUIParentObject; // Assign your main UI parent object here

    [Header("Object to Spawn")] // New Header for the object to spawn
    public GameObject objectToSpawn; // Assign the object to be spawned here
    public GameObject objectToSpawnAfterUI; // Assign the object to be spawned after UI disappears

    [Header("Touchpads")] // New Header for clarity
    public GameObject mainTouchpadObject; // Renamed from touchpadFlatObject
    public GameObject notesTouchpadObject; // New touchpad for notes

    [Header("Title Objects")]
    public GameObject iccTitleObject;
    public GameObject ciaTitleObject;
    public GameObject conquerTitleObject;
    public GameObject vrTitleObject;
    public GameObject publicationsTitleObject;

    [Header("Notes Panel Specific")] // Added Header for clarity
    public TMP_InputField notesInputField; // Added: Input field within the notes panel
    public AudioSource successAudioSource; // Added: AudioSource for success sound

    private List<GameObject> allTitleObjects;
    private GameObject currentActiveSectionTitle;
    private bool vrSectionUnlocked = false;
    // private TMP_InputField currentTouchpadTargetField; // Removed: No longer needed with separate handlers

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the list of all title objects
        allTitleObjects = new List<GameObject>
        {
            iccTitleObject,
            ciaTitleObject,
            conquerTitleObject,
            vrTitleObject,
            publicationsTitleObject
        };

        // Initialize new state variables
        currentActiveSectionTitle = null;
        vrSectionUnlocked = false;

        // Ensure all target objects are inactive at start
        DeactivateAllTargetObjects();
        
        if (mainTouchpadObject != null) mainTouchpadObject.SetActive(false); // Updated reference
        if (notesTouchpadObject != null) notesTouchpadObject.SetActive(false); // Added for new touchpad
        if (notesPannelObject != null) notesPannelObject.SetActive(false);
    }

    private void DeactivateAllTargetObjects()
    {
        if (enterPasswordObject != null)
            enterPasswordObject.SetActive(false);
        if (inputFieldObject != null && inputFieldObject.gameObject != null)
        {
            inputFieldObject.gameObject.SetActive(false);
            inputFieldObject.text = ""; // Clear password field text
        }
        
        if (mainTouchpadObject != null) // Updated reference
            mainTouchpadObject.SetActive(false);
        if (notesTouchpadObject != null) // Added for new touchpad
            notesTouchpadObject.SetActive(false);
        if (notesPannelObject != null)
            notesPannelObject.SetActive(false);

        if (allTitleObjects != null)
        {
            foreach (GameObject titleObject in allTitleObjects)
            {
                if (titleObject != null)
                    titleObject.SetActive(false);
            }
        }
        currentActiveSectionTitle = null; // Reset active section
        // currentTouchpadTargetField = null; // Removed
    }

    private void ActivateCommonObjects()
    {
        if (enterPasswordObject != null)
            enterPasswordObject.SetActive(true);
        if (inputFieldObject != null && inputFieldObject.gameObject != null)
            inputFieldObject.gameObject.SetActive(true);
    }

    public void OnInputFieldSelected() // This is for the original password input field
    {
        Debug.Log("OnInputFieldSelected called for main password."); // DEBUG
        if (inputFieldObject != null && mainTouchpadObject != null) // Updated reference
        {
            // currentTouchpadTargetField = inputFieldObject; // Removed
            // Debug.Log("currentTouchpadTargetField set to: inputFieldObject"); // Removed
            mainTouchpadObject.SetActive(true); // Use main touchpad
        }
    }

    // New method to handle selection of the notes input field
    public void OnNotesInputFieldSelected()
    {
        Debug.Log("[PM_Controller] OnNotesInputFieldSelected called for notes.");
        if (notesInputField != null && notesTouchpadObject != null)
        {
            // Only show touchpad if the input field is currently interactable
            if (notesInputField.interactable)
            {
                if (!notesTouchpadObject.activeSelf) // Only activate if not already active
                {
                    notesTouchpadObject.SetActive(true); // Activate notes touchpad
                    Debug.Log("[PM_Controller] notesTouchpadObject activated by OnNotesInputFieldSelected because field is interactable.");
                }
            }
            else
            {
                Debug.Log("[PM_Controller] OnNotesInputFieldSelected: notesInputField is NOT interactable, touchpad not shown/kept hidden.");
            }
        }
    }

    // public void OnTouchpadDigitPressed(string digit) // REMOVED - Replaced by specific handlers below
    // {
    //    // ... Old combined logic removed ...
    // }

    // New method for the main password touchpad
    public void OnMainPasswordTouchpadDigitPressed(string digit)
    {
        Debug.Log($"OnMainPasswordTouchpadDigitPressed called with digit: {digit}");
        if (inputFieldObject != null)
        {
            inputFieldObject.ActivateInputField();
            inputFieldObject.text += digit;
            CheckPassword(); // CheckPassword uses inputFieldObject.text
        }
        // Main password touchpad usually stays active until password entry is complete or cancelled.
    }

    // New method for the notes input field touchpad
    public void OnNotesTouchpadDigitPressed(string digit)
    {
        Debug.Log($"[PM_Controller] OnNotesTouchpadDigitPressed: Received digit \'{digit}\'.");

        if (digit == "6")
        {
            if (notesInputField != null)
            {
                if (notesInputField.interactable)
                {
                    notesInputField.ActivateInputField();
                    notesInputField.text = "6/6"; 
                    Debug.Log("[PM_Controller] notesInputField text set to \'6/6\'.");

                    if (notesTouchpadObject != null)
                    {
                        notesTouchpadObject.SetActive(false); 
                        Debug.Log("[PM_Controller] notesTouchpadObject set to inactive.");
                    }

                    notesInputField.interactable = false; 
                    Debug.Log("[PM_Controller] notesInputField set to non-interactable.");

                    // Play success sound immediately
                    if (successAudioSource != null)
                    {
                        successAudioSource.Play();
                        Debug.Log("[PM_Controller] Success audio played immediately on digit 6 press.");
                    }
                    else
                    {
                        Debug.LogWarning("[PM_Controller] successAudioSource is not assigned. Cannot play audio.");
                    }

                    // Start the final sequence coroutine
                    StartCoroutine(FinalSequenceCoroutine());
                }
                else
                {
                    Debug.Log("[PM_Controller] notesInputField is already non-interactable. '6' press ignored for this action.");
                }
            }
            else
            {
                Debug.LogWarning("[PM_Controller] notesInputField is NULL. Cannot process digit '6'.");
            }
        }
        else
        {
            // If the digit is not "6", do nothing to the notesInputField or notesTouchpadObject.
            Debug.Log($"[PM_Controller] Digit '{digit}' is not '6', ignoring for special notes input field behavior.");
        }
    }

    private System.Collections.IEnumerator FinalSequenceCoroutine()
    {
        Debug.Log("[PM_Controller] FinalSequenceCoroutine started. Waiting for 3 seconds...");
        yield return new WaitForSeconds(3f);

        Debug.Log("[PM_Controller] 3 seconds elapsed. Deactivating UI parent and spawning object.");

        // Play success sound -- MOVED to OnNotesTouchpadDigitPressed
        // if (successAudioSource != null)
        // {
        // successAudioSource.Play();
        // Debug.Log("[PM_Controller] Success audio played.");
        // }
        // else
        // {
        // Debug.LogWarning("[PM_Controller] successAudioSource is not assigned. Cannot play audio.");
        // }

        if (mainUIParentObject != null)
        {
            mainUIParentObject.SetActive(false);
            Debug.Log("[PM_Controller] mainUIParentObject deactivated.");
        }
        else
        {
            Debug.LogWarning("[PM_Controller] mainUIParentObject is not assigned. Cannot deactivate.");
        }

        if (objectToSpawn != null)
        {
            objectToSpawn.SetActive(true);
            Debug.Log($"[PM_Controller] '{objectToSpawn.name}' activated.");
        }
        else
        {
            Debug.LogWarning("[PM_Controller] objectToSpawn is not assigned. Cannot spawn.");
        }

        // Spawn the additional object after UI disappears
        if (objectToSpawnAfterUI != null)
        {
            objectToSpawnAfterUI.SetActive(true);
            Debug.Log($"[PM_Controller] '{objectToSpawnAfterUI.name}' activated after UI disappeared.");
        }
        else
        {
            Debug.LogWarning("[PM_Controller] objectToSpawnAfterUI is not assigned. Cannot spawn additional object.");
        }
    }

    private void CheckPassword()
    {
        if (inputFieldObject == null) return;

        string currentPassword = inputFieldObject.text;

        if (currentPassword == "444")
        {
            if (currentActiveSectionTitle == vrTitleObject) // Check if VR section is active
            {
                HandleCorrectVRPassword();
            }
            else
            {
                // Correct password, but not for VR section. Clear input.
                inputFieldObject.text = "";
            }
        }
        else if (currentPassword.Length >= 3)
        {
            // Incorrect password or too long
            inputFieldObject.text = "";
        }
    }

    private void HandleCorrectVRPassword()
    {
        vrSectionUnlocked = true; // Mark VR as unlocked

        // Deactivate password entry UI
        if (enterPasswordObject != null) enterPasswordObject.SetActive(false);
        if (inputFieldObject != null && inputFieldObject.gameObject != null)
        {
            inputFieldObject.gameObject.SetActive(false); 
            inputFieldObject.text = "";                  
        }
        if (mainTouchpadObject != null) mainTouchpadObject.SetActive(false); // Updated reference
        if (notesTouchpadObject != null) notesTouchpadObject.SetActive(false); // Ensure notes touchpad is also off

        // Activate notes panel
        if (notesPannelObject != null) notesPannelObject.SetActive(true);
        // Ensure notes input field is interactable when notes panel is shown -- REMOVED FOR NEW LOGIC
        // if (notesInputField != null) 
        // {
        // notesInputField.interactable = true;
        // Debug.Log("notesInputField set to interactable in HandleCorrectVRPassword.");
        // }
        
        // Deactivate VR title as notes panel is now shown
        if (vrTitleObject != null) vrTitleObject.SetActive(false);
    }

    // Old HandleCorrectPassword is no longer directly used by CheckPassword in this new logic.
    // It can be removed or kept if used elsewhere, but for this specific flow,
    // HandleCorrectVRPassword is used. For this change, we assume it's replaced.
    /*
    private void HandleCorrectPassword()
    {
        DeactivateAllTargetObjects();
        if (notesPannelObject != null)
            notesPannelObject.SetActive(true);
    }
    */

    // Helper method to set the active section
    private void SetActiveSection(GameObject titleToActivate, bool showPasswordEntry)
    {
        DeactivateAllTargetObjects(); // Deactivates all titles, password UI, notes panel

        if (titleToActivate != null)
        {
            titleToActivate.SetActive(true);
            currentActiveSectionTitle = titleToActivate;
        }
        // else currentActiveSectionTitle remains null due to DeactivateAllTargetObjects()

        if (showPasswordEntry)
        {
            ActivateCommonObjects(); // Activates password input field and related UI
        }
    }

    // Public methods to be called by button OnClick events

    public void OnICCButtonClicked()
    {
        SetActiveSection(iccTitleObject, true);
    }

    public void OnCIAPartnershipButtonClicked()
    {
        SetActiveSection(ciaTitleObject, true);
    }

    public void OnPlanToConquerButtonClicked()
    {
        SetActiveSection(conquerTitleObject, true);
    }

    public void OnVRButtonClicked()
    {
        if (vrSectionUnlocked)
        {
            // VR section is already unlocked, show notes panel directly
            DeactivateAllTargetObjects(); // Clear everything first
            if (notesPannelObject != null) notesPannelObject.SetActive(true); // Show notes
            // Ensure notes input field is interactable when notes panel is shown -- REMOVED FOR NEW LOGIC
            // if (notesInputField != null) 
            // {
            // notesInputField.interactable = true;
            // Debug.Log("notesInputField set to interactable in OnVRButtonClicked (unlocked).");
            // }
            if (vrTitleObject != null) vrTitleObject.SetActive(false); // Ensure VR title is hidden
            currentActiveSectionTitle = vrTitleObject; // Set context to VR (even if title hidden)
        }
        else
        {
            // VR section not unlocked, show password entry
            SetActiveSection(vrTitleObject, true);
        }
    }

    public void OnPublicationsButtonClicked()
    {
        SetActiveSection(publicationsTitleObject, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
