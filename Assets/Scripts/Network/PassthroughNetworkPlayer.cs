using System;
using Mirror;
using Passthrough;
using UnityEngine;
using Utils;

namespace Network
{
    public class PassthroughNetworkPlayer : NetworkBehaviour
    {
        [SyncVar(hook = "OnEnablePassthrough")] public bool enablePassthrough = false;

        private bool _isVr;

        private void Start()
        {
            _isVr = DeviceTypeChecker.Instance.IsVr;
        }

        private void OnEnablePassthrough(bool oldEnable, bool newEnable)
        {
            if (_isVr)
            {
                Debug.Log("Kuk on enable passthrough network");
                PassthroughManager.Instance.EnablePassthrough(newEnable);
            }
        }
        
        [Command]
        public void CmdEnablePassthrough(bool enable)
        {
            Debug.Log("Kuk cmd enable passthrough");
            enablePassthrough = enable;
        }
    }
}
