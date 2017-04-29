using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlantAndDefuse : MonoBehaviour {

	public Text timer;
	public Text defuseTimer;
	public Text title;
	public Text warningText;
	public Text bombTimer;
	public Text defuseText;
	public Text bombInputText;
	public Text defuseInputText;
	public Button plantDefuse;
	public Button startGame;
	public Button reset;
	public Image c4;
	public Slider slider;
	public GameObject bombInput;
	public GameObject defuseInput;
	public AudioSource bombAudio;
	public AudioClip bombPlanted;
	public AudioClip bombDefused;
	public AudioClip bombExploded;
	public AudioClip countdown;
	public AudioClip countdownFinish;

	AudioSource audioSource;
	float bombInputTime;
	float bomb;
	float defusal;
	float originalDefusal;
	float bombMinute;
	bool defuse;
	bool startDefuse;
	bool defused;
	bool once;
	bool gameStarted;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		reset.gameObject.SetActive (false);
		c4.enabled = false;
		timer.enabled = false;
		warningText.enabled = false;
		defuseTimer.enabled = false;
		slider.gameObject.SetActive (false);
		plantDefuse.gameObject.SetActive (false);
		defuse = false;
		defused = false;
		startDefuse = false;
		slider.value = 1.0f;
		once = true;
		gameStarted = false;
		timer.text = "00:00";
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	// Update is called once per frame
	void Update () {
		if (startDefuse && defuse) {
			defuseTimer.text = (defusal -= 1.0f * Time.deltaTime).ToString ();
			slider.value = defusal  / originalDefusal;
		}
		if (defusal <= 0.0f && defuse) {
			startDefuse = false;
			defusal = 0.0f;
			defuseTimer.text = defusal.ToString ();
			defused = true;
			defuse = false;
			audioSource.clip = bombDefused;
			audioSource.Play ();
			bombAudio.Stop ();
			print ("defused");
		}
		if (defuse) {
			bomb -= 1.0f * Time.deltaTime;
			timer.text = bombMinute.ToString () + ":" + Mathf.RoundToInt (bomb).ToString ();
			if (bomb <= 9.5f) {
				timer.text = bombMinute.ToString () + ":0" + Mathf.RoundToInt (bomb).ToString ();
			}
		}
		if (bomb <= 0.0f && bombMinute > 0.0f) {
			bomb = 59.0f;
			bombMinute -= 1.0f;
		}
		if (bomb <= 0.0f && bombMinute <= 0.0f && gameStarted) {
			bomb = 0.0f;
			bombMinute = 0.0f;
			timer.text = bombMinute.ToString () + ":" + Mathf.RoundToInt (bomb).ToString () + "0";
			defuse = false;
			audioSource.clip = bombExploded;
			audioSource.Play ();
			bombAudio.Stop ();
			print ("exploded");
			gameStarted = false;
			startDefuse = false;
		}
		if (bomb < 10.0f && bombMinute <= 0.0f && defuse && once) {
			bombAudio.clip = countdownFinish;
			bombAudio.Stop ();
			print ("10 seconds remaining");
			bombAudio.Play ();
			bombAudio.loop = false;
			once = false;
		}
	}

	public void GameStart () {
		if (bombInputText.text != "" && defuseInputText.text != "" && int.Parse (bombInputText.text) > 0 && int.Parse (defuseInputText.text) > 0 && int.Parse (bombInputText.text) <= 6000 && int.Parse (defuseInputText.text) <= 6000) {
			timer.enabled = true;
			c4.enabled = true;
			plantDefuse.gameObject.SetActive (true);
			plantDefuse.GetComponentInChildren<Text> ().text = "Plant the Bomb";
			title.enabled = false;
			warningText.enabled = false;
			bombTimer.enabled = false;
			defuseText.enabled = false;
			startGame.gameObject.SetActive (false);
			defuseInput.SetActive (false);
			bombInput.SetActive (false);
			defuseTimer.text = originalDefusal.ToString ();
			gameStarted = true;
			reset.gameObject.SetActive (true);
		} else {
			warningText.enabled = true;
		}
	}

	public void BombInput () {
		bombInputTime = float.Parse (bombInputText.text);
		bombInputTime = Mathf.Abs (bombInputTime);
		bombInputTime = Mathf.Round (bombInputTime);
		if (bombInputTime >= 60) {
			bombMinute = bombInputTime / 60.0f;
			bombMinute = Mathf.Round (bombMinute) - 1.0f;
			bomb = Mathf.Abs (bombInputTime - (60.0f * bombMinute));
			if (bomb > 60f) {
				bomb -= 60f;
				bombMinute++;
			}
		} else {
			bombMinute = 0.0f;
			bomb = bombInputTime;
		}
		if (bomb == 60f) {
			bomb = 59.0f;
		}
	}

	public void DefuseInput () {
		defusal = Mathf.Abs (float.Parse (defuseInputText.text));
		originalDefusal =  Mathf.Abs (float.Parse (defuseInputText.text));
	}

	public void Planted () {
		if (!defuse && defuseTimer.enabled == false) {
			plantDefuse.GetComponentInChildren<Text> ().text = "Defuse the Bomb";
			defuse = true;
			defuseTimer.enabled = true;
			slider.gameObject.SetActive (true);
			audioSource.clip = bombPlanted;
			audioSource.Play ();
			bombAudio.clip = countdown;
			bombAudio.Play ();
		}
	}

	public void Restart () {
		bombAudio.Stop ();
		print ("restarted");
		audioSource.Stop ();
		c4.enabled = false;
		timer.enabled = false;
		defuseTimer.enabled = false;
		slider.gameObject.SetActive (false);
		plantDefuse.gameObject.SetActive (false);
		reset.gameObject.SetActive (false);
		defuse = false;
		defused = false;
		startDefuse = false;
		slider.value = 1.0f;
		once = true;
		gameStarted = false;
		timer.text = "00:00";
		title.enabled = true;
		bombTimer.enabled = true;
		defuseText.enabled = true;
		startGame.gameObject.SetActive (true);
		defuseInput.SetActive (true);
		bombInput.SetActive (true);
		bombAudio.loop = true;
		defusal = originalDefusal;
		if (bombInputTime >= 60) {
			bombMinute = bombInputTime / 60.0f;
			bombMinute = Mathf.Round (bombMinute) - 1.0f;
			bomb = Mathf.Abs (bombInputTime - (60.0f * bombMinute));
			if (bomb > 60f) {
				bomb -= 60f;
				bombMinute++;
			}
		} else {
			bombMinute = 0.0f;
			bomb = bombInputTime;
		}
		if (bomb == 60f) {
			bomb = 59.0f;
		}
	}

	public void OnPointerDown () {
		if (defuse) {
			startDefuse = true;
		}
	}

	public void OnPointerUp () {
		if (defuse) {
			startDefuse = false;
		}
		if (!defused && defuse) {
			defusal = originalDefusal;
			defuseTimer.text = originalDefusal.ToString ();
			slider.value = originalDefusal;
		}
	}
}
