using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public ParabolaController parabolaController;
	public Transform cannonTrans;
	public GameObject bulletPrefab;
	[HideInInspector]
	public GameObject currentBulletObj;
	public bool isPressed = false;
	public float firePower = 20;
	public bool isFalling = false;

	private static GameManager _Instance;
	public static GameManager Instance {
		get { return _Instance; }
	}
	void Awake () {
		_Instance = this;
	}
	void Start () {
		currentBulletObj = CreateBullet (cannonTrans.position);
	}
	/// <summary>
	/// 创建炮弹
	/// </summary>
	/// <param name="pos">指定目标位置</param>
	/// <returns></returns>
	private GameObject CreateBullet (Vector3 pos) {
		GameObject bulletObj = Instantiate (bulletPrefab);
		bulletObj.transform.position = pos;
		bulletObj.SetActive (false);
		currentBulletObj = bulletObj;
		return bulletObj;
	}
	/// <summary>
	/// 发射炮弹
	/// </summary>
	/// <param name="force">指定给炮弹的刚体组件添加多大的力</param>
	private void FireBullet (Vector3 force) {
		if (currentBulletObj == null) return;
		isFalling = true;
		currentBulletObj.SetActive (true);
		currentBulletObj.GetComponent<Rigidbody> ().useGravity = true;
		currentBulletObj.GetComponent<Rigidbody> ().AddForce (force, ForceMode.Impulse);
	}
	/// <summary>
	/// 计算发射时，鼠标按下位置到炮弹初始位置的向量（方向）
	/// </summary>
	/// <param name="fromPos">炮弹的位置</param>
	/// <param name="toPos">鼠标按下时的位置</param>
	/// <returns></returns>
	private Vector2 CalculationFireForce (Vector3 fromPos, Vector3 toPos) {
		return (new Vector2 (toPos.x, toPos.y) - new Vector2 (fromPos.x, fromPos.y)) * firePower;
	}
	void Update () {
		if (isFalling) return;
		if (Input.GetMouseButtonDown (0)) {
			isPressed = true;
			if (currentBulletObj == null) {
				currentBulletObj = CreateBullet (cannonTrans.position);
			}
		} else if (Input.GetMouseButtonUp (0)) {
			if (currentBulletObj == null) return;
			isPressed = false;
			if (isFalling == false) {
				FireBullet (CalculationFireForce (currentBulletObj.transform.position,
					Camera.main.ScreenToWorldPoint (Input.mousePosition)));
			}
		}
		if (isPressed) {
			Vector3 direction = CalculationFireForce (currentBulletObj.transform.position,
				Camera.main.ScreenToWorldPoint (Input.mousePosition));
			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
			cannonTrans.eulerAngles = new Vector3 (0, 0, angle);
			parabolaController.SetTrajectPointPos (cannonTrans.position, direction / currentBulletObj.GetComponent<Rigidbody> ().mass);
		}
	}
}