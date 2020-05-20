using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour
{
    private Vector2 beginPosition = new Vector2();
    [Tooltip("the max distance to decide drag direction")]
    public float disJugdement;//距离判断阀值
    // Start is called before the first frame update
    void Start()
    {
        countModel.instance.randomIni();
        countModel.instance.randomIni();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MDrag()
    {
        beginPosition = Input.mousePosition;       
    }
    
    public void MDragEnd()
    {
        Vector2 DValue = new Vector2();
        DValue.x = Input.mousePosition.x - beginPosition.x;
        DValue.y = Input.mousePosition.y - beginPosition.y;
        getMoveDirection(DValue);
    }


    //判断是向哪个方向拖拽的
    public void getMoveDirection(Vector2 DValue)
    {
        if (DValue.x >= disJugdement && Math.Abs(DValue.y) < disJugdement)
        {
            //----------toRight
            countModel.instance.moveRight();
            if (view.instance.compareTZFZGrid(2))
            {
                countModel.instance.randomIni();
            }
        }
        if(DValue.x <=- disJugdement && Math.Abs(DValue.y) < disJugdement)
        {
            //----------toLeft
            countModel.instance.moveLeft();
            if (view.instance.compareTZFZGrid(1))
            {
                countModel.instance.randomIni();
            }
        }
        if (DValue.y <= -disJugdement && Math.Abs(DValue.x) < disJugdement)
        {
            //----------toDown
            countModel.instance.moveDown();
            if (view.instance.compareTZFZGrid(4))
            {
                countModel.instance.randomIni();
            }
        }
        if (DValue.y >= disJugdement && Math.Abs(DValue.x) < disJugdement)
        {
            //----------toUp
            countModel.instance.moveUp();
            if (view.instance.compareTZFZGrid(3))
            {
                countModel.instance.randomIni();
            }
        }
    }

    public void rePlay()
    {
        view.instance.tzfeGridBefor = new int[4,4];
        countModel.instance.tzfeGrid = new int[4, 4];
        view.instance.hideAllBlock();
        view.instance.source.text = "0";
        countModel.instance.source = 0;
        countModel.instance.bestSource = PlayerPrefs.GetInt("bestSource");
        countModel.instance.randomIni();
        countModel.instance.randomIni();
    }
}
