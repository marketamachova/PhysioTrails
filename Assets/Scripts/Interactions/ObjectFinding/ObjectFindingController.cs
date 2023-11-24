using UnityEngine;

namespace Interactions.ObjectFinding
{
    public class ObjectFindingController : MonoBehaviour
    {
        [SerializeField] private ScoreController scoreController;
        
        [SerializeField] private int collectType = 0; 
        
        public void OnPointedCorrectly()
        {
            scoreController.OnHit();
        }
        
        public void OnPointedIncorrectly()
        {
            scoreController.OnMiss();
        }

        public int CollectType => collectType;
    }
}
