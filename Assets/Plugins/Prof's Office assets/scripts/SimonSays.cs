using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class SimonSaysController : MonoBehaviour
{
    [Header("Button References (Assign 1-9 in order)")]
    [Tooltip("Assign button GameObjects 1 through 9 in this list.")]
    public List<GameObject> buttons; // Assign in Inspector: [0]=Button1, [1]=Button2, ..., [8]=Button9

    [Header("Feedback Settings")]
    public Color successFlashColor = Color.green;
    public Color failureColor = Color.red;
    public float feedbackDuration = 0.75f; // How long success/failure colors show

    [Header("Hint Animation Settings")]
    public Color hintFlashColor = Color.white;
    public float hintBlinkDuration = 0.1f;
    public float pauseBetweenHintBlinksInSequence = 0.1f;
    public float pauseAfterFullSequenceRepeat = 1f;
    public float pauseBeforeNextHintAfterSuccess = 0.1f;

    [Header("Audio Settings")]
    [Tooltip("Audio clip that plays when player completes the final sequence")]
    public AudioSource completionAudioSource;
    [Tooltip("Delay in seconds before playing the completion audio")]
    [Range(0f, 5f)]
    public float completionAudioDelay = 0.4f;

    [Tooltip("Second audio clip that plays after the initial completion audio")]
    public AudioSource postCompletionAudioSource;
    [Tooltip("Delay in seconds, after the initial completion audio is played (or would have played), before this second audio clip plays.")]
    [Range(0f, 10f)]
    public float postCompletionAudioDelay = 1.0f;

    [Header("Door Settings")]
    [Tooltip("The transform that acts as the pivot point for the door")]
    public Transform doorAnchor;
    [Tooltip("The door GameObject that should rotate (if different from this object)")]
    public GameObject doorToRotate;
    [Tooltip("How far to rotate the door when opened (in degrees)")]
    [Range(0f, 180f)]
    public float doorOpenAngle = 90f;
    [Tooltip("How long it takes for the door to open")]
    [Range(0.1f, 5f)]
    public float doorOpenDuration = 1.5f;

    // --- Game State ---
    private int currentStage = 0; // 0: Start [5,5], 1: Next [5,5,7,9], 2: Final [5,5,1,3]
    private List<List<int>> targetSequences;
    private List<int> playerInputSequence = new List<int>();

    // For storing original colors to revert after feedback
    private Dictionary<Button, ColorBlock> originalButtonColorBlocks = new Dictionary<Button, ColorBlock>();
    private Coroutine feedbackCoroutine = null; // To manage flashing/failure display
    private Coroutine hintCoroutine = null;
    private Coroutine doorCoroutine = null;
    private bool doorOpened = false;

    void Start()
    {
        StoreOriginalColors();
        InitializeGame();
    }

    void InitializeGame()
    {
        // Stop any ongoing feedback or hint coroutines
        if (feedbackCoroutine != null)
        {
            StopCoroutine(feedbackCoroutine);
            feedbackCoroutine = null;
        }
        if (hintCoroutine != null)
        {
            StopCoroutine(hintCoroutine);
            hintCoroutine = null;
        }

        // Ensure all buttons are reset to their true original colors
        RevertAllButtonColors();

        targetSequences = new List<List<int>> {
            new List<int> { 5, 5 },          // Stage 0 target
            new List<int> { 5, 5, 7, 9 },    // Stage 1 target
            new List<int> { 5, 5, 1, 3 }     // Stage 2 target
        };

        currentStage = 0;
        playerInputSequence.Clear();
        Debug.Log("Simon Says Initialized. Waiting for sequence: " + string.Join(", ", targetSequences[currentStage]));

        // Start hint for Stage 0 (repeating) - add explicit debug log
        Debug.Log("Starting initial hint animation for stage 0");
        if (buttons.Count > 0 && targetSequences.Count > 0 && targetSequences[currentStage].Count > 0)
        {
            // Use Invoke to delay the start of the first hint slightly to ensure initialization is complete
            Invoke("StartFirstHint", 0.5f);
        }
        else
        {
            Debug.LogError("Cannot start hint - buttons list is empty or target sequences not defined!");
        }
    }

    void StartFirstHint()
    {
        if (hintCoroutine != null)
        {
            StopCoroutine(hintCoroutine);
        }
        
        Debug.Log("Launching first hint sequence animation now...");
        hintCoroutine = StartCoroutine(ShowSequenceHintRoutine(targetSequences[currentStage], true, hintBlinkDuration, pauseBetweenHintBlinksInSequence, pauseAfterFullSequenceRepeat));
        
        // Debug state of buttons to ensure colors are different
        for (int i = 0; i < buttons.Count; i++)
        {
            Button btn = buttons[i].GetComponent<Button>();
            if (btn != null)
            {
                Debug.Log($"Button {i+1} normal color: {btn.colors.normalColor}, hint color will be: {hintFlashColor}");
            }
        }
    }

    void StoreOriginalColors()
    {
        originalButtonColorBlocks.Clear();
        foreach (GameObject btnGO in buttons)
        {
            Button uiButton = btnGO.GetComponent<Button>();
            if (uiButton != null)
            {
                originalButtonColorBlocks[uiButton] = uiButton.colors;
            }
            else
            {
                Debug.LogWarning($"Button GameObject {btnGO.name} does not have a UI.Button component. Cannot store original color.");
            }
        }
    }

    // --- Public method called by Buttons ---
    // Ensure each button's interaction event calls this with its correct number (1-9)
    public void ButtonPressed(int buttonId)
    {
        // If a hint is playing, stop it. Player interaction takes precedence.
        if (hintCoroutine != null)
        {
            StopCoroutine(hintCoroutine);
            hintCoroutine = null;
            RevertAllButtonColors(); // Ensure buttons are reset from any partial hint state
        }

        if (currentStage >= targetSequences.Count)
        {
            Debug.Log("Game already completed!");
            return; // All stages done
        }

        // If feedback is currently showing, ignore input until it finishes
        if (feedbackCoroutine != null)
        {
            Debug.Log("Ignoring input during feedback.");
            return;
        }

        Debug.Log($"Button {buttonId} pressed. Current Stage: {currentStage}");
        playerInputSequence.Add(buttonId);

        // Get the target sequence for the current stage
        List<int> currentTarget = targetSequences[currentStage];

        // --- Check the input sequence ---
        bool sequenceCorrectSoFar = true;
        for (int i = 0; i < playerInputSequence.Count; i++)
        {
            // Check if input is longer than target or doesn't match at current position
            if (i >= currentTarget.Count || playerInputSequence[i] != currentTarget[i])
            {
                sequenceCorrectSoFar = false;
                break;
            }
        }

        if (!sequenceCorrectSoFar)
        {
            Debug.Log("Incorrect sequence!");
            HandleFailure();
        }
        else if (playerInputSequence.Count == currentTarget.Count)
        {
            // Sequence matches the target length and content
            Debug.Log($"Stage {currentStage} complete!");
            HandleSuccess();
        }
        else
        {
            // Correct so far, but sequence not yet complete
            Debug.Log("Correct input, waiting for next...");
        }
    }

    void HandleSuccess()
    {
        // Stop any hint that might be running
        if (hintCoroutine != null)
        {
            StopCoroutine(hintCoroutine);
            hintCoroutine = null;
            RevertAllButtonColors();
        }
        // Start success feedback (flash all buttons)
        feedbackCoroutine = StartCoroutine(FlashAllButtonsRoutine(successFlashColor));

        // Advance to the next stage
        currentStage++;
        playerInputSequence.Clear(); // Reset input for the next stage

        if (currentStage >= targetSequences.Count)
        {
            Debug.Log("Congratulations! All sequences completed!");
            
            // Play completion audio if available, with delay
            if (completionAudioSource != null)
            {
                Debug.Log($"Will play completion audio after {completionAudioDelay}s delay");
                StartCoroutine(PlayDelayedCompletionAudio());
            }
            else
            {
                Debug.LogWarning("Completion audio source not assigned. No sound will play.");
            }
            
            // Open the door after the Simon Says game is completed
            if (!doorOpened && doorAnchor != null)
            {
                doorCoroutine = StartCoroutine(OpenDoorRoutine());
            }
            else if (doorAnchor == null)
            {
                Debug.LogWarning("Door anchor not assigned. Door will not open.");
            }
        }
        else
        {
            Debug.Log($"Advanced to Stage {currentStage}. Waiting for sequence: " + string.Join(", ", targetSequences[currentStage]));
        }
    }

    // New coroutine to play completion audio with delay
    IEnumerator PlayDelayedCompletionAudio()
    {
        // Wait for the initial completion audio delay
        if (completionAudioDelay > 0f)
        {
            yield return new WaitForSeconds(completionAudioDelay);
        }
        
        // Play the initial completion audio
        if (completionAudioSource != null && completionAudioSource.clip != null)
        {
            Debug.Log("Playing initial completion audio now");
            completionAudioSource.Play();
        }
        else if (completionAudioSource != null && completionAudioSource.clip == null)
        {
            Debug.LogWarning("Initial completionAudioSource is assigned, but has no AudioClip. Skipping play.");
        }

        // Now handle the post-completion audio
        if (postCompletionAudioSource != null && postCompletionAudioSource.clip != null)
        {
            // Wait for the post-completion audio delay
            if (postCompletionAudioDelay > 0f)
            {
                Debug.Log($"Waiting {postCompletionAudioDelay}s to play post-completion audio.");
                yield return new WaitForSeconds(postCompletionAudioDelay);
            }
            
            Debug.Log("Playing post-completion audio now.");
            postCompletionAudioSource.Play();
        }
        else if (postCompletionAudioSource != null && postCompletionAudioSource.clip == null)
        {
            Debug.LogWarning("Post-completionAudioSource is assigned, but has no AudioClip. Skipping play.");
        }
    }

    // New coroutine to handle door opening animation
    IEnumerator OpenDoorRoutine()
    {
        doorOpened = true;
        Debug.Log("Opening door...");

        // Determine which transform to rotate (doorToRotate if specified, otherwise this object)
        Transform doorTransform = doorToRotate != null ? doorToRotate.transform : transform;
        
        // Store the initial state
        Vector3 initialPosition = doorTransform.position;
        Quaternion initialRotation = doorTransform.rotation;
        
        // Set up the rotation axis
        Vector3 doorAxis = doorAnchor.up; // Y-axis of the anchor for rotation
        
        // Calculate the door's initial position relative to the hinge/anchor
        Vector3 doorToAnchorVector = initialPosition - doorAnchor.position;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < doorOpenDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / doorOpenDuration);
            
            // Calculate the current angle for this frame
            float currentAngle = t * doorOpenAngle;
            
            // Create a rotation for this frame
            Quaternion currentRotation = Quaternion.AngleAxis(currentAngle, doorAxis) * initialRotation;
            
            // Calculate the new position that maintains the door rotation around the anchor point
            Vector3 rotatedVector = Quaternion.AngleAxis(currentAngle, doorAxis) * doorToAnchorVector;
            Vector3 newPosition = doorAnchor.position + rotatedVector;
            
            // Apply the new position and rotation
            doorTransform.SetPositionAndRotation(newPosition, currentRotation);
            
            yield return null;
        }
        
        Debug.Log("Door opened successfully");
        doorCoroutine = null;
    }

    void HandleFailure()
    {
        // Stop any hint that might be running
        if (hintCoroutine != null)
        {
            StopCoroutine(hintCoroutine);
            hintCoroutine = null;
            RevertAllButtonColors();
        }
        // Start failure feedback (show specific buttons in red)
        feedbackCoroutine = StartCoroutine(ShowFailureRoutine(failureColor));

        // Reset player input for the *current* stage (they have to retry the stage)
        playerInputSequence.Clear();
        Debug.Log($"Failure on Stage {currentStage}. Input reset. Game will restart after feedback.");
    }

    // --- Coroutines for Visual Feedback ---

    IEnumerator FlashAllButtonsRoutine(Color flashColor)
    {
        // Set all buttons to flash color
        foreach (GameObject btn in buttons)
        {
            SetButtonColor(btn, flashColor);
        }

        yield return new WaitForSeconds(feedbackDuration);

        // Revert all buttons to original colors
        RevertAllButtonColors();
        feedbackCoroutine = null; // Signal feedback is finished

        if (currentStage < targetSequences.Count) // Game not yet won
        {
            yield return new WaitForSeconds(pauseBeforeNextHintAfterSuccess); // Brief pause
            if (buttons.Count > 0 && currentStage < targetSequences.Count && targetSequences[currentStage].Count > 0)
            {
                if(hintCoroutine != null) StopCoroutine(hintCoroutine); // Safety stop
                hintCoroutine = StartCoroutine(ShowSequenceHintRoutine(targetSequences[currentStage], false, hintBlinkDuration, pauseBetweenHintBlinksInSequence, 0f));
            }
        }
        else
        {
            Debug.Log("All stages complete! No next hint to show.");
        }
    }

    IEnumerator ShowFailureRoutine(Color failColor)
    {
        List<int> failureButtonIds = new List<int> { 1, 3, 5, 7, 9 };

        // Set specified buttons to failure color
        foreach (int id in failureButtonIds)
        {
            // Ensure the ID is valid (1-based) and within the bounds of the buttons list
            if (id > 0 && id <= buttons.Count)
            {
                // buttons list is 0-indexed, so subtract 1 from id
                SetButtonColor(buttons[id - 1], failColor);
            }
            else
            {
                Debug.LogWarning($"Invalid button ID {id} specified in failureButtonIds. It's out of range for the assigned buttons list (count: {buttons.Count}).");
            }
        }

        yield return new WaitForSeconds(feedbackDuration);

        // Revert all buttons to original colors (simplest way to reset)
        RevertAllButtonColors();
        feedbackCoroutine = null; // Signal feedback is finished
        InitializeGame(); // Restart the game from the beginning
    }

    IEnumerator ShowSequenceHintRoutine(List<int> sequence, bool shouldRepeat, float blinkDur, float interBlinkP, float postSeqP)
    {
        Debug.Log($"SimonSays: Starting sequence hint: [{string.Join(", ", sequence)}], shouldRepeat={shouldRepeat}");
        
        // Ensure all buttons are reset to original colors before starting
        RevertAllButtonColors();

        // Wait a frame to ensure UI updates
        yield return null;
        
        do {
            Debug.Log($"SimonSays: Beginning sequence hint animation pass for sequence: [{string.Join(", ", sequence)}]");
            
            // Process each button in the sequence
            for (int i = 0; i < sequence.Count; i++)
            {
                // Stop immediately if this coroutine is no longer the active one
                if (hintCoroutine == null) {
                    Debug.Log("SimonSays: Hint sequence was stopped. Exiting.");
                    yield break;
                }
                
                int buttonId = sequence[i];
                Debug.Log($"SimonSays: Hinting button {buttonId} (step {i+1}/{sequence.Count})");
                
                if (buttonId <= 0 || buttonId > buttons.Count) {
                    Debug.LogWarning($"SimonSays: Invalid buttonId {buttonId}. Skipping hint step.");
                    continue;
                }
                
                // Get the button GameObject (using 0-based indexing)
                GameObject buttonObj = buttons[buttonId - 1];
                Button uiButton = buttonObj.GetComponent<Button>();
                
                if (uiButton == null) {
                    Debug.LogWarning($"SimonSays: Button {buttonId} has no UI.Button component. Skipping hint step.");
                    continue;
                }
                
                // Get the original colors from our stored dictionary
                ColorBlock originalColors;
                if (!originalButtonColorBlocks.TryGetValue(uiButton, out originalColors)) {
                    Debug.LogWarning($"SimonSays: Original colors for button {buttonId} not found. Using current colors.");
                    originalColors = uiButton.colors;
                }
                
                Debug.Log($"SimonSays: Button {buttonId} original color: {originalColors.normalColor}");
                
                // Create a new ColorBlock with very explicit changes for the hint
                ColorBlock hintColors = new ColorBlock();
                hintColors.normalColor = hintFlashColor;
                hintColors.highlightedColor = hintFlashColor;
                hintColors.pressedColor = originalColors.pressedColor;
                hintColors.selectedColor = originalColors.selectedColor;
                hintColors.disabledColor = originalColors.disabledColor;
                hintColors.colorMultiplier = originalColors.colorMultiplier;
                hintColors.fadeDuration = originalColors.fadeDuration;
                
                // Apply the hint color to this button
                uiButton.colors = hintColors;
                Debug.Log($"SimonSays: Set button {buttonId} to hint color: {hintFlashColor}");
                
                // Force UI refresh
                Canvas.ForceUpdateCanvases();
                
                // Wait for blink duration
                Debug.Log($"SimonSays: Waiting {blinkDur}s for button {buttonId} to show hint color");
                yield return new WaitForSeconds(blinkDur);
                
                // Check if we've been stopped during the blink
                if (hintCoroutine == null) {
                    Debug.Log("SimonSays: Hint sequence was stopped during blink. Exiting.");
                    yield break;
                }
                
                // Restore original colors immediately, regardless of current state
                uiButton.colors = originalColors;
                Debug.Log($"SimonSays: Restored button {buttonId} to original color");
                
                // Force UI refresh
                Canvas.ForceUpdateCanvases();
                
                // Add pause between buttons in sequence (unless this is the last one)
                if (i < sequence.Count - 1) {
                    Debug.Log($"SimonSays: Pausing {interBlinkP}s before next button hint");
                    yield return new WaitForSeconds(interBlinkP);
                    
                    // Check if stopped during inter-blink pause
                    if (hintCoroutine == null) {
                        Debug.Log("SimonSays: Hint sequence was stopped between blinks. Exiting.");
                        yield break;
                    }
                }
            }
            
            // If we're not repeating, break the loop now
            if (!shouldRepeat) {
                Debug.Log("SimonSays: Sequence shown once, not repeating.");
                break;
            }
            
            // Pause after completing the full sequence
            Debug.Log($"SimonSays: Completed one pass of sequence. Pausing {postSeqP}s before repeating.");
            yield return new WaitForSeconds(postSeqP);
            
            // Check if stopped during post-sequence pause
            if (hintCoroutine == null) {
                Debug.Log("SimonSays: Hint sequence was stopped during post-sequence pause. Exiting.");
                yield break;
            }
            
        } while (shouldRepeat && hintCoroutine != null);
        
        // If we made it here, the sequence completed normally
        Debug.Log("SimonSays: Hint sequence completed normally.");
        
        // Clear the hint coroutine field
        hintCoroutine = null;
    }

    // --- Helper Methods ---

    void SetButtonColor(GameObject buttonObj, Color color)
    {
        Button uiButton = buttonObj.GetComponent<Button>();
        if (uiButton != null)
        {
            // Create a completely new ColorBlock instance
            ColorBlock originalCb = uiButton.colors;
            ColorBlock cb = new ColorBlock();
            cb.normalColor = color;
            cb.highlightedColor = color;
            cb.pressedColor = originalCb.pressedColor;
            cb.selectedColor = originalCb.selectedColor;
            cb.disabledColor = originalCb.disabledColor;
            cb.colorMultiplier = originalCb.colorMultiplier;
            cb.fadeDuration = originalCb.fadeDuration;
            
            uiButton.colors = cb;
            
            // Force UI refresh
            Canvas.ForceUpdateCanvases();
        }
        else
        {
            Debug.LogWarning($"Button GameObject {buttonObj.name} does not have a UI.Button component. Cannot set color.");
        }
    }

    void RevertAllButtonColors()
    {
        foreach (KeyValuePair<Button, ColorBlock> entry in originalButtonColorBlocks)
        {
            if (entry.Key != null) // Check if the button component still exists
            {
                entry.Key.colors = entry.Value; // Restore the original ColorBlock
            }
        }
    }
}
