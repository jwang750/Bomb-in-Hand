using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private CharacterController cc;
	private Collider co;
	private float delay = 1f;
	//public float moveSpeed;
	//public float jumpSpeed;
	//public float horizontalMove, verticalMove;
	public int playerCol;
	private Vector3 dir;

	//血条
	//主摄像机对象
	private Camera worldCam = null;
	//NPC名称
	private string playername = "Player";
	float npcHeight;
	//红色血条贴图
	public Texture2D blood_red;
	//黑色血条贴图
	public Texture2D blood_black;
	private int HP;
	Vector3 pos;

	private void Start()
	{
		cc = GetComponent<CharacterController>();
		//----------血条-----------
		GameObject worldCam1 = GameObject.Find("MainCamera");
		worldCam = worldCam1.GetComponent<Camera>();

		co = GetComponent<Collider>();
		//得到模型原始高度
		float size_y = co.bounds.size.y;
		npcHeight = size_y;
		HP = 100;
		pos = transform.localPosition;
		playerCol = 5 - (int)pos.z;
		//Debug.Log(pos);
	}

	private void Update()
	{
		//horizontalMove = Input.GetAxis("Horizontal") * moveSpeed;
		//verticalMove = Input.GetAxis("Vertical") * moveSpeed;

		//dir = transform.forward * verticalMove + transform.right * horizontalMove;
		//cc.Move(dir * Time.deltaTime);
		if (Input.GetKey(KeyCode.W))
        {
			//dir = transform.forward * verticalMove;
			if (pos.x > 4 && delay > 0)
			{
				pos.x = pos.x - 1;
				delay = -1;
			}
			//Debug.Log(pos);
		}
		if (Input.GetKey(KeyCode.S))
		{
			//dir = transform.forward* verticalMove;
			//cc.Move(dir * 1);
			if (pos.x < 8 && delay > 0)
			{
				pos.x = pos.x + 1;
				delay = -1;
			}
			//Debug.Log(pos);
		}
		if (Input.GetKey(KeyCode.A))
		{
			//dir = transform.right * horizontalMove;
			//cc.Move(dir * 1);
			if (pos.z > 1 && delay > 0)
			{
				pos.z = pos.z - 1;
				delay = -1;
				playerCol = playerCol + 1;
			}
			//Debug.Log(pos);
		}
		if (Input.GetKey(KeyCode.D))
		{
			//dir = transform.right * horizontalMove;
			//cc.Move(dir * 1);
			if (pos.z < 4 && delay > 0)
			{
				pos.z = pos.z + 1;
				delay = -1;
				playerCol = playerCol - 1;
			}
			//Debug.Log(pos);
		}
		transform.localPosition = pos;
		delay = delay - Time.deltaTime;
		if(delay < -1.5)
        {
			delay = 1;
        }
	}
	void OnGUI()
	{
		//得到NPC头顶在3D世界中的坐标
		//默认NPC坐标点在脚底下，所以这里加上npcHeight它模型的高度即可
		Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + npcHeight, transform.position.z);
		//根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
		Vector2 position = worldCam.WorldToScreenPoint(worldPosition);
		//得到真实NPC头顶的2D坐标
		position = new Vector2(position.x, Screen.height - position.y);
		//计算出血条的宽高
		Vector2 bloodSize = GUI.skin.label.CalcSize(new GUIContent(blood_red));
		//通过血值计算红色血条显示区域
		int blood_width = 120 * HP / 100;
		//先绘制黑色血条
		//GUI.DrawTexture(new Rect(position.x, position.y, 120, 20), blood_black);
		//if (HP > 0)
		//{
			//在绘制红色血条
		//	GUI.DrawTexture(new Rect(position.x, position.y, blood_width, 20), blood_red);
		//}
		//计算NPC名称的宽高
		Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(playername));
		//设置显示颜色为黄色
		//GUI.color = Color.black;
		//绘制NPC名称
		GUI.Label(new Rect(position.x, position.y - nameSize.y, nameSize.x, nameSize.y), playername);

	}
    private void OnCollisionEnter(Collision collision)
    {
		//Debug.Log("collision!");
        if(collision.gameObject.tag == "bomb")
        {
			HP = HP - 20;
        }
    }
}
