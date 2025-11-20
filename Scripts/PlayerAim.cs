using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace AimlabDemo { 
public class PlayerAim : MonoBehaviour
{
    public float mouseSensitivity = 3f;
    public bool uiPanelActive = false;
    Camera cam;
    float xRotation = 0f;
        public UIManager uIManager;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            cam = Camera.main;
        }

        void Update()
        {
            if (uiPanelActive)
            {
                Cursor.lockState = CursorLockMode.None;
                return;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
              
            // Mouse Look 
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Horizontal rotation
            transform.Rotate(0f, mouseX, 0f);

            // Vertical rotation (clamped)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);

            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Shooting
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                Shoot();
            }
        }

        void Shoot()
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                Target target = hit.collider.GetComponent<Target>();

                if (target != null)
                {
                    target.Hit(); 
                    return;
                }
            }

            UIManager.Instance.RegisterMiss();
        }
    }
}
