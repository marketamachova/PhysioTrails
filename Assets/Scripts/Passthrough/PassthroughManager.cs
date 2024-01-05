using UnityEngine;
using Utils;

namespace Passthrough
{
    public class PassthroughManager : MonoSingleton<PassthroughManager>
    {
        [SerializeField] private OVRPassthroughLayer ovrPassthroughLayer;
        void Start()
        {
            EnablePassthrough(false);
        }
        
        public void EnablePassthrough(bool enable)
        {
            if (ovrPassthroughLayer == null)
            {
                Debug.Log("Kuk ovr passthrough layer is null");
                return;
            }
            Debug.Log("Kuk enabling passthrough");
            ovrPassthroughLayer.hidden = enable;
        }
    }
}
