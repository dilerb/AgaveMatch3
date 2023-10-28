using UnityEngine;
using UnityEngine.U2D;

namespace Runtime.Utilities
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteFromAtlas: MonoBehaviour
    {
        [SerializeField] private SpriteAtlas spriteAtlas;
        [SerializeField] private string spriteName;

        private void Start()
        {
            var sprite = spriteAtlas.GetSprite(spriteName);

            if (sprite != null)
            {
                GetComponent<SpriteRenderer>().sprite = sprite;
            }
            else
            {
                Debug.LogWarning("Sprite not found!: " + spriteName);
            }
        }
    }
}