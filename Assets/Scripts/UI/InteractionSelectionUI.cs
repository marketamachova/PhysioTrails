using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class InteractionSelectionUI : MonoBehaviour
    {
        [SerializeField] private List<GameObject> wireLoopSubmenuItems;
        [SerializeField] private List<GameObject> objectFindingSubmenuItems;
        [SerializeField] private List<GameObject> avoidObstaclesSubmenuItems;
        
        private void Start()
        {
            DisableAllSubmenus();
        }

        public void EnableWireLoopSubmenu()
        {
            DisableAllSubmenus();
            wireLoopSubmenuItems.ForEach(item => item.SetActive(true));
        }
        
        public void EnableObjectFindingSubmenu()
        {
            DisableAllSubmenus();
            objectFindingSubmenuItems.ForEach(item => item.SetActive(true));
        }
        
        public void EnableAvoidObstaclesSubmenu()
        {
            DisableAllSubmenus();
            avoidObstaclesSubmenuItems.ForEach(item => item.SetActive(true));
        }
        
        public void DisableAllSubmenus()
        {
            wireLoopSubmenuItems.ForEach(item => item.SetActive(false));
            objectFindingSubmenuItems.ForEach(item => item.SetActive(false));
            avoidObstaclesSubmenuItems.ForEach(item => item.SetActive(false));
        }
    }
}
