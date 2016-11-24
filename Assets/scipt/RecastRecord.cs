using UnityEngine;
using System.Collections;

public class RecastRecord{
    private bool over = false;//計時是否結束
    private float timeleft=0;//記錄的時間
	public RecastRecord(float time)
    {
        timeleft = time;
    }
    public void reduceTime(float time)//用於減少記錄的時間,time為減少量
    {
        timeleft -= time;
        if (timeleft <= 0)
        {
            over = true;
        }
    }
    public bool isover()//回傳over值
    {
        return over;
    }
}
