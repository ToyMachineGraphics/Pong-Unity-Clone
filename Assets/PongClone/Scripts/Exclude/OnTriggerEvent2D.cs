using UnityEngine;

namespace PongClone
{
    public class OnTriggerEvent2D : MonoBehaviour
    {
        public ColliderEvent2D onEnter;
        public ColliderEvent2D onStay;
        public ColliderEvent2D onExit;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            onEnter?.Invoke(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            onStay?.Invoke(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            onExit?.Invoke(collision);
        }
    }
}