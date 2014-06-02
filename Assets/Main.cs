using UnityEngine;
using Assets;
using System.Collections.Generic;

public class Main : MonoBehaviour {

    public GameObject hamster;
    public Texture bar;
    public Texture hamsterTex;

    private Battery battery;
    private bool touchEnabled = true;

    private List<Hamster> caught;
    private Hamster runner;

    private int level = 0;

    // Use this for initialization
    void Start() {
        caught = new List<Hamster>();
        battery = new Battery(100);
    }

    private Vector3 WorldPoint(float x, float y) {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));
        pos.z = 0;
        return pos;
    }

    // Update is called once per frame
    void Update() {
        if (battery.IsFilled())
            battery = new Battery((1 << ++level) * 100);

        if (touchEnabled) {
            for (int i = 0; i < Input.touchCount; i++) {
                Touch touch = Input.GetTouch(i);
                if (touch.deltaTime > 0) {
                    float speed = touch.deltaPosition.magnitude / touch.deltaTime;
                    battery.Add(speed / Screen.height);
                }

                if (runner != null && runner.Tapped(touch)) {
                    caught.Add(runner);
                    runner.Remove();
                    runner = null;
                }
            }
        }

        foreach (Hamster h in caught)
            battery.Add(h.GetEnergyOutput());

        battery.ResetRate();
        if (runner == null && battery.GetAverageRate() > 4)
            runner = new Hamster(hamster, WorldPoint(Random.Range(0, Screen.width),
                Random.Range(Screen.height / 16, Screen.height * 6 / 10)));
    }

    public void BuyHamster() {
        if (battery.Sell(500))
            caught.Add(new Hamster());
    }

    void OnGUI() {
        GUI.depth = 1;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.skin.label.fontSize = 48;
        GUI.skin.button.fontSize = 36;

        drawBar(0, battery.GetAmountFilled() / battery.GetCapacity());
        GUI.Label(new Rect(Screen.width / 15, 0, Screen.width * 13 / 15, Screen.height / 10), 
            string.Format("{0}/{1}", (int)battery.GetAmountFilled(), battery.GetCapacity()));

        float rateY = Screen.height * 3 / 20;
        drawBar(rateY, battery.GetAverageRate() / 5);
        GUI.Label(new Rect(Screen.width / 15, rateY, Screen.width * 13 / 15, Screen.height / 10),
            string.Format("{0:F3}", battery.GetAverageRate()));
        
        float hamWidth = Screen.width / Mathf.Max(10, caught.Count);
        float hamHeight = hamWidth * hamsterTex.height / hamsterTex.width;
        float top = Screen.height * 3 / 10f;
        for (int i = 0; i < caught.Count; i++)
            GUI.DrawTexture(new Rect(i * hamWidth, top, hamWidth, hamHeight), hamsterTex);

        if (!touchEnabled)
            return;

        if (GUI.Button(new Rect(0, Screen.height * 15 / 16f, Screen.width / 2, Screen.height / 16f), "Playpen")) {
            caught.Clear();
            level = 0;
            battery = new Battery(100);
            if (runner != null) {
                runner.Remove();
                runner = null;
            }
        }

        if (GUI.Button(new Rect(Screen.width / 2, Screen.height * 15 / 16f, Screen.width / 2, Screen.height / 16f), "Shop")) {
            gameObject.AddComponent<Shop>().Init(this);
        }
    }

    public void SetTouchEnabled(bool enabled) {
        touchEnabled = enabled;
    }

    void drawBar(float y, float amount) {
        amount = Mathf.Min(1, amount);
        float wUnit = Screen.width / 15;
        GUI.DrawTexture(new Rect(wUnit, y, wUnit * 13 * amount, Screen.height / 10), bar);
    }
}
