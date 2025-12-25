using Gameplay.General.Configs;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.Current.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameInfo", fileName = "GameInfoConfig")]
    public class GameInfoConfig : BaseGameInfoConfig
    {
        [Space(20)]
        [Header("GAME settings:")]
        [SerializeField] private float planeSpeed = 10;
        [Space]
        [SerializeField][MinMaxSlider(1, 30)] private Vector2 rocketsSpeeds;
        [SerializeField][MinMaxSlider(1, 30)] private Vector2 followingDurations;
        [SerializeField] private float rocketRotationValue = 1;
        [Space]
        [SerializeField][MinMaxSlider(1, 30)] private Vector2 rocketsSpawnDelay;
        [SerializeField] private float rocketsSpawnDecreaseValue = 0.1f;
        
        public float PlaneSpeed => planeSpeed;
        
        public Vector2 RocketsSpeeds => rocketsSpeeds;
        public Vector2 FollowingDurations => followingDurations;
        public float RocketRotationValue => rocketRotationValue;
        
        public Vector2 RocketsSpawnDelay => rocketsSpawnDelay;
        public float RocketsSpawnDecreaseValue => rocketsSpawnDecreaseValue;
    }
}