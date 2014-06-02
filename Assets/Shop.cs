using UnityEngine;
using System;
using System.Collections;

public class Shop : MonoBehaviour {

    private Main main;

    public void Init(Main main) {
        this.main = main;
        main.SetTouchEnabled(false);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {
        GUI.depth = 0;
        int i = 1;
        GUI.skin.button.alignment = TextAnchor.MiddleRight;
        GUI.Label(new Rect(Screen.width / 10, Screen.height / 10, Screen.width * 4 / 5, Screen.height * 7 / 10), "BUY CRAP HERE");
        AddButton(i++, main.hamsterTex, "hamster, cost: 500", main.BuyHamster);

        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
        if (GUI.Button(new Rect(Screen.width / 10, Screen.height * 4 / 5, Screen.width * 4 / 5, Screen.height / 10), "Stop buying crap")) {
            Destroy(this);
            main.SetTouchEnabled(true);
        }
    }

    void AddButton(int i, Texture icon, string name, Action action) {
        if (GUI.Button(new Rect(Screen.width / 10, Screen.height * i / 10, Screen.width * 4 / 5, Screen.height / 10), name))
            action();

        GUI.DrawTexture(new Rect(Screen.width / 10, Screen.height * i / 10, 
            Screen.height / 10 * icon.width / icon.height, Screen.height / 10), icon);
    }
}
