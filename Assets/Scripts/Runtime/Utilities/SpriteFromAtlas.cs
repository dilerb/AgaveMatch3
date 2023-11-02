using Sirenix.OdinInspector;
using UnityEditor.Rendering;
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
            GenerateSprite(spriteName);
        }
        
        [Button]
        public void GenerateSprite(string s)
        {
            var sprite = spriteAtlas.GetSprite(s);

            if (sprite != null)
            {
                GetComponent<SpriteRenderer>().sprite = sprite;
            }
            else
            {
                Debug.LogWarning("Sprite not found!: " + s);
            }
        }
    }
}