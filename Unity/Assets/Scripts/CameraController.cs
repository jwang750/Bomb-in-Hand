using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class CameraController : MonoBehaviour
{
    private float delay = 4f;
    float countdown;
    public Transform player;
    public npc pirate;
    public PlayerController nurse;
    public box box1;
    public box box2;
    public box box3;
    public box box4;
    public box box5;
    public box box6;
    public box box7;
    public box box8;
    public box box9;
    public box box10;
    public box box11;
    public box box12;
    public box box13;
    public box box14;
    public box box15;
    public box box16;
    public box box17;
    public box box18;
    public box box19;
    public box box20;
    private int holeLoc1;
    private int holeLoc2;

    private float mouseX, mouseY;
    private int received;

    public float mouseSensituvity;

    public float xRotation;
    private void Start()
    {
        holeLoc1 = -1;
        holeLoc2 = -1;
        ConnectToTcpServer();
        //info = null;
        received = 0;
        countdown = delay;

    }

    private void Update()
    {
        if(received != 0)
        {
            if (pirate.move==null && countdown < 0)
            {
                SendMessage();
                received = 0;
            }
        }

        checkbox();
        Debug.Log(holeLoc1 +" and "+ holeLoc2);

        mouseX = Input.GetAxis("Mouse X") * mouseSensituvity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensituvity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);

        //player.Rotate(Vector3.up * mouseX);
        //transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        countdown -= Time.deltaTime;
    }
    private void checkbox()
    {
        if(box1.boxnum != -1)
        { 
            if(holeLoc1 == -1) {
                holeLoc1 = box1.boxnum;
                box1.boxnum = -1;
            }
            else if(holeLoc2 == -1)
            {
                holeLoc2 = box1.boxnum;
                box1.boxnum = -1;
            }

        }
        if (box2.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box2.boxnum;
                box2.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box2.boxnum;
                box2.boxnum = -1;
            }
        }
        if (box3.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box3.boxnum;
                box3.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box3.boxnum;
                box3.boxnum = -1;
            }
        }
        if (box4.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box4.boxnum;
                box4.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box4.boxnum;
                box4.boxnum = -1;
            }
        }
        if (box5.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box5.boxnum;
                box5.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box5.boxnum;
                box5.boxnum = -1;
            }
        }
        if (box6.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box6.boxnum;
                box6.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box6.boxnum;
                box6.boxnum = -1;
            }
        }
        if (box7.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box7.boxnum;
                box7.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box7.boxnum;
                box7.boxnum = -1;
            }
        }
        if (box8.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box8.boxnum;
                box8.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box8.boxnum;
                box8.boxnum = -1;
            }
        }
        if (box9.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box9.boxnum;
                box9.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box9.boxnum;
                box9.boxnum = -1;
            }
        }
        if (box10.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box10.boxnum;
                box10.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box10.boxnum;
                box10.boxnum = -1;
            }
        }
        if (box11.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box11.boxnum;
                box11.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box11.boxnum;
                box11.boxnum = -1;
            }
        }
        if (box12.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box12.boxnum;
                box12.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box12.boxnum;
                box12.boxnum = -1;
            }
        }
        if (box13.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box13.boxnum;
                box13.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box13.boxnum;
                box13.boxnum = -1;
            }
        }
        if (box14.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box14.boxnum;
                box14.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box14.boxnum;
                box14.boxnum = -1;
            }
        }
        if (box15.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box15.boxnum;
                box15.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box15.boxnum;
                box15.boxnum = -1;
            }
        }
        if (box16.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box16.boxnum;
                box16.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box16.boxnum;
                box16.boxnum = -1;
            }
        }
        if (box17.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box17.boxnum;
                box17.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box17.boxnum;
                box17.boxnum = -1;
            }
        }
        if (box18.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box18.boxnum;
                box18.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box18.boxnum;
                box18.boxnum = -1;
            }
        }
        if (box19.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box19.boxnum;
                box19.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box19.boxnum;
                box19.boxnum = -1;
            }
        }
        if (box20.boxnum != -1)
        {
            if (holeLoc1 == -1)
            {
                holeLoc1 = box20.boxnum;
                box20.boxnum = -1;
            }
            else if (holeLoc2 == -1)
            {
                holeLoc2 = box20.boxnum;
                box20.boxnum = -1;
            }
        }
    }
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    private void ConnectToTcpServer()
    {
        Debug.Log("8888");
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();

        }
        catch (Exception e)
        {
            print(e);
        }

    }
    private void ListenForData()
    {

        try
        {
            socketConnection = new TcpClient("localhost", 8888);
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

                    if (serverMessage.StartsWith("action"))
                    {
                        string[] info = serverMessage.Split(' ');
                        countdown = delay;
                        pirate.move = info;
                        received = 1;
                 
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
    public void SendMessage()//简单的发送数据
    {
        if (socketConnection == null)
        {
            return;
        }
        try
        {
            NetworkStream stream = socketConnection.GetStream();

            if (stream.CanWrite)
            {

                string clientMessage = nurse.playerCol.ToString() + " " + pirate.npcCol.ToString() + " " + pirate.npcRow.ToString() + " " + holeLoc1.ToString() + " "+ holeLoc2.ToString();

                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);

                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                holeLoc1 = -1;
                holeLoc2 = -1;

            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}
