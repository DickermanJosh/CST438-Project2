using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool IsPaused;
        public GameObject pauseMenuUI;
        // public GameObject settingsMenuUI;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            InputHandler.Instance.OnPausePressed += HandlePauseEvent;
        }

        private void OnDisable()
        {
            if (InputHandler.Instance != null)
                InputHandler.Instance.OnPausePressed -= HandlePauseEvent;
        }

        private void HandlePauseEvent()
        {
            if (!IsPaused)
            {
                Pause();
                return;
            }
            Resume();
        }

        private void Pause()
        {
            pauseMenuUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            IsPaused = true;
        }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            IsPaused = false;
        }

        /*
         * Restarts the entire level, does not load last checkpoint
         */
        public void RestartLevel()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            IsPaused = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // public void ToggleSettingsMenu()
        // {
        //     settingsMenuUI.SetActive(!settingsMenuUI.activeSelf);
        // }

        public void QuitToMainMenu()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            IsPaused = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Settings_MainMenu");
        } 
    }
}