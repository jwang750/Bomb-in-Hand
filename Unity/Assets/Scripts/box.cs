using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    public int boxnum;
    // Start is called before the first frame update
    void Start()
    {
        boxnum = -1;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "bomb")
        {
            Destroy(this.gameObject);
            string s = this.gameObject.name;
            string num = s.Substring(3);
            boxnum = int.Parse(num);
            //Debug.Log(boxnum);
        }
    }
}
