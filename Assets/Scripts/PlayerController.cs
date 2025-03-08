using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public int speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private int count;
    private Label scoreText;
    private Label winLoseText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        var uiDocument = Object.FindFirstObjectByType<UIDocument>();
        if (uiDocument != null)
        {
            var root = uiDocument.rootVisualElement;
            scoreText = root.Q<Label>("scoretxt"); 
            winLoseText = root.Q<Label>("winlosetxt");

            winLoseText.style.display = DisplayStyle.None;
        }

        SetCountText();
    }

    void OnMove(InputValue value)
    {
        Vector2 movementVector = value.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + count.ToString();
        }

        if (count >= 12)
        {
            if (winLoseText != null)
            {
                winLoseText.text = "You Win!";
                winLoseText.style.display = DisplayStyle.Flex;
            }
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            CameraController cameraController = Object.FindFirstObjectByType<CameraController>();
            if (cameraController != null)
            {
                cameraController.enabled = false;
            }
            Destroy(gameObject);

            if (winLoseText != null)
            {
                winLoseText.text = "You Lose!";
                winLoseText.style.display = DisplayStyle.Flex;
            }

        }

    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }
    }
}
