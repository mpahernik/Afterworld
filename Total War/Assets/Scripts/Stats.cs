using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public int movement;
    public int Maximummovement;
    public int damage;
    public int currenthealth;
    public int health;
    public int range;
    public int armor;
    GameObject textObject;
    public Transform mouseovered;
    float mouseovertimer = 0;


    public enum  UnitType {Worker, Building, Spearman, Swordman, Horseman, Archer, Mining };
    public UnitType unittype;
    [HideInInspector]public UnitType Worker = UnitType.Worker;
    [HideInInspector] public UnitType Mining = UnitType.Mining;

    // Start is called before the first frame update
    void Start()
    {

        currenthealth = health;
        movement = Maximummovement;
    }
    // Update is called once per frame
    void Update()
    {
        textObject = this.transform.GetChild(3).gameObject;
        textObject.GetComponent<TextMesh>().text = currenthealth.ToString();
    if(currenthealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnMouseOver()
    {
        mouseovertimer += Time.deltaTime;
        if (mouseovertimer > 1)
        {
           // Debug.Log("radi ili ne");
        }
    }
    private void OnMouseExit()
    {
        mouseovertimer = 0;
    }


}
