using UnityEngine;

namespace Interactions.AvoidObstacles
{
    public class AvoidObstaclesController : MonoBehaviour
    {
        [SerializeField] private AvoidObstaclesScoreController scoreController;
        
        public void OnHit()
        {
            Debug.Log("Kuk HIT");
        }
        
        public void OnAvoidCorrect()
        {
            Debug.Log("Kuk avoid CORRECT");

        }
        
        public void OnAvoidIncorrect()
        {
            Debug.Log("Kuk avoid INCORRECT");

        }
    }
}
