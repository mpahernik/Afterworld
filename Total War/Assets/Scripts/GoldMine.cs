using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMine : MonoBehaviour
{



    public GameObject[] Miners = new GameObject[5];
    public int MaxGold;
    public int CurrentGold;
    public GameObject Gameman;
    int i = 0;
    private void Start()
    {
        CurrentGold = MaxGold;
    }


    private void Update()
    {


        foreach (GameObject miner in Miners)
        {

            if (miner != null)
            {
                if (Vector2.Distance(this.transform.position, miner.transform.position) > 1)
                {

                    Miners[i].transform.gameObject.GetComponent<Stats>().unittype = Miners[i].transform.gameObject.GetComponent<Stats>().Worker;
                    Miners[i] = null;

                }
            }
            i++;
        }
        i = 0;
            
       



        if (Gameman.gameObject.GetComponent<Manager>().resourceturn )
        {
            foreach (GameObject miner in Miners)
            {
                
                if (miner != null)
                {
                    if (Vector2.Distance(this.transform.position, miner.transform.position) <= 1)
                    {
                        CurrentGold -= 10;
                    }
                    else
                    {
                        Miners[i].transform.gameObject.GetComponent<Stats>().unittype = Miners[i].transform.gameObject.GetComponent<Stats>().Worker;
                        Miners[i] = null;

                    }
                }
                i++;
            }
            i = 0;
        }
        Gameman.gameObject.GetComponent<Manager>().resourceturn = false;
    }
}
