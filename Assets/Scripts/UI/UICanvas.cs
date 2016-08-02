using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour {
	public static UICanvas self;

	[Header("画布")]
	[SerializeField]
	private bool hideCanvas = true;
	[SerializeField, Range(10, 30)]
	private float fadeTime = 30f;
	private float canvasTime = 30f;
	private UIControl[] allCtrl;
	public UIControl main;
	public UIControl current;
	public Button mask;
	public Canvas canvas;

	[HideInInspector]
	public float turnTime;
	[Header("光标")]
	[SerializeField, Range(0, 1)]
	private float cursorTime = 1f;
	[SerializeField]
	private Texture2D[] cursorTexture;
	
	void Awake () {
		self = this;
	}

	void Start() {
		SetCursor(true);
		canvas = GetComponent<Canvas>();
		main.transform.SetAsFirstSibling();
		mask.transform.SetAsFirstSibling();
		allCtrl = GetComponentsInChildren<UIControl>();
		mask.onClick.AddListener( delegate () {
			HideAllPanel();
		});
	}
	
	void Update () {
		HideCanvas();
		MouseEvent();
	}

	void SetCursor(bool able) {
		Texture2D t2d = null;
		Vector2 vec = Vector2.zero;
		if (cursorTexture == null || cursorTexture.Length == 0) 
			return;
		if (able) {
			int idx = UnityEngine.Random.Range(0, cursorTexture.Length);
			t2d = cursorTexture[idx];
			vec = new Vector2(t2d.width / 2, t2d.height / 2);
		}
		Cursor.SetCursor(t2d, vec, CursorMode.ForceSoftware); 
	}

	void MouseEvent() {
		//停止
		if (turnTime == 1f)
			return;
		//关闭
		if (turnTime == 0f) {
			SetCursor(false);
			return;
		}
		//点击
		if (Input.GetMouseButtonDown (0)) {
			SetCursor(true);
			return;
		}
		//长按
		if (Input.GetMouseButton (0)) {
			cursorTime += Time.deltaTime;
			if (cursorTime >= turnTime) {
				cursorTime -= turnTime;
				SetCursor(true);
			}
		}
	}
	
	void HideCanvas() {
		if (!hideCanvas)
			return;
		if (Input.GetMouseButton (0))
			canvasTime = fadeTime;
		if (canvasTime > 0) {
			canvasTime -= Time.deltaTime;
			if (canvas.enabled) 
				return;
			canvas.enabled = true;
			SetCursor(true);
		} else {
			canvasTime = -1f;
			if (!canvas.enabled) 
				return;
			canvas.enabled = false;
			SetCursor(false);
		}
	}

	void HideAllPanel() {
		for (int i = 0; i < allCtrl.Length; i++) {
			if(allCtrl[i] != main) {
				allCtrl[i].HidePanel();
			}
		}
	}

	public UIControl GetControl(string name) {
		UIControl ctrl = null;
		Transform tran = canvas.transform.Find (name);
		if (tran)
			ctrl = tran.GetComponent<UIControl>();
		return ctrl;
	}
}
