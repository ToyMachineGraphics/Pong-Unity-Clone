using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PongClone
{
    public abstract class Gameplay : MonoBehaviour
    {
        public GameObject leftPlayer, rightPlayer;
        public BasePlayer me, opponent;

        #region Setting
        protected const int MAX_SPEED = 32;
        [Range(1, MAX_SPEED)]
        public float playerSpeed = 1;
        [Range(1, MAX_SPEED)]
        public float ballSpeed = 1;
        public bool rightHandedness = true;
        #endregion

        public BaseBall ball;
        public Wall leftWall;
        public Wall rightWall;

        public GameObject deathVFX;

        public GlobalData globalData;
        public SceneFader sceneFader;
        [SerializeField] private string _mainMenu = null;

        public abstract void ManualStart();

        protected virtual void Initialize(GameObject leftPlayer, GameObject rightPlayer, Type opponentType)
        {
            if (rightHandedness)
            {
                InitializeForRightHandedness(leftPlayer, rightPlayer, opponentType, out me, out opponent);
            }
            else
            {
                InitializeForLeftHandedness(leftPlayer, rightPlayer, opponentType, out me, out opponent);
            }
            
            me.speed = opponent.speed = playerSpeed;
            me.Ball = opponent.Ball = ball;
            me.preHitObject += globalData.PlayBallHitSoundWith;
            opponent.preHitObject += globalData.PlayBallHitSoundWith;
            ball.speed = ballSpeed;
            ball.onHitCeilingFloor = globalData.PlayBallHitSoundWith;

            me.myTurn = true;
            opponent.myTurn = false;
        }

        protected void InitializeForRightHandedness(GameObject leftPlayer, GameObject rightPlayer, Type opponentType, out BasePlayer me, out BasePlayer opponent)
        {
            me = rightPlayer.AddComponent<ManualPlayer>();
            me.id = 1;
            me.facing = Direction.Left;
            opponent = leftPlayer.AddComponent(opponentType) as BasePlayer;
            opponent.id = 0;
            opponent.facing = Direction.Right;
        }

        protected void InitializeForLeftHandedness(GameObject leftPlayer, GameObject rightPlayer, Type opponentType, out BasePlayer me, out BasePlayer opponent)
        {
            me = leftPlayer.AddComponent<ManualPlayer>();
            me.id = 0;
            me.facing = Direction.Right;
            opponent = rightPlayer.AddComponent(opponentType) as BasePlayer;
            opponent.id = 1;
            opponent.facing = Direction.Left;
        }

        protected void Fail(BasePlayer player)
        {
            Debug.Log("Fail");
            ((IPause)me).Pause();
            ((IPause)opponent).Pause();
            StartCoroutine(Explode(player));
        }

        private IEnumerator Explode(BasePlayer player)
        {
            WaitForSeconds flash = new WaitForSeconds(0.1f);
            int times = 0;
            while (times < 7)
            {
                player.Blink();
                times++;
                yield return flash;
            }
            globalData.PlayDeathSound();
            player.gameObject.SetActive(false);
            Instantiate(deathVFX, player.transform.position, Quaternion.identity);
        }

        /// <summary>
        /// Load current scene as a means of replay. Called by UI button.
        /// </summary>
        public void LoadCurrentScene()
        {
            Scene scene = SceneManager.GetActiveScene();
            //SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
            sceneFader.LoadSceneAsync(scene.name);
        }

        public void ToMainMenu()
        {
            sceneFader.LoadSceneAsync(_mainMenu);
        }
    }
}