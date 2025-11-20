using NUnit.Framework.Internal;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AimlabDemo { 
public class UIManager : MonoBehaviour
{
        public static UIManager Instance;

        public PlayerAim player;
        public TargetSpawner targetSpawner;
        private Slider sensitivitySlider;
        private Slider xSpawnDistanceSlider;
        private Slider ySpawnDistanceSlider;
        private TextMeshProUGUI sliderValueText;
        private TextMeshProUGUI xDistanceValueSliderValueText;
        private TextMeshProUGUI yDistanceValueSliderValueText;
        public GameObject settingsPanel;

        float lastHitTime = 0f;
        private float currentSpeed = 0f; 
        private float averageSpeed = 0f;
        private int totalHits = 0;
        private int score = 0;
        private float totalScore = 0;

        public TextMeshProUGUI speedValueText;
        public TextMeshProUGUI avgValueText;
        public TextMeshProUGUI bestAvgValueText;
        public TextMeshProUGUI scoreValueText;

        public float sessionLength = 60f;
        bool isPaused = false;
        float timeLeft;

        public TextMeshProUGUI timerText;
        public GameObject sessionInfoPanel;
        public TextMeshProUGUI sessionInfoText;

        bool sessionRunning = false;

        public Image damageOverlay;
        public float flashAlpha = 0.6f;
        public float fadeSpeed = 1.5f;
        float currentAlpha = 0f;

        private void Awake()
        {
            Instance = this;
            sensitivitySlider = settingsPanel.transform.GetChild(0).GetComponent<Slider>();
            xSpawnDistanceSlider = settingsPanel.transform.GetChild(1).GetComponent<Slider>();
            ySpawnDistanceSlider = settingsPanel.transform.GetChild(2).GetComponent<Slider>();
            sliderValueText = settingsPanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>();
            xDistanceValueSliderValueText = settingsPanel.transform.GetChild(7).GetComponent<TextMeshProUGUI>();
            yDistanceValueSliderValueText = settingsPanel.transform.GetChild(8).GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            StartSession();

            sensitivitySlider.value = player.mouseSensitivity;
            xSpawnDistanceSlider.value = targetSpawner.spawnRange.x;
            ySpawnDistanceSlider.value = targetSpawner.spawnRange.y;
            sliderValueText.text = player.mouseSensitivity.ToString("0.000");
            xDistanceValueSliderValueText.text = targetSpawner.spawnRange.x.ToString("0.00");
            yDistanceValueSliderValueText.text = targetSpawner.spawnRange.y.ToString("0.00");

            sensitivitySlider.onValueChanged.AddListener(val =>
            {
                player.mouseSensitivity = val;
                sliderValueText.text = val.ToString("0.000"); 
            });

            xSpawnDistanceSlider.onValueChanged.AddListener(val =>
            {
                targetSpawner.spawnRange.x = val;
                xDistanceValueSliderValueText.text = val.ToString("0.00");
            });

            ySpawnDistanceSlider.onValueChanged.AddListener(val =>
            {
                targetSpawner.spawnRange.y = val;
                yDistanceValueSliderValueText.text = val.ToString("0.00");
            });
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleSettingsPanel(true);
                PauseSession();
            }

            UpdateSession();

           

        }

        public void PauseSession()
        {
            isPaused = true;
        }

        public void ResumeSession()
        {
            isPaused = false;
        }

        public void StartSession()
        {
            float best = GetHighScore();
            bestAvgValueText.text = "Best Score : "+best.ToString("0.00");
            scoreValueText.text = "Score : 00";

            timeLeft = sessionLength;
            sessionRunning = true;

            ResetSession();
        }

        void UpdateSession()
        {
            if (!sessionRunning || isPaused)
                return;

            timeLeft -= Time.deltaTime;

            // update timer UI
            timerText.text = FormatTime(timeLeft);

            if (timeLeft <= 0)
            {
                EndSession();
            }
        }

        string FormatTime(float t)
        {
            t = Mathf.Max(0, t);
            int minutes = Mathf.FloorToInt(t / 60f);
            int seconds = Mathf.FloorToInt(t % 60f);
            return minutes.ToString("0") + ":" + seconds.ToString("00");
        }

        void EndSession()
        {
            sessionRunning = false;
            SaveHighScore();
            ToggleInfoPanel(true);

            sessionInfoText.text =
                "Session Over!\n" +
                "Avg Speed: " + averageSpeed.ToString("0.00") + "\n" +
                "Current Score: " + totalScore.ToString("0.00") + "\n" +
                "Best Score: " + GetHighScore().ToString("0.00");

            bestAvgValueText.text = "Best Score : " + GetHighScore().ToString("0.00");
        }

        public void ResetSession()
        {
            totalHits = 0;
            averageSpeed = 0;
            currentSpeed = 0;
            score = 0;
            totalScore = 0;
            lastHitTime = Time.time;
        }


        public void ToggleSettingsPanel(bool active)
        {
            settingsPanel.SetActive(active);
            player.uiPanelActive = active;

            if (!active)
            {
                ResumeSession();
            }
        }

        public void ToggleInfoPanel(bool active)
        {
            sessionInfoPanel.SetActive(active);
            player.uiPanelActive = active;
            if (!active)
            {
                StartSession();
            }
        }

        public void RegisterHit()
        {
            float now = Time.time;

            if (totalHits > 0)
            {
                float delta = now - lastHitTime;

                // Hits per second
                currentSpeed = 1f / delta;
                speedValueText.text = currentSpeed.ToString("0.00");
                // Running average
                averageSpeed = (averageSpeed * (totalHits - 1) + currentSpeed) / totalHits;

                avgValueText.text = averageSpeed.ToString("0.00");
            }

            totalHits++;
            score += 10;
            totalScore = score * averageSpeed;
            scoreValueText.text = "Score : " + totalScore.ToString("F2");

            lastHitTime = now;
        }

        public void RegisterMiss()
        {
            totalHits--;
            score -= 5;
            totalScore = score * averageSpeed;
            scoreValueText.text = "Score : " + totalScore.ToString("F2");
            if (score < 0) score = 0;
            AudioManager.Instance.Play("miss");
            ShowDamage();
        }

        public void ShowDamage()
        {
            if(StartCoroutine(UpdateDamageOverlay()) != null)
                return;

            StartCoroutine(UpdateDamageOverlay());
        }

        IEnumerator UpdateDamageOverlay()
        {
            Color c = damageOverlay.color;
            c.a = flashAlpha;
            damageOverlay.color = c;

            while (c.a > 0f)
            {
                c.a -= Time.deltaTime * fadeSpeed;
                damageOverlay.color = c;
                yield return null; 
            }
        }

        public void SaveHighScore()
        {
            float best = PlayerPrefs.GetFloat("BestSocre", 0f);

            if (totalScore > best)
            {
                PlayerPrefs.SetFloat("BestSocre", totalScore);
                PlayerPrefs.Save();
            }
        }

        public float GetHighScore()
        {
            return PlayerPrefs.GetFloat("BestSocre", 0f);
        }
    }

}
