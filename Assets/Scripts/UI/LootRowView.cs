using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LootRowView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;

        private readonly Vector3 _scalePunchStrength = new Vector3(0.2f, 0.2f, 0.2f);

        public void SetText(string text) =>
            _text.text = text;

        public void PlayIncreaseAnimation()
        {
            transform.DOComplete();
            transform.DOPunchScale(_scalePunchStrength, 0.3f, 0, 0.0f);
        }

        public void SetSprite(Sprite sprite) =>
            _image.sprite = sprite;
    }
}