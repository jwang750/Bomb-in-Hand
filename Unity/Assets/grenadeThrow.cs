using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.Serialization;

public class grenadeThrow : MonoBehaviour
{
    // Start is called before the first frame update
    private float throwForce;
    public GameObject grenadePrefab;
    //public BombController Bomb;
    private float bloodValue;
    private float tmpValue;
    private int go;
    private int accumate;
    private float dTime = 0;
    public float delay = 1f;
    public float accumulate_velocity;
    float countdown;
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("nurse");
        ConnectToTcpServer();
        tmpValue = 0.0f;
        bloodValue = 0.0f;
        go = 0;
        countdown = delay;
        throwForce = 0.5f;
    }

    private void OnGUI()
    {
        //点击加血
        if (accumate == 1)
        {
            tmpValue = 1.0f;
            bloodValue = Mathf.Lerp(bloodValue, tmpValue, 0.001f);
        }

        //点击减血
        if (go == 1)
        {
            tmpValue = 0.0f;
            bloodValue = Mathf.Lerp(bloodValue, tmpValue, 1f);
        }


        GUI.color = Color.red; //血条，设置为红色
        GUI.HorizontalScrollbar(new Rect(40, 40, 400, 40), 0.0f, bloodValue, 0.0f, 1.0f, GUI.skin.GetStyle("HorizontalScrollbar"));
    }



    // Update is called once per frame
    void Update()
    {
        //go = 1;
        countdown -= Time.deltaTime;
        if (accumate == 1)
        {
            throwForce = throwForce + accumulate_velocity;
        }

        if (go == 1)
        {
            if (countdown <= 0f)
            {
                countdown = delay;
                ThrowGrenade();
                throwForce = 0.5f;
                go = 0;
            }
  
        }

    }

    void ThrowGrenade()
    {
        Vector3 p = new Vector3(player.transform.position.x - 1f, player.transform.position.y + 1f, player.transform.position.z);
        GameObject grenade = GameObject.Instantiate(grenadePrefab, p, player.transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        //Debug.Log(transform.forward);
        p = transform.forward;
        p = new Vector3((float)-1.0, (float)0.6, (float)0.0);
        Debug.Log(throwForce);
        rb.AddForce(p * throwForce, ForceMode.VelocityChange);
    }

    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
            //Logger.Log("server start!");

        }
        catch (Exception e)
        {
            print(e);
            // Logger.Log("On client connect exception " + e);
        }

    }
    private void ListenForData()
    {

        try
        {
            socketConnection = new TcpClient("localhost", 7777);
            NetworkStream stream = socketConnection.GetStream();

            Byte[] bytes = new Byte[1024];

            while (true)
            {
                // Get a stream object for reading 				
                int length;
                // Read incomming stream into byte arrary. 	

                while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    var incommingData = new byte[length];
                    Array.Copy(bytes, 0, incommingData, 0, length);
                    // Convert byte array to string message. 						
                    string serverMessage = Encoding.ASCII.GetString(incommingData);
                    //Logger.Log("server message received as: " + serverMessage);
                    // process data

                    if (serverMessage.StartsWith("action"))
                    {
                        string[] info = serverMessage.Split(' ');

                        if (info[1].Equals("accumulate"))
                        {
                            //Debug.Log("accumulate");
                            accumate = 1;
                            go = 0;
                        }
                        else if (info[1].Equals("throw"))
                        {
                            //Debug.Log("throw");
                            go = 1;
                            accumate = 0;
                        }
                        else if (info[1].Equals("no"))
                        {
                            //Debug.Log("no");
                            accumate = 0;
                            go = 0;
                        }

                    }
                    if (!serverMessage.StartsWith("action"))
                    {
                        Debug.Log("error");
                    }

                }
            }

        }
        catch (Exception e)
        {
            print(e);
            //Logger.Log("exception: " + e);
        }

    }
}
