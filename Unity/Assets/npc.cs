using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc : MonoBehaviour
{
    private float throwForce = 15f;
    public float delay = 0.1f;
    float countdown;
    public GameObject grenadePrefab;
    bool hascol;
    public string[] move;
    public int npcRow;
    public int npcCol;
    private CharacterController cc;
    //血条
    //主摄像机对象
    private Collider co;
    private Camera worldCam = null;
    //NPC名称
    private string playername = "NPC";
    float npcHeight;
    //红色血条贴图
    public Texture2D blood_red;
    //黑色血条贴图
    public Texture2D blood_black;
    private int HP = 100;
    Vector3 throwdir;
    Vector3 pos;
    private int step = 1;
    Vector3 g = new Vector3(0, -9.8f, 0);
    void Start()
    {
        cc = GetComponent<CharacterController>();
        hascol = false;
        GameObject worldCam1 = GameObject.Find("MainCamera");
        worldCam = worldCam1.GetComponent<Camera>();
        co = GetComponent<Collider>();
        //得到模型原始高度
        float size_y = co.bounds.size.y;
        npcHeight = size_y;
        countdown = delay;
        move = null;
        pos = transform.localPosition;
        npcRow = (int)pos.x - 3;
        npcCol = (int)pos.z;

    }

    // Update is called once per frame
    void Update()
    {
        if (countdown < 0 && move != null)
        {
            if (step == move.Length)
            {
                step = 1;
                move = null;
            }
            if (step < move.Length)
            {
                if (move[step].Equals("UP"))
                {
                    pos.x = pos.x - 1;
                    npcRow -= 1;
                }
                else if (move[step].Equals("DOWN"))
                {
                    pos.x = pos.x + 1;
                    npcRow += 1;
                }
                else if (move[step].Equals("LEFT"))
                {
                    pos.z = pos.z - 1;
                    npcCol -= 1;
                }
                else if (move[step].Equals("RIGHT"))
                {
                    pos.z = pos.z + 1;
                    npcCol += 1;
                }
                step = step + 1;
            }
            countdown = delay;
            transform.localPosition = pos;
        }
        countdown -= Time.deltaTime;
        
        //cc.Move(g* Time.deltaTime);
     
        if (hascol == true)
        {
            HP = HP - 20;
            hascol = false;
        }

        if(transform.localPosition.y<-5f || HP==0)
		{

			FindObjectOfType<GameManager>().EndGame();
		}

    }
    void ThrowGrenade()
    {
        Vector3 p = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z - 1 );
        GameObject grenade = GameObject.Instantiate(grenadePrefab, p, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        throwdir = transform.forward;
        throwdir.y = throwdir.y + 0.7f;
        rb.AddForce(throwdir * throwForce, ForceMode.VelocityChange);
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
        //注解2
        //计算出血条的宽高
        Vector2 bloodSize = GUI.skin.label.CalcSize(new GUIContent(blood_red));

        //通过血值计算红色血条显示区域
        int blood_width = 60 * HP / 100;
        //先绘制黑色血条
        GUI.DrawTexture(new Rect(position.x - 30, position.y-100, 60, 10), blood_black);
        //在绘制红色血条
        if (HP > 0)
        {
            GUI.DrawTexture(new Rect(position.x - 30, position.y-100, blood_width, 10), blood_red);
        }

        //注解3
        //计算NPC名称的宽高
        Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(playername));
        //设置显示颜色为黄色
        GUI.color = Color.black;
        //绘制NPC名称
        GUI.Label(new Rect(position.x - 30, position.y - nameSize.y-100, nameSize.x, nameSize.y), playername);

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("collision!");
        if (collision.gameObject.tag == "bomb")
        {
            HP = HP - 20;
        }
    }
}

