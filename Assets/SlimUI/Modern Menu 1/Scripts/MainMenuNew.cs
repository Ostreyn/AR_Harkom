using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace SlimUI.ModernMenu{
	public class MainMenuNew : MonoBehaviour {
		Animator CameraObject;
/*
		[Header("Loaded Scene")]
		[Tooltip("The name of the scene in the build settings that will load")]
		public string sceneName = ""; 
*/
		public enum Theme {custom1, custom2, custom3};
		[Header("Theme Settings")]
		public Theme theme;
		int themeIndex;
		public FlexibleUIData themeController;

		[Header("Panels")]
		[Tooltip("The UI Panel parenting all sub menus")]
		public GameObject mainCanvas;
		[Tooltip("The UI Panel that holds the INPUT window tab")]
		public GameObject Panel_Input;
		[Tooltip("The UI Panel that holds the OUTPUT window tab")]
		public GameObject Panel_Output;
		[Tooltip("The UI Panel that holds the PROCESS window tab")]
		public GameObject Panel_Process;
		[Tooltip("The UI Panel that holds the STORAGE window tab")]
		public GameObject Panel_Storage;

		[Header("SFX")]
		[Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
		public AudioSource hoverSound;
		[Tooltip("The GameObject holding the Audio Source component for the AUDIO SLIDER")]
		public AudioSource sliderSound;
		[Tooltip("The GameObject holding the Audio Source component for the SWOOSH SOUND when switching to the Settings Screen")]
		public AudioSource swooshSound;

		// campaign button sub menu
		[Header("Menus")]
		[Tooltip("The Menu for when the MAIN menu buttons")]
		public GameObject mainMenu;
		[Tooltip("THe first list of buttons")]
		public GameObject firstMenu;
		[Tooltip("The Menu for when the PLAY button is clicked")]
		public GameObject playMenu;
		[Tooltip("The Menu for when the EXIT button is clicked")]
		public GameObject exitMenu;
		[Tooltip("Optional 4th Menu")]
		public GameObject extrasMenu;

		// highlights
		[Header("Highlight Effects")]
		[Tooltip("Highlight Input Device for when Input Tab is selected in Settings")]
		public GameObject lineInput;
		[Tooltip("Highlight Image for when CONTROLS Tab is selected in Settings")]
		public GameObject lineOutput;
		[Tooltip("Highlight Image for when MOVEMENT Sub-Tab is selected in KEY BINDINGS")]
		public GameObject lineProcess;
		[Tooltip("Highlight Image for when COMBAT Sub-Tab is selected in KEY BINDINGS")]
		public GameObject lineStorage;

		[Header("LOADING SCREEN")]
		public GameObject loadingMenu;
		public Slider loadBar;
		public TMP_Text finishedLoadingText;
		public bool requireInputForNextScene = false;

		void Start(){
			CameraObject = transform.GetComponent<Animator>();

			playMenu.SetActive(false);
			exitMenu.SetActive(false);
			if(extrasMenu) extrasMenu.SetActive(false);
			firstMenu.SetActive(true);
			mainMenu.SetActive(true);

			SetThemeColors();
		}

		void SetThemeColors(){
			if(theme == Theme.custom1){
				themeController.currentColor = themeController.custom1.graphic1;
				themeController.textColor = themeController.custom1.text1;
				themeIndex = 0;
			}else if(theme == Theme.custom2){
				themeController.currentColor = themeController.custom2.graphic2;
				themeController.textColor = themeController.custom2.text2;
				themeIndex = 1;
			}else if(theme == Theme.custom3){
				themeController.currentColor = themeController.custom3.graphic3;
				themeController.textColor = themeController.custom3.text3;
				themeIndex = 2;
			}
		}

		public void PlayCampaign(){
			exitMenu.SetActive(false);
			if(extrasMenu) extrasMenu.SetActive(false);
			playMenu.SetActive(true);
		}
		
		public void PlayCampaignMobile(){
			exitMenu.SetActive(false);
			if(extrasMenu) extrasMenu.SetActive(false);
			playMenu.SetActive(true);
			mainMenu.SetActive(false);
		}
		
		public void ReturnMenu(){
			playMenu.SetActive(false);
			if(extrasMenu) extrasMenu.SetActive(false);
			exitMenu.SetActive(false);
			mainMenu.SetActive(true);
		}
/*
		public void NewGame(string){
			if(sceneName != ""){
				StartCoroutine(LoadAsynchronously(sceneName));
			}
		}
*/
		public void LoadScene(string scene){
			if(scene != ""){
				StartCoroutine(LoadAsynchronously(scene));
			}
		}

		public void  DisablePlayCampaign(){
			playMenu.SetActive(false);
		}

		public void Position2(){
			DisablePlayCampaign();
			CameraObject.SetFloat("Animate",1);
		}

		public void Position1(){
			CameraObject.SetFloat("Animate",0);
		}

		void DisablePanels(){
			Panel_Input.SetActive(false);
			Panel_Output.SetActive(false);
			Panel_Process.SetActive(false);
			Panel_Storage.SetActive(false);

			lineInput.SetActive(false);
			lineOutput.SetActive(false);
			lineProcess.SetActive(false);
			lineStorage.SetActive(false);
		}

		public void InputPanel(){
			DisablePanels();
			Panel_Input.SetActive(true);
			lineInput.SetActive(true);
		}

		public void OutputPanel(){
			DisablePanels();
			Panel_Output.SetActive(true);
			lineOutput.SetActive(true);
		}

		public void ProcessPanel(){
			DisablePanels();
			Panel_Process.SetActive(true);
			lineProcess.SetActive(true);
		}

		public void StoragePanel(){
			DisablePanels();
			Panel_Storage.SetActive(true);
			lineStorage.SetActive(true);
		}

		public void PlayHover(){
			hoverSound.Play();
		}

		public void PlaySFXHover(){
			sliderSound.Play();
		}

		public void PlaySwoosh(){
			swooshSound.Play();
		}

		// Are You Sure - Quit Panel Pop Up
		public void AreYouSure(){
			exitMenu.SetActive(true);
			if(extrasMenu) extrasMenu.SetActive(false);
			DisablePlayCampaign();
		}

		public void AreYouSureMobile(){
			exitMenu.SetActive(true);
			if(extrasMenu) extrasMenu.SetActive(false);
			mainMenu.SetActive(false);
			DisablePlayCampaign();
		}

		public void ExtrasMenu(){
			playMenu.SetActive(false);
			if(extrasMenu) extrasMenu.SetActive(true);
			exitMenu.SetActive(false);
		}

		public void QuitGame(){
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}

		IEnumerator LoadAsynchronously(string sceneName){ // scene name is just the name of the current scene being loaded
			AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
			operation.allowSceneActivation = false;
			mainCanvas.SetActive(false);
			loadingMenu.SetActive(true);

			while (!operation.isDone){
				float progress = Mathf.Clamp01(operation.progress / .9f);
				loadBar.value = progress;

				if(operation.progress >= 0.9f){
					if(requireInputForNextScene){
						finishedLoadingText.gameObject.SetActive(true);

						if(Input.anyKeyDown){
							operation.allowSceneActivation = true;
						}
					}else{
						operation.allowSceneActivation = true;
					}
				}
				
				yield return null;
			}
		}
	}
}