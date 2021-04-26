using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Generate : MonoBehaviour
{
	
	
	public static int startNodes = 10;//number of starting nodes
	public static List<Node> allNodes = new List<Node>();
	public static List<string> namesList = new List<string>();
	
    // Start is called before the first frame update
    void Start()
    {
		GetNamesList();
        CreateNodes(startNodes);
		CreateNewspaper();
    }
	
	
	
	public void GetNamesList(){
		string namesText = Resources.Load<TextAsset>("TextFiles/Names.txt").text;
		string [] namesArray = namesText.Split('\n');
		namesList = new List<string>(namesArray);
	}
	
	public string GetName(){
		if (namesList.Count == 0 || namesList == null){
			Debug.Log( "Error: Could Not Retrieve a Name for a Node");
			return null;
		}
		int random = Random.Range(0,namesList.Count-1);
		string name = namesList[random];
		namesList.RemoveAt(random);
		return name;
	}
	
	

	public void CreateNodes(int amount){
		GameObject prefab = Resources.Load<GameObject>("Prefabs/Node");
		for(int i=0;i<amount;i++){
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
		
		if(allNodes.Count == 0){
			Debug.Log("Error: Node List empty");
			return false;
        }

		if(allNodes == null){
			Debug.Log("Error: Node List is null");
			return false;
		}

		foreach(Node element in allNodes){
			RectTransform rtElement = element.GetComponent<RectTransform>() as RectTransform;
			if((rtElement.anchoredPosition-rtCheck.anchoredPosition).magnitude < 260){
				Debug.Log("Collision Detected. New Position Chosen for " + rtCheck.name);
				return true;
			}
		}
		Debug.Log("Placement Successfully chosen for " + rtCheck.name);
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
	
    // Update is called once per frame
    void Update()
    {
        
    }
}
