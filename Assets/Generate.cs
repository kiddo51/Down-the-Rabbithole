using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Generate : MonoBehaviour
{
	
	
	public static int startNodes = 10;//number of starting nodes
	public static List<Node> allNodes = new List<Node>();
    public static Node mainNode;
    public static List<Node> teamA = new List<Node>();
    public static List<Node> teamB = new List<Node>();
	public static List<string> namesList = new List<string>();
    public static List<string> headlines = new List<string>();

    public GameObject keepTryingButton;
    public GameObject[] columns;
    public TextMeshProUGUI topHeadline;
    public GameObject winScreen;

    public static bool soundsOn = true;

    public static void ToggleSounds() {
        soundsOn = !soundsOn;
    }

    // Start is called before the first frame update
    void Start()
    {
		GetNamesList();
        CreateNodes(startNodes);
        //CreateNewspaper();
        CreateAllegiances();
        CreateHeadlines();
        CreateTopHeadline();
    }

    public void CreateTopHeadline() {
        string headlinesText = Resources.Load<TextAsset>("TextFiles/TopHeadlines").text;
        string[] headlinesArray = headlinesText.Split('\n');
        List<string> headlinesList = new List<string>(headlinesArray);

        string unformattedHeadline = headlinesList[Random.Range(0, headlinesList.Count - 1)];
        Debug.Log(mainNode);
        string headline = string.Format(unformattedHeadline, mainNode.name.Trim());
        topHeadline.SetText(headline);
    }	
	
	public void GetNamesList(){
		string namesText = Resources.Load<TextAsset>("TextFiles/Names.txt").text;
		string [] namesArray = namesText.Split('\n');
		namesList = new List<string>(namesArray);
	}
	
	public string GetName(){
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

    public void CreateAllegiances() {
        // create desired connections, don't need to create full connection, just dictionary of booleans or something. make sure everything is connected.
        // by the associative nature we're using, all nodes will end up on one of two sides, so maybe you just have to create two lists and randomly assort each node to one of the two lists
        
		//assign main node to teamA
		teamA.Add(allNodes[0]);
        mainNode = allNodes[0];

		//    randomly add to teamA or teamB
		foreach (Node node in allNodes) {
			if (node == allNodes[0]) {
				continue;
			}
			if(Random.Range(0.0f, 1.0f) < 0.5){
				Debug.Log("Team B includes: " + node.name);
				teamB.Add(node);
            } else {
                teamA.Add(node);
            }
		}
		
        // store each list as a static variable (team1, team2 or something like that) and a variable that says which team the main node is on (we'd be able to search it, but for ease of use)
        
    }

    public void CreateHeadlines() {
        // create headlines from text list and the two allegiance lists we created, use format strings
        // make sure every entity is in a headline at least once (I think that should cover making sure everything is connected, but check)

        string goodHeadlinesText = Resources.Load<TextAsset>("TextFiles/GoodHeadlines").text;
        string badHeadlinesText = Resources.Load<TextAsset>("TextFiles/BadHeadlines").text;
        string[] goodHeadlinesArray = goodHeadlinesText.Split('\n');
        List<string> goodHeadlinesList = new List<string>(goodHeadlinesArray);
        string[] badHeadlinesArray = badHeadlinesText.Split('\n');
        List<string> badHeadlinesList = new List<string>(badHeadlinesArray);

        /*Dictionary<Node, bool> nodesConnectedToMain = new Dictionary<Node, bool>();
        foreach (Node node in allNodes) {
            nodesConnectedToMain[node] = false;
            if (node == mainNode) {
                nodesConnectedToMain[node] = true;
            }
        }*/
        List<Node> nodesConnectedToMain = new List<Node>();
        nodesConnectedToMain.Add(mainNode);

        // Should create at least one headline for each node, and since each refers another, an average of two nodes, with some variety
        List<Node> allOtherNodes = new List<Node>(allNodes);
        allOtherNodes.Remove(mainNode);
        for (int i = 0; i < allOtherNodes.Count + 3; i++) {
            Node node = allOtherNodes[Random.Range(0, allOtherNodes.Count - 1)];
            if (i < allOtherNodes.Count) {
                node = allOtherNodes[i];
            }
            if (node == mainNode) {
                continue;
            }
            Node otherNode = null;
            while (otherNode == null || otherNode == node) {
                otherNode = nodesConnectedToMain[Random.Range(0, nodesConnectedToMain.Count - 1)];
            }

            bool sameTeam = (teamA.Contains(node) && teamA.Contains(otherNode)) || (teamB.Contains(node) && teamB.Contains(otherNode));
            Debug.Log(node.name + " - " + otherNode.name + " sameteam: " + sameTeam + " - " + teamA.Contains(node) + " " + teamA.Contains(otherNode) + " " + teamB.Contains(node) + " " + teamB.Contains(otherNode));
            List<string> headlinesList = goodHeadlinesList;
            if (!sameTeam) {
                headlinesList = badHeadlinesList;
            }
            string unformattedHeadline = headlinesList[Random.Range(0, headlinesList.Count - 1)];
            string headline = string.Format(unformattedHeadline, node.name.Trim(), otherNode.name.Trim());
            headlinesList.Remove(unformattedHeadline);

            headlines.Insert(Random.Range(0, headlines.Count - 1),headline);
            //Debug.Log("Adding headline: " + headline);
            nodesConnectedToMain.Add(node);
            if (nodesConnectedToMain.Contains(mainNode)) {
                nodesConnectedToMain.Remove(mainNode);
            }
        }
        Debug.Log("Headlines: " + headlines.Count);

        // Each column is 8 rows, 4 columns (32 slots total, and we want to sort of spread things out evenly)
        GameObject headlinePrefab = Resources.Load<GameObject>("Prefabs/Headline");
        GameObject scribblePrefab = Resources.Load<GameObject>("Prefabs/Scribble");
        int headlinesPerColumn = headlines.Count / 4;
        if (headlines.Count % 4 != 0) {
            headlinesPerColumn += 1;
        }
        //Debug.Log("Headlines per column: " + headlinesPerColumn);
        for (int column = 0; column < 4; column++) {
            List<int> headlineSpots = new List<int>();
            if (headlinesPerColumn > headlines.Count) {
                headlinesPerColumn = headlines.Count; //This will probably happen to the last column, except in cases where it's evenly divisible by 4
            }
            for (int i = 0; i < headlinesPerColumn; i++) {
                int spot = Random.Range(0, 8);
                while (headlineSpots.Contains(spot)) {
                    spot = Random.Range(0, 8);
                }
                headlineSpots.Add(spot);
            }
            for (int i = 0; i < 8; i++) {
                GameObject newHeadlineObject = null;
                if (headlineSpots.Contains(i)) {
                    newHeadlineObject = Instantiate(headlinePrefab);
                    TextMeshProUGUI newHeadlineText = newHeadlineObject.GetComponent<TextMeshProUGUI>();
                    newHeadlineText.SetText(headlines[0]);
                    headlines.Remove(headlines[0]);
                } else {
                    newHeadlineObject = Instantiate(scribblePrefab);
                }
                newHeadlineObject.transform.parent = columns[column].transform;
                RectTransform newHeadlineTransform = newHeadlineObject.gameObject.GetComponent<RectTransform>();
                newHeadlineTransform.localPosition = Vector2.zero;
            }
        }

    }

    public bool CheckConnections() {
		// use the Connection.Connections list
		// for each connection involving the main node, add the other node to a list
		// compare that list to the allegiance list for the main node's opposition, if they match exactly, return true, else false

		// gather all connections involving main node
		List<Node> checkTeamB = new List<Node>();
		foreach (Connection connection in Connection.Connections){
			//check if main node is part of the connection
			if(connection.NodeA == mainNode && !connection.positive){
				checkTeamB.Add(connection.NodeB);
				Debug.Log("Connection to " + connection.NodeB.name + " acknowledged.");
			}
            if(connection.NodeB == mainNode && !connection.positive){
				checkTeamB.Add(connection.NodeA);
				Debug.Log("Connection to " + connection.NodeA.name + " acknowledged.");
			}
        }

		//check if each teamB node is in checkTeamB and remove it
        foreach(Node node in teamB){
            if (checkTeamB.Contains(node)){
				checkTeamB.Remove(node);
			}
            else{
				Debug.Log("Missing at least one selection");
				Debug.Log(node.name + " is one of the missing suspects");
				return false;
			}
        }

        //win condition
        if (checkTeamB.Count == 0) {
            Debug.Log("You win!");
            winScreen.SetActive(true);
            return true;
        }

        //loop for debugging purposes
        Debug.Log("Too many selections!");
        foreach (Node node in checkTeamB) {
            Debug.Log(node.name + " is not actually part of Team B");
        }

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
