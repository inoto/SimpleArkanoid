using UnityEngine;

namespace SimpleArkanoid
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] SpriteRenderer Field;

        Camera _camera;

        void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        void Start()
        {
            Vector2 fieldSize = Field.size * Field.transform.localScale;
            fieldSize *= 1.2f; // hack to make good UI for windows ratios
            if (Screen.width / (float)Screen.height <= fieldSize.x / fieldSize.y)
                _camera.orthographicSize = (fieldSize.x * Screen.height) / (Screen.width * 2f);
            else
                _camera.orthographicSize = (fieldSize.y / 2f);
        }
    }
}