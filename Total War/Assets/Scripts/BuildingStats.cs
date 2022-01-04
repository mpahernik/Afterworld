using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStats : MonoBehaviour
{
    public int Building_currenthealth;
    public int Building_maxhealth;
    GameObject textObject; // hp napraviti
    public GameObject unittobuild;
    public GameObject Ui;
    public int Level;
    Transform spawnpoint;
    int resourceusing;
    bool Click = false;
    public int Unitcost;
    void Start()
    {
        
        Building_currenthealth = Building_maxhealth;
        Level = 1;
        spawnpoint = this.transform.GetChild(0);
    }
    private void Update()
    {
        if (this.CompareTag("BlackBuildings")) resourceusing = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().Black_resourse;
        if (this.CompareTag("RedBuildings")) resourceusing = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().Red_resourse;

        if (Click && resourceusing >= Unitcost)
        {
            Instantiate(unittobuild, spawnpoint.position, Quaternion.identity);
            if (this.CompareTag("BlackBuildings")) GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().Black_resourse -= Unitcost;
            if (this.CompareTag("RedBuildings")) GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().Red_resourse -= Unitcost;
                       
            Click = false;
        }
        Click = false;
    }

    public void SpawnUnit()
    {
        Click = true;
    }
}
