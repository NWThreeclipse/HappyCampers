using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerController : MonoBehaviour
{
    // Movement
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private TMPro.TextMeshProUGUI moveSpeedText;
    [SerializeField] private UnityEngine.UI.Slider moveSpeedSlider;

    private float capturedBulletTime = 0.0f;
    private float elapsedTime = 0.0f;   // Accumulated time since script start

    //Player Components
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool camperInRange = false;
    private bool isChoosingConversationOption = false; // New state variable
    private string conversationTargetName = "";         // New variable to hold the NPC's name

    private int camperPoints = 0;

    // public TextMeshPro camperHUD; 
    [SerializeField] private TMPro.TextMeshProUGUI camperHUD;
    [SerializeField] private TMPro.TextMeshProUGUI camperItem;

    // --- ITEM PICKUP ---
    private bool itemInRange = false;                 // Tracks if any item is close
    [SerializeField] private Item itemToPickUp = null;                // Reference to the item component
    private bool isHoldingItem = false;               // New variable to track if an item is currently held

    private bool toggleRun = false;


    private void Awake()
    {
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0.0f; // No gravity
        camperItem.text = "N/A";
    }


    void Update()
    {

        // Get movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        // Set animator bool
        bool isWalking = movement != Vector2.zero;
        // animator.SetBool("isWalking", isWalking);

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Debug.Log("Mouse World Position: " + mouseWorldPos);

        // Flip sprite if moving left/right
        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }
        // else if(rb.position.x < mouseWorldPos.x)
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.position.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        // else
        // {
        //     spriteRenderer.flipX = true;
        // }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("RUNNING");
            toggleRun = !toggleRun;
            // moveSpeed = 10f;

            // score += 2000; // Increment score by 10 when H is presse`d
        }

        if (toggleRun)
            moveSpeed = 10f;
        else
            moveSpeed = 3f;

        // if (camperInRange)
        // {
        //     if (Input.GetKeyDown(KeyCode.E))
        //     {
        //         // StartConversation(other.name);
        //         StartConversation("Camper");
        //     }
        // }

        if (camperInRange && !isChoosingConversationOption)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // When E is pressed, transition to the choice state
                EnterConversationChoiceState("Camper");
            }
        }

        // if (itemInRange && !camperInRange && itemToPickUp != null)
        if (itemInRange && !camperInRange && itemToPickUp != null && !isHoldingItem) 
        {
            if (Input.GetKeyDown(KeyCode.E) && !isHoldingItem)
            {
                PickUpItem(itemToPickUp);
            }
        }

        if (isHoldingItem && Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }

        // --- INPUT CHECK FOR DIALOGUE OPTIONS (1, 2, 3) ---
        if (isChoosingConversationOption)
        {
            int option = 0;
            if (Input.GetKeyDown(KeyCode.Alpha1)) option = 1;
            else if (Input.GetKeyDown(KeyCode.Alpha2)) option = 2;
            else if (Input.GetKeyDown(KeyCode.Alpha3)) option = 3;

            if (option != 0)
            {
                // If a valid option is pressed, process it and exit the choice state
                HandleConversationOption(option, conversationTargetName);
            }

            // if(Input.GetKeyDown(KeyCode.Escape))
            //     DialogBox.instance.CloseDialog();
        }

    }

    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        // Debug.Log("Elapsed Time: " + elapsedTime);
        // rb.AddForce(movement * moveSpeed * Time.deltaTime);
        rb.linearVelocity = movement.normalized * moveSpeed;

        // Set animator parameters based on velocity direction
        Vector2 velocity = rb.linearVelocity;

        bool isWalkingHorizontal = velocity.x > 0.01f || velocity.x < -0.01f;
        animator.SetBool("isWalkingRight", isWalkingHorizontal);
        animator.SetBool("isWalkingUp", velocity.y > 0.01f);
        animator.SetBool("isWalkingDown", velocity.y < -0.01f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.CompareTag("Ground"))
        {
            // Debug.Log("Grounded");
            // isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // if (collision.gameObject.CompareTag("Ground"))
        {
            // Debug.Log("Grounded FAlse");
            // isGrounded = false;
        }
    }

    // Called when another collider enters the circle trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // /Debug.Log("Object entered: " + other.name);
        if (other.name == "Camper")
        {
            camperInRange = true;
            Debug.Log($"{other.name} says hi");
            Debug.Log($"Would you like to start a conversation with {other.name}");
            Debug.Log($"Would you like to start a conversation with {other.name}");
        }
        else if (other.name == "torch")
        {
            Debug.Log($"{other.name} ");
            Torch torch = other.GetComponent<Torch>();
            if (torch != null)
            {
                // Torch logic here
            }
        }


         // Item Check
        Item item = other.GetComponent<Item>();
        if (item != null)
        {
            if (isHoldingItem) return;
            // Only consider pickup if the item is NOT already picked up
            if (!item.ItemIsPickedUp)
            {
                itemInRange = true;
                itemToPickUp = item;
                Debug.Log($"Press 'E' to pick up the {other.name}.");
            }
        }
    }

    // Called while another collider stays inside the circle trigger
    private void OnTriggerStay2D(Collider2D other)
    {
        // Debug.Log("Object staying: " + other.name);

    }

    // Called when another collider exits the circle trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        // Debug.Log("Object exited: " + other.name);
        if (other.name == "Camper")
        {
            // playerInRange = false;
            Debug.Log($"{other.name} walked away. Conversation cancelled.");
            DialogBox.instance.CloseDialog();
            camperInRange = false;
        }


           // Item Exit
        Item item = other.GetComponent<Item>();
        if (item != null && itemToPickUp == item)
        {
            itemInRange = false;
            // itemToPickUp = null;
            Debug.Log($"You walked away from the {other.name}.");
        }
    }



    private void EnterConversationChoiceState(string playerName)
    {
        // Set the state and target
        isChoosingConversationOption = true;
        conversationTargetName = playerName;

        // Display the prompt to the player
        // Debug.Log($"--- Starting conversation with {playerName} ---");
        DialogBox.instance.ShowDialog($"--- Starting conversation with {playerName} ---");
        // Debug.Log("Choose an option:");
        // Debug.Log("Press '1' for: Small Talk");
        // Debug.Log("Press '2' for: Entertain");
        // Debug.Log("Press '3' for: Compete");
        // Here you would open a dialogue UI with the buttons/options
       StartCoroutine(ShowOptionsAfterDialog());
    }


    private IEnumerator ShowOptionsAfterDialog()
    {
        // Wait until the current dialog is fully typed and player presses the key
        while (DialogBox.instance.IsTyping)
            yield return null;

        // Now show the conversation options
        string options = "Choose an option:\n" +
                         "Press '1' for: Small Talk\n" +
                         "Press '2' for: Entertain\n" +
                         "Press '3' for: Compete";

        DialogBox.instance.ShowDialog(options);

        // At this point you could enable UI buttons or input handling
    }

    private void HandleConversationOption(int option, string playerName)
    {
        // Reset the choice state so the player can move/do other things
        isChoosingConversationOption = false;

        switch (option)
        {
            case 1:
                DialogBox.instance.ShowDialog( $"[SMALL TALK] Dialog Begins with {playerName}.");
                // Debug.Log($"[SMALL TALK] Dialog Begins with {playerName}.");
                int conversationRating = UnityEngine.Random.Range(1, 11);
                if (conversationRating < 5)
                    // camperPoints -= 1;
                    AddPoints(0);
                else
                    // camperPoints += 1;
                    AddPoints(1);
                // Call method to start the small talk sequence
                // AddPoints()
                break;

            case 2:
                // Debug.Log($"[ENTERTAIN] Attempting to entertain {playerName}.");
                DialogBox.instance.ShowDialog( $"[SMALL TALK] Dialog Begins with {playerName}.");
                int entertainmentRating = UnityEngine.Random.Range(1, 11);
                if (entertainmentRating < 5)
                    // camperPoints -= 1;
                    AddPoints(0);
                else
                    // camperPoints += 1;
                    AddPoints(1);

                // Call method to start the entertainment sequence
                break;

            case 3:
                // Debug.Log($"[COMPETE] Challenging {playerName} to a competition.");
                DialogBox.instance.ShowDialog( $"[SMALL TALK] Dialog Begins with {playerName}.");
                int competitionRating = UnityEngine.Random.Range(1, 11);
                if (competitionRating < 5)
                    // camperPoints -= 1;
                    AddPoints(0);
                else
                    // camperPoints += 1;
                    AddPoints(1);
                // Call method to start the competition sequence
                break;
        }

        // Clean up the target name
        conversationTargetName = "";
    }

    public void AddPoints(int pointsToAdd)
    {
        camperPoints += pointsToAdd;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        // The format specifier "D6" means: 
        // "Format this as a Decimal integer, using a minimum of 6 digits."
        // If currentScore is 123, the string will be "000123".
        // If currentScore is 5, the string will be "000005".

        camperHUD.text = camperPoints.ToString("D6");
    }

    private void PickUpItem(Item item)
    {
        item.ItemIsHeld(transform);
        camperItem.text = item.name;
        itemToPickUp = item;
        isHoldingItem = true;
        Debug.Log($"You picked up the {item.name}!");
        itemInRange = false;

    }
    
    private void DropItem()
    {
        Debug.Log("Item dropped!");
        camperItem.text = "N/A";
        if (itemToPickUp == null) return;
        itemToPickUp.ItemIsDropped();
        isHoldingItem = false;
        itemToPickUp = null; // Clear the reference since we're no longer holding it
    }
   

    
}
