using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Generate : MonoBehaviour
{
	
	
	public static int startNodes = 10;//number of starting nodes
	public static List<Node> allNodes = new List<Node>();
    public static List<Node> teamA = new List<Node>();
    public static List<Node> teamB = new List<Node>();
	public static List<string> namesList = new List<string>();

    public GameObject keepTryingButton;
	
    // Start is called before the first frame update
    void Start()
    {
		GetNamesList();
        CreateNodes(startNodes);
        //CreateNewspaper();
        CreateAllegiances();
        CreateHeadlines();
    }
	
	
	
	public void GetNamesList(){
		string namesText = Resources.Load<TextAsset>("TextFiles/Names.txt").text;
		string [] namesArray = namesText.Split('\n');
		namesList = new List<string>(namesArray);
	}
	
	public string GetName(){
		int random = Random.Range(0,namesList.Count - 1);
		string name = namesList[random];
		namesList.RemoveAt(random);
		return name;
	}
	
	

	public void CreateNodes(int amount){
		GameObject prefab = Resources.Load<GameObject>("Prefabs/Node");
		for(int i=0;i<=amount;i++){
			CreateNode(prefab);
		}
	}
	
	
	public void CreateNode(GameObject prefab){
		Node newNode = Instantiate(prefab).GetComponent<Node>() as Node;
		GameObject nodes = GameObject.Find("Nodes");
		newNode.transform.parent = nodes.transform;
		TextMeshProUGUI newTMP = newNode.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>() as TextMeshProUGUI;
		newNode.name = GetName();
		newTMP.SetText(newNode.name);
		RectTransform newRectTransform = newNode.gameObject.GetComponent <RectTransform>() as RectTransform;
		
		//check if this is the main/first node
		if(allNodes.Count == 0){
			Debug.Log( newNode.name + " is the Main Node");
			newRectTransform.anchoredPosition = Vector2.zero;
			allNodes.Add(newNode);
			return;
		}
		
		newRectTransform.anchoredPosition = new Vector2(Random.Range(-800,800),Random.Range(-300,300));
		
		float timeout = 0; 
		while(CollisionDetected(newRectTransform)){	
			newRectTransform.anchoredPosition = new Vector2(Random.Range(-800,800),Random.Range(-300,300));
			timeout += Time.deltaTime;
			if(timeout > 8.0){
				Debug.Log("Too Many Collisions, Try Lowering Starting Nodes Setting");
				return;
			}
		}
		allNodes.Add(newNode);
	}
	
	public bool CollisionDetected(RectTransform rtCheck){
		foreach(Node element in allNodes){
			RectTransform rtElement = element.GetComponent<RectTransform>() as RectTransform;
			if((rtElement.anchoredPosition-rtCheck.anchoredPosition).magnitude < 260){
				Debug.Log("Collision Detected. New Position Chosen for " + rtCheck.name);
				return true;
			}
		}
		return false;
	}
	
	
	
	public void CreateNewspaper(){
		CreateFrontPage();
		
		/*
		int pagecount = 2;
		string buffer = "read fail";
		while(buffer!=null){
			GameObject newPage = new GameObject("Page "+ pagecount);
			
			pagecount++;
		}
		*/
	}
	
	
	public void CreateFrontPage(){
		//make front page game object and put it in the proper place in the heirarchy on the canvas
		GameObject frontPage = new GameObject("Front Page");
		GameObject newspapers = GameObject.Find("Newspapers");
		frontPage.transform.parent = newspapers.transform;
		
		//give game object image component from assets
		Image fpImage = frontPage.AddComponent<Image>() as Image;
		fpImage.sprite = Resources.Load<Sprite>("Images/front-page");
		
		//position the paper
		RectTransform rtFrontPage = frontPage.GetComponent<RectTransform>() as RectTransform;
		RectTransform rtNewspapers = newspapers.GetComponent<RectTransform>() as RectTransform;
		rtFrontPage.anchoredPosition = rtNewspapers.anchoredPosition;
		//size the paper
		
		
		
		/*changes the absolute size
		rtFrontPage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,1200);
		rtFrontPage.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,580);
		*/
		
		
		//give game object formatted text of front page headline
		//TextMeshProUGUI fpTMP = frontPage.AddComponent<TextMeshProUGUI>() as TextMeshProUGUI;
		//fpTMP.SetText("TEST");
		
		
		//TODO add lines of squiggles
	}
	
    public void CreateAllegiances() {
        // create desired connections, don't need to create full connection, just dictionary of booleans or something. make sure everything is connected.
        // by the associative nature we're using, all nodes will end up on one of two sides, so maybe you just have to create two lists and randomly assort each node to one of the two lists
        // allNodes[0] => teamA
        // foreach (Node node in allNodes) {
        //    if (node == allNodes[0]) {
        //       continue;
        //    }
        //    randomly add to teamA or teamB

        // store each list as a static variable (team1, team2 or something like that) and a variable that says which team the main node is on (we'd be able to search it, but for ease of use)
        
    }

    public void CreateHeadlines() {
        // create headlines from text list and the two allegiance lists we created, use format strings
        // make sure every entity is in a headline at least once (I think that should cover making sure everything is connected, but check)
    }

    public bool CheckConnections() {
        // use the Connection.Connections list
        // for each connection involving the main node, add the other node to a list
        // compare that list to the allegiance list for the main node's opposition, if they match exactly, return true, else false

        // if teamB matches exactly the connections, return true

        return false;
    }

    public void EndGame() {
        if (CheckConnections()) {
            // put up end text
        } else {
            // put up "keep trying" text and a button to go back to the board
            keepTryingButton.SetActive(true);
        }
    }


}
