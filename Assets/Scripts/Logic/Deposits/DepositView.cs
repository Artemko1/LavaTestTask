using System.Collections.Generic;
using Data.DataLoot;
using DG.Tweening;
using Logic.Drop;
using UnityEngine;

namespace Logic.Deposits
{
    [RequireComponent(typeof(DropSpawner))]
    public class DepositView : MonoBehaviour
    {
        [SerializeField] private GameObject _viewBase;
        [SerializeField] private Transform _dropSpawnPoint;
        [SerializeField] private List<GameObject> _viewVariants;

        private DropSpawner _dropSpawner;

        private void Awake() =>
            _dropSpawner = GetComponent<DropSpawner>();

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

        public void DropLoot(Loot loot)
        {
            for (var i = 0; i < loot.Amount; i++)
            {
                _dropSpawner.SpawnCollectableWithBurst(new Loot(loot.Type, 1), _dropSpawnPoint.position, Random.rotation);
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