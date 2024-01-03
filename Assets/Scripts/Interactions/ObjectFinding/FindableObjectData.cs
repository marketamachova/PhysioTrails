using UnityEngine;

namespace Interactions.ObjectFinding
{
    public abstract class FindableObjectData : ScriptableObject
    {
        [SerializeField] private string objectName;
        [SerializeField] private int type;
        [SerializeField] private Mesh mesh;
        [SerializeField] private Material material;
        
        public string ObjectName => objectName;
        public Mesh Mesh => mesh;
        public Material Material => material;
        public int Type => type;
    }
}
