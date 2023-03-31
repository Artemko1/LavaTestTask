using System;
using System.Collections.Generic;
using DG.Tweening;
using Logic.Drop;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logic.Deposits
{
    public class DepositView : MonoBehaviour
    {
        [SerializeField] private GameObject _viewBase;
        [SerializeField] private Transform _dropSpawnPoint;
        [SerializeField] private List<GameObject> _viewVariants;
        [SerializeField] private DroppedLoot _droppedLootPrefab;

        private void Start()
        {
            DisableAllViews();
            _viewVariants[^1].SetActive(true);
        }

        public void PlayMiningAnimation() => 
            _viewBase.transform.DOPunchScale(new Vector3(-0.3f, -0.3f, -0.3f), 0.3f, elasticity: 0.5f);

        public void ChangeView(int miningLeft)
        {
            DisableAllViews();

            int newViewIndex = Mathf.Clamp(miningLeft, 0, _viewVariants.Count - 1);
            _viewVariants[newViewIndex].SetActive(true);
        }

        public void DropResources(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                DroppedLoot drop = Instantiate(_droppedLootPrefab, _dropSpawnPoint.position, Random.rotation);

                Vector2 randomHorizontalMagnitude = Random.insideUnitCircle * Random.Range(0.75f, 1.25f);
                float verticalMagnitude = Random.Range(4f, 5f);

                var rb = drop.GetComponent<Rigidbody>();
                rb.AddForce(randomHorizontalMagnitude.x, verticalMagnitude, randomHorizontalMagnitude.y, ForceMode.VelocityChange);
            }
        }

        private void DisableAllViews()
        {
            foreach (GameObject viewVariant in _viewVariants)
            {
                viewVariant.SetActive(false);
            }
        }
    }
}