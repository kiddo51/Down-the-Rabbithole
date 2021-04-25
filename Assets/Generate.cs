using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Generate : MonoBehaviour
{
	
	
	public static int startNodes = 10;
	public static List<string> namesList = new List<string>();
	
    // Start is called before the first frame update
    void Start()
    {
		GetNamesList();
        CreateNodes(startNodes);
    }
	
	public void CreateNodes(int amount){
		GameObject prefab = Resources.Load<GameObject>("Prefabs/Node");
		for(int i=0;i<=amount;i++){
			CreateNode(prefab);
		}
	}
	
	public void CreateNode(GameObject prefab){
		Node newNode = Instantiate(prefab).GetComponent<Node>() as Node;
		GameObject canvas = GameObject.Find("Nodes");
		newNode.transform.parent = canvas.transform;
		TextMeshProUGUI newTMP = newNode.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>() as TextMeshProUGUI;
		newNode.name = GetName();
		newTMP.SetText(newNode.name);
		RectTransform newRectTransform = newNode.gameObject.GetComponent <RectTransform>() as RectTransform;
		newRectTransform.anchoredPosition = new Vector2(Random.Range(-800,800),Random.Range(-300,300));
		
	}

	public void GetNamesList(){
		string namesText = Resources.Load<TextAsset>("TextFiles/Names.txt").text;
		string [] namesArray = namesText.Split('\n');
		namesList = new List<string>(namesArray);
	}

	
	public string GetName(){
		int random = Random.Range(0,namesList.Count);
		string name = namesList[random];
		namesList.RemoveAt(random);
		return name;
	}
	
	
    // Update is called once per frame
    void Update()
    {
        
    }
}
