using System;
using System.Collections.Generic;
using Ld50.Characters;
using Ld50.Interactable;
using Ld50.Interactable.Shields;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Ld50.Core
{
    public class GameTimeManager : MonoBehaviour
    {
        public PlayerMovement player;

        public Transform gameStartPos, gameEndPos;

        public float startDuration = 5f;

        public bool isPlaying, gameStarting, gameOver, waitRestart;

        public Explosion explosion;

        public Fire firePrefab;

        public Hole holePrefab;

        public Turret mainTurret;

        public Shield[] shields;

        public List<Transform> firePositions, holePositions;

        public Transform fireGroup, holeGroup;

        private List<Callback> _callbacks; // (1 / float seconds) probability

        private CameraFollower _cameraFollower;
        
        private Ship.Ship _ship;

        private void Awake()
        {
            _cameraFollower = FindObjectOfType<CameraFollower>();
            _ship = FindObjectOfType<Ship.Ship>();
            _callbacks = new List<Callback> {
                new(25, StartFire),
                new(25, MakeHole),
                new(45, BrakeShield),
                new(45, BrakeTurret),
            };

            _cameraFollower.enabled = false;
        }

        private void StartFire()
        {
            var placeIndex = Random.Range(0, firePositions.Count);
            var pos = firePositions[placeIndex].position;

            Instantiate(firePrefab, pos, Quaternion.identity, fireGroup);
        }

        private void MakeHole()
        {
            if (holePositions.Count == 0)
                return;

            var placeIndex = Random.Range(0, holePositions.Count);
            var pos = holePositions[placeIndex].position;

            holePositions.RemoveAt(placeIndex);

            Instantiate(holePrefab, pos, Quaternion.identity, holeGroup);
        }

        private void BrakeShield() => shields[Random.Range(0, shields.Length)].Brake();


        private void BrakeTurret() => mainTurret.Brake();

        private void Update()
        {
            if (isPlaying)
            {
                SpawnEvents();

                if (_ship.health < 0)
                {
                    gameOver = true;
                    isPlaying = false;

                    for (int i = 0; i < 4; i++)
                    {
                        var pos = player.transform.position + (Vector3)Random.insideUnitCircle * 5f;
                        Instantiate(explosion, pos, Quaternion.identity);
                    }

                    var startPos = _cameraFollower.transform.position;
                    _cameraFollower.enabled = false;
                    var t = 0f;

                    Observable
                        .EveryUpdate()
                        .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(startDuration)))
                        .Do(
                            _ =>
                            {
                                _cameraFollower.transform.position = Vector3.Lerp(
                                    startPos,
                                    gameEndPos.position,
                                    t / startDuration
                                );
                                t += Time.deltaTime;
                            }
                        )
                        .DoOnCompleted(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name))
                        .Subscribe()
                        .AddTo(this);
                }
            }
            else if (gameOver)
            {
                if (waitRestart && Input.anyKey)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
            else
            {
                WaitForGameStart();
            }
        }

        private void SpawnEvents()
        {
            var e = Random.value;

            foreach (var callback in _callbacks)
            {
                if (e <= callback.Frequency)
                {
                    callback.callback();
                    break;
                }

                e -= callback.Frequency;
            }
        }

        private void WaitForGameStart()
        {
            if (!Input.anyKey || gameStarting)
                return;

            gameStarting = true;

            var startPos = _cameraFollower.transform.position;
            var t = 0f;

            Observable
                .EveryUpdate()
                .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(startDuration)))
                .Do(
                    _ =>
                    {
                        _cameraFollower.transform.position = Vector3.Lerp(startPos, player.transform.position, t / startDuration);
                        t += Time.deltaTime;
                    }
                )
                .DoOnCompleted(
                    () =>
                    {
                        _cameraFollower.enabled = true;
                        player.GetComponent<Collider2D>().enabled = true;
                        isPlaying = true;
                        gameStarting = false;
                    }
                )
                .Subscribe()
                .AddTo(this);
        }

        [Serializable]
        private class Callback
        {
            public float averagePerSeconds;

            public Action callback;

            public float Frequency => Time.deltaTime / averagePerSeconds;

            public Callback(float averagePerSeconds, Action callback)
            {
                this.averagePerSeconds = averagePerSeconds;
                this.callback = callback;
            }
        }
    }
}