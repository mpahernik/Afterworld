using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public enum GameState { PlayerOneDraft, PlayerTwoDraft, PlayerOneTurn, PlayerTwoTurn, GameOver };
public class Manager : MonoBehaviour
{
    public float turntimer = 30;
    public Tilemap tilemap;
    int friendlyunits;
    int enemyunits;
    int gold;
    private Transform _selected = null;
    private Transform background;
    Vector3 mouseclick;
    int unitdamage;
    private Stats enemystatsref;
    private Stats friendlystats;
    private float distance = 0f;
    public GameState curstate;
    public GameObject[] blackarmy = null;
    public GameObject[] redarmy = null;
    public GameObject[] blackbuildings = null;
    public GameObject[] redbuildings = null;
    public Text textturntimer;
    public GameObject Damagepop;
    public GameObject movementpop;
    TextMesh damagetext;
    TextMesh movementtext;
    public bool endtrunclick = false;
    public Text gameover;
    public Text Resourcetext;
    public Walkable walkabl;
    public bool turnchangerr = false;
    public int Black_resourse;
    public int Red_resourse;
    public int Black_income = 0;
    public int Red_income = 0;
    public GameObject[] Mines = null;
    int blackmineersworking= 0;
    int redminersworking = 0;
    public bool resourceturn = false;

    // Start is called before the first frame update
    private void Start()
    {
        Black_resourse = 100;
        Red_resourse = 100;
        curstate = GameState.PlayerOneDraft;
        friendlyunits = LayerMask.GetMask("Black");
        enemyunits = LayerMask.GetMask("Red");
        gold = LayerMask.GetMask("Gold");
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellCoords = tilemap.WorldToCell(mousePos);
        Vector3 cursorPosition = tilemap.GetCellCenterWorld(cellCoords);
        blackarmy = GameObject.FindGameObjectsWithTag("Black");
        redarmy = GameObject.FindGameObjectsWithTag("Red");
        blackbuildings = GameObject.FindGameObjectsWithTag("BlackBuildings");
        redbuildings = GameObject.FindGameObjectsWithTag("RedBuildings");
        bool groundclick = walkabl.walkable;
        if (blackbuildings.Length == 0 || redbuildings.Length == 0)
        {
            if(blackbuildings.Length > redbuildings.Length)
            {
                gameover.text = "BLACK WON";
                gameover.gameObject.SetActive(true);
            }
            else
            {
                gameover.text = "RED WON";
                gameover.color = new Color(112, 0, 0, 255);
                gameover.gameObject.SetActive(true);

            }
            curstate = GameState.GameOver;
        }
        turntimer -= Time.deltaTime;
        int turntime = (int)turntimer;
        textturntimer.text = turntime.ToString();
        if(turntimer <= 0 || endtrunclick == true)
        {
            resourceturn = true;
            foreach (GameObject unit in redarmy)
            {
                if(unit != null)
                unit.GetComponent<Stats>().movement = unit.GetComponent<Stats>().Maximummovement;
                if (unit.gameObject.GetComponent<Stats>().unittype == unit.gameObject.GetComponent<Stats>().Mining)
                {
                    redminersworking++;
                    
                }
            }
            foreach (GameObject unit in blackarmy)
            {
                if(unit != null)
                unit.GetComponent<Stats>().movement = unit.GetComponent<Stats>().Maximummovement;
                if (unit.gameObject.GetComponent<Stats>().unittype == unit.gameObject.GetComponent<Stats>().Mining)
                {
                    blackmineersworking++;                   
                }
            }
            Changestates(GameState.PlayerOneTurn, GameState.PlayerTwoTurn,ref curstate);
            Changeteams(ref friendlyunits,ref enemyunits);
            turnchangerr = true;
            endtrunclick = false; 
            Black_income= blackmineersworking * 10;
            Red_income = redminersworking * 10;
            Black_resourse += Black_income;
            Red_resourse += Red_income;
            
            turntimer = 30;
            
            blackmineersworking = 0;
            redminersworking = 0;
            
        }
        if (curstate == GameState.PlayerOneDraft)
        {
            //draftaj unite
            // cursate kad završi draft
            curstate = GameState.PlayerTwoDraft;
        }
        if(curstate == GameState.PlayerTwoDraft)
        {

            curstate = GameState.PlayerOneTurn;
        }

        if(curstate == GameState.PlayerOneTurn)
        {
            Resourcetext.text = Black_resourse.ToString();
        }
        if (curstate == GameState.PlayerTwoTurn)
        {
            Resourcetext.text = Red_resourse.ToString();
        }

        if (_selected && _selected.tag != "BlackBuildings")
        {
            if (friendlystats.movement > 0)
            {
                background = _selected.GetChild(0);
            }
            else
            {
                background.gameObject.SetActive(false);
                background = _selected.GetChild(1);                
            }
            background.gameObject.SetActive(true);
        }
        else if(background != null)
        {
            background.gameObject.SetActive(false);
        } 



        if (Input.GetMouseButtonDown(0) && _selected != null)
        {
            if (_selected.tag != "BlackBuildings")
            {
                background.gameObject.SetActive(false);
            }
            else
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                _selected.GetComponent<BuildingStats>().Ui.SetActive(false);
            }
            _selected = null;
        }

        // raycast on click +++
        if(Input.GetMouseButtonDown(0)){
            RaycastHit2D hit = Physics2D.Raycast(cursorPosition, Vector2.zero, Mathf.Infinity, friendlyunits) ;
            if (hit)
            {
                var selected = hit.transform;
                if (selected.tag == "BlackBuildings")
                {
                    selected.GetComponent<BuildingStats>().Ui.SetActive(true);
                }
                else
                {
                    friendlystats = selected.gameObject.GetComponent<Stats>();
                    unitdamage = friendlystats.damage;
                }
                mouseclick = hit.transform.position;                
                _selected = selected;   

            }
        }
        if (Input.GetMouseButtonDown(1) && _selected )
        {
            mouseclick = cursorPosition;
            distance = Vector2.Distance(_selected.position, mouseclick) ;
            RaycastHit2D enemycheck = Physics2D.Raycast(mouseclick, Vector2.zero, Mathf.Infinity, enemyunits);
            RaycastHit2D friendlycheck = Physics2D.Raycast(mouseclick, Vector2.zero, Mathf.Infinity, friendlyunits);
            RaycastHit2D goldcheck = Physics2D.Raycast(mouseclick, Vector2.zero, Mathf.Infinity, gold);



            if (enemycheck && _selected.gameObject.GetComponent<Stats>().range >= distance && friendlystats.movement >= 1)  //mlačenje dio
            {
                enemystatsref = enemycheck.transform.gameObject.GetComponent<Stats>();
                enemystatsref.currenthealth -= (unitdamage - enemystatsref.armor);
                GameObject Popdmg = Instantiate(Damagepop, enemycheck.transform.position, Quaternion.identity) as GameObject;
                damagetext = Popdmg.transform.GetChild(0).GetComponent<TextMesh>();
                damagetext.text = (unitdamage - enemystatsref.armor).ToString();
                Destroy(Popdmg, 1.0f);
                if (enemystatsref.currenthealth <= 0 && distance <= 1)
                {
                    StartCoroutine(Moveafterkill(0.3f));
                }
                friendlystats.movement = 0;


            }
            else if (goldcheck && _selected.gameObject.GetComponent<Stats>().range >= distance && friendlystats.movement >= 1 && _selected.gameObject.GetComponent<Stats>().unittype == _selected.gameObject.GetComponent<Stats>().Worker)  //worker dio
            {
                if(goldcheck.transform.gameObject.GetComponent<GoldMine>().Miners[0] == null)
                goldcheck.transform.gameObject.GetComponent<GoldMine>().Miners[0] = _selected.gameObject;
                else if (goldcheck.transform.gameObject.GetComponent<GoldMine>().Miners[1] == null)
                    goldcheck.transform.gameObject.GetComponent<GoldMine>().Miners[1] = _selected.gameObject;
                else if (goldcheck.transform.gameObject.GetComponent<GoldMine>().Miners[2] == null)
                    goldcheck.transform.gameObject.GetComponent<GoldMine>().Miners[2] = _selected.gameObject;
                else if (goldcheck.transform.gameObject.GetComponent<GoldMine>().Miners[3] == null)
                    goldcheck.transform.gameObject.GetComponent<GoldMine>().Miners[3] = _selected.gameObject;
                else if (goldcheck.transform.gameObject.GetComponent<GoldMine>().Miners[4] == null)
                    goldcheck.transform.gameObject.GetComponent<GoldMine>().Miners[4] = _selected.gameObject;
                else
                {
                    Debug.Log("Rudnik je pun");
                }

                _selected.gameObject.GetComponent<Stats>().unittype = _selected.gameObject.GetComponent<Stats>().Mining;
                Debug.Log("Našli smo rudnik bravo kretenu!!!!");
            }
            else if (friendlycheck && distance < 1 && friendlystats.movement > 0) //nema mlačenja ali si stisnio na svojeg
            {
                Debug.Log("nemožes na svoje");
            }
            else if (distance < 1 && friendlystats.movement > 0 && groundclick == true) // nema mlačenja nisi stisnuo na svoje pomakni se 
            {
                _selected.transform.position = mouseclick;
                friendlystats.movement--;
                GameObject Popmov = Instantiate(movementpop, friendlystats.transform.position, Quaternion.identity) as GameObject;
                movementtext = Popmov.transform.GetChild(0).GetComponent<TextMesh>();
                movementtext.text = friendlystats.movement.ToString();
                Destroy(Popmov, 1.0f);
                //movementpop
            }
            else
            {
                Debug.Log(" glupane");
            }
            
        }
        walkabl.walkable = false;
        
        

    }




    IEnumerator Moveafterkill(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _selected.transform.position = mouseclick;

    }
    private void Changestates(GameState stateone, GameState statetwo,ref GameState curntstate)
    {
        if(curntstate == stateone)
        {
            curntstate = statetwo;
        }
        else
        {
            curntstate = stateone;
        }
     }
    private void Changeteams(ref int frendlay,ref int enemylay)
    {
        int holder = enemylay;
        enemylay = frendlay;
        frendlay = holder;
        _selected = null;
    }
    
   public void Endturnclick()
    {
        endtrunclick = true;
    }
    
}

