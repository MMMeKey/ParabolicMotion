using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	//正在下落过程中
	//当炮弹超出边界之后，回收炮弹
	void Update()
	{
		if (this.transform.position.y<=-10||this.transform.position.x>=8){
			this.gameObject.SetActive(false);
			Destroy(this.gameObject,1);
			GameManager.Instance.currentBulletObj=null;
			GameManager.Instance.isFalling=false;
		}
	}
}
