using UnityEngine;
using UnityEngine.EventSystems;

namespace DecorPackA.UI
{
    public class FKnobControl : MonoBehaviour, IDragHandler
    {
        [SerializeField] public Transform knob;

        public event System.Action OnChanged;

        private float _angle;

        public float Angle
        {
            get => _angle;
            set
            {
                _angle = value;
                SetKnobRotation();
            }
        }

        private Vector2 mousePos;

        public void OnDrag(PointerEventData eventData)
        {
            mousePos = (Vector2)KInputManager.GetMousePos();

            Angle = Mathf.Atan2(
                mousePos.y - transform.position.y,
                mousePos.x - transform.position.x) * Mathf.Rad2Deg;

            Angle -= 90f;

            SetKnobRotation();
            OnChanged?.Invoke();
        }

        private void SetKnobRotation()
        {
            var targetRotation = Quaternion.Euler(new Vector3(0, 0, Angle));
            knob.transform.rotation = targetRotation;
        }
    }
}
