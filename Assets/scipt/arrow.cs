using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class arrow : MonoBehaviour
{
    public Animator animator;
    public unit unit;
    void Start()
    {
        unit = (unit)transform.Find("twoD").GetComponent("unit");
        animator = transform.Find("twoD").GetComponent<Animator>();
    }
    
    void Update()
    {
        
        if (unit.stiffTime<=0&&(!unit.conversely)&&(!unit.conversely)&&unit.alive&&(!unit.exiled)/*Input.GetMouseButton(0)*/)
        {
            //获取鼠标的坐标，鼠标是屏幕坐标，Z轴为0，这里不做转换
            Vector3 mouse = Input.mousePosition;
           // Debug.Log("mouse is" + mouse);
            mouse = Camera.main.ScreenToWorldPoint(mouse);
            //Debug.Log("after change mouse is"+mouse);
            //获取物体坐标，物体坐标是世界坐标，将其转换成屏幕坐标，和鼠标一直
            Vector3 obj =transform.position;
            //Debug.Log("obj is" + obj);
            //Debug.Log("objv:"+o+bj);
            //屏幕坐标向量相减，得到指向鼠标点的目标向量，即黄色线段
            Vector3 direction = mouse - obj;
           // Debug.Log("direction  is" + direction);
            //将Z轴置0,保持在2D平面内

            //将目标向量长度变成1，即单位向量，这里的目的是只使用向量的方向，不需要长度，所以变成1
            


            
            direction.y = 0f;
            direction = direction.normalized;
            //Debug.Log("forward is"+ transform.forward);

            //当目标向量的Y轴大于等于0.4F时候，这里是用于限制角度，可以自己条件
            transform.forward = -direction;
            //transform.forward = direction;//by sasalicat
            //transform.rotation= Quaternion.LookRotation
            /*if (direction.y >= 0.4f)
            {
                //物体自身的Y轴和目标向量保持一直，这个过程XY轴都会变化数值
                transform.up = direction;
            }*/
        }
        
        /*if (Input.GetKeyDown(KeyCode.W)||Input.GetKey(KeyCode.W))
        {
            forward.y += 1*Time.deltaTime*speed;
            
            
        }
        
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D))
        {
            forward.x += 1 * Time.deltaTime * speed;

        }
       
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
        {
            forward.x -= 1 * Time.deltaTime * speed;

        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S))
        {
            forward.y -= 1*Time.deltaTime*speed;
            
        }
        
         Quaternion q1 = transform.rotation;
         Quaternion q2 = Quaternion.Euler(new Vector3(0, 1, 0));
        double xita = Quaternion.Angle(q1, q2);
        Debug.Log("xita is" + xita);*/
        //double tanV = speed.x / speed.y;
        //Debug.Log("tanV is" + tanV);
        //double arfa = (Math.Atan(tanV) / (Math.PI / 180));
        // Debug.Log("arfa is" + arfa);
        //double angle_diff = arfa - xita;
        //Debug.Log("before" + speed);
        //Debug.Log("360-angle"+(360-angle)+"x"+ (float)Math.Sin(360.0 - angle) * speed.x+"y"+ -(float)Math.Cos(360.0 - angle) * speed.x);
        //Debug.Log(speed);
        /*speed.Normalize();
        
        Debug.Log("angle diff is"+angle_diff );
        float xVal=speed.magnitude * (float)Math.Sin((((double)angle_diff) / 180) * Math.PI);
        float yVal=speed.magnitude * (float)Math.Cos((((double)angle_diff) / 180) * Math.PI);
        speed.x = xVal;//(float)Math.Cos((double)angle_diff);
        speed.y = yVal;//(float)Math.Sin((double)angle_diff);
        Debug.Log("speed x is"+xVal+"speed y is"+yVal);
        Debug.Log("Cos(x) is" + (float)Math.Cos((((double)angle_diff)/180)*Math.PI) + "Sin(y) is" + (float)Math.Sin(((((double)angle_diff))/180)*Math.PI));
        Debug.Log("after change speed is" + speed);
        transform.Translate(speed);
        speed.x = 0;
        speed.y = 0;*/
        
        
    }
    
}
