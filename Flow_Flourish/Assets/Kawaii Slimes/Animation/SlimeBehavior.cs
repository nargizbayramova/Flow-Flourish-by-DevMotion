using UnityEngine;

public class SlimeBehavior : MonoBehaviour
{
    private Animator anim;
    private bool isJumping = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component missing!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // Handle touch input for mobile
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                ToggleJump();
            }
        }

        // Optional: Mouse input for testing in editor
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                ToggleJump();
            }
        }
#endif
    }

    void ToggleJump()
    {
        isJumping = !isJumping;
        anim.SetBool("IsJumping", isJumping);

        // Optional: Add jump force if using physics
        // GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
    }
}