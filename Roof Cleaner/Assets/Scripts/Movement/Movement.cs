using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] private MouseInput _mouseInput;
    [SerializeField] private SurfaceSlider _surfaceSlider;

    [SerializeField] private SpriteMask _spriteMask;
    [SerializeField] private Transform _maskPosition;

    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (GameLoop.CanMovePlayer == true)
        {
            var mask = Instantiate(_spriteMask, _maskPosition.position, Quaternion.identity);

            Vector3 swerveAmount = Time.deltaTime * _speed * _mouseInput.MoveFactor;
            Vector3 directionAlongSurface = _surfaceSlider.Project(swerveAmount);

            _rigidbody.MovePosition(_rigidbody.position + directionAlongSurface);

            ClampPosition();
        }
    }

    private void ClampPosition()
    {
        _rigidbody.position = new Vector3(
            Mathf.Clamp(_rigidbody.position.x, ((_surfaceSlider._border.x / 2) * -1) + _surfaceSlider._offsetX, (_surfaceSlider._border.x / 2) - _surfaceSlider._offsetX),
            Mathf.Clamp(_rigidbody.position.y, (_surfaceSlider._borderMin.y + _surfaceSlider._offsetY), (_surfaceSlider._borderMax.y - _surfaceSlider._offsetY)),
            Mathf.Clamp(_rigidbody.position.z, (_surfaceSlider._borderMin.z + _surfaceSlider._offsetZ), (_surfaceSlider._borderMax.z - _surfaceSlider._offsetZ)));
    }
}
