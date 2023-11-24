using UnityEngine;

namespace Interactions.ObjectFinding
{
    public abstract class PointableObject : MonoBehaviour
    {
        public virtual void OnPointed()
        {
            // Highlight object
        }
        
        public virtual void OnUnpointed()
        {
            // Unhighlight object
        }
        
    }
}
