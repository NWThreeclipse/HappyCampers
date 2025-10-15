using UnityEngine;
using TMPro;
using System.Collections;

public class DialogBox : MonoBehaviour
{
    public static DialogBox instance;

    public TextMeshProUGUI dialogText;
    public float textSpeed = 0.05f;
    public GameObject dialogPanel;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    public bool IsTyping => isTyping;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (dialogPanel != null)
            dialogPanel.SetActive(false);
    }

    // Called by external character to start dialog
    public void ShowDialog(string message)
    {
        if (dialogPanel != null)
            dialogPanel.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(message));
    }
    // Call this to immediately close the dialog
    public void CloseDialog()
    {
        // Stop typing coroutine if running
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    
        // Clear text
        if (dialogText != null)
            dialogText.text = "";
    
        // Hide the dialog panel
        if (dialogPanel != null)
            dialogPanel.SetActive(false);
    }

    private IEnumerator TypeText(string message)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (char c in message)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;

        // Wait for player to press a key to continue
        yield return StartCoroutine(WaitForNextInput());

        ClearText();
    }

    private IEnumerator WaitForNextInput()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
    }

    // Clear the text (can be triggered by character as well)
    public void ClearText()
    {
        dialogText.text = "";
        if (dialogPanel != null)
            dialogPanel.SetActive(false);
    }
}
