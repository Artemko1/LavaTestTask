using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Logic.Deposits
{
    public class DepositView : MonoBehaviour
    {
        [SerializeField] private GameObject _viewBase;
        [SerializeField] private Transform _dropSpawnPoint;
        [SerializeField] private List<GameObject> _viewVariants;
        [SerializeField] private GameObject _droppedResourcePrefab;

        public void PlayMining(int miningLeft)
        {
            _viewBase.transform.DOPunchScale(new Vector3(-0.3f, -0.3f, -0.3f), 0.3f, elasticity: 0.5f);

            foreach (GameObject viewVariant in _viewVariants)
            {
                viewVariant.SetActive(false);
            }

            int newViewIndex = Mathf.Clamp(miningLeft, 0, _viewVariants.Count - 1);
            _viewVariants[newViewIndex].SetActive(true);
        }

        public void DropResources(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var instance = Instantiate(_droppedResourcePrefab, _dropSpawnPoint.position, Random.rotation)
                    .GetComponent<Rigidbody>();

                Vector2 randomHorizontalMagnitude = Random.insideUnitCircle * Random.Range(0.75f, 1.25f);
                float verticalMagnitude = Random.Range(4f, 5f);

                instance.AddForce(randomHorizontalMagnitude.x, verticalMagnitude, randomHorizontalMagnitude.y, ForceMode.VelocityChange);
            }
        }
    }
}