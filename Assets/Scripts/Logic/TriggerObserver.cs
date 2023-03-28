using System;
using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(Collider))]
    public class TriggerObserver : MonoBehaviour
    {
        private Collider _collider;

        private void OnTriggerEnter(Collider other) => 
            TriggerEnter?.Invoke(other);

        private void OnTriggerExit(Collider other) =>
            TriggerExit?.Invoke(other);

        private void OnTriggerStay(Collider other) =>
            TriggerStay?.Invoke(other);

        public event Action<Collider> TriggerEnter;
        public event Action<Collider> TriggerExit;
        public event Action<Collider> TriggerStay;
    }
}