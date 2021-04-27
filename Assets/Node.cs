using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerClickHandler
{
	public static Node selectedNode;
	public static bool positiveSelect;

    public AudioSource tackSound;	
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

	
	public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left){
			CheckSelected(true);
		}
            
        else if (eventData.button == PointerEventData.InputButton.Middle){
			
		}
            
        else if (eventData.button == PointerEventData.InputButton.Right){
			CheckSelected(false);
		}
            
    }


	public void CheckSelected(bool positive)
	{
		
		if(selectedNode==null){
			selectedNode = this;
			positiveSelect = positive;
			//Debug.Log(gameObject.name + "Selected, positive = " + positive);
		}
		else if(positiveSelect != positive){
			selectedNode = null;
			//Debug.Log("Selection Removed");
		}
		else if(selectedNode==this){
			selectedNode = null;
			//Debug.Log("Selection Removed");
		}
		else{
			//Debug.Log("Altering Connection");
			Connect(positive);
			selectedNode=null;
		}
	}
	
	public void Connect(bool positive)
	{
        if (Generate.soundsOn) {
            tackSound.Play();
        }

        Connection existingConnection = null;
		//check whether connection already exists
		foreach(Connection element in Connection.Connections){
			if(element.Check(this,selectedNode)){
				existingConnection = element;
				if(element.positive!=positive){
					element.positive = positive;
					Image elementImage = element.gameObject.GetComponent <Image>() as Image;
					elementImage.color = Color.green;
					if(!positive){
						elementImage.color = Color.red;
					}
                    return;
				}
			}
		}
		
		if(existingConnection !=null){
			Connection.Connections.Remove(existingConnection);
			Destroy(existingConnection.gameObject);
            return;
		}
		
		//make connection
		GameObject newConnection = new GameObject("Connection: " + this.name + " <-> " + selectedNode.name);
		Connection connection = newConnection.AddComponent <Connection>() as Connection;
		connection.NodeA = this;
		connection.NodeB = selectedNode;
		connection.positive = positive;
		Connection.Connections.Add(connection);
		
		
		//render connection
		float lineWidth = 5.0f;
		Image connectionImage = newConnection.AddComponent <Image>() as Image;
		connectionImage.color = Color.green;
		if(!connection.positive){
			connectionImage.color = Color.red;
		}
		
		RectTransform rectTransformA = connection.NodeA.gameObject.GetComponent<RectTransform>() as RectTransform;
		RectTransform rectTransformB = connection.NodeB.gameObject.GetComponent<RectTransform>() as RectTransform;
		
		Vector3 differenceVector = rectTransformB.position - rectTransformA.position;
 
		RectTransform rectTransform = newConnection.GetComponent<RectTransform>() as RectTransform;
		rectTransform.sizeDelta = new Vector2( differenceVector.magnitude, lineWidth);
		rectTransform.pivot = new Vector2(0, 0.5f);
		rectTransform.position = rectTransformA.position;
		float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
		rectTransform.localRotation = Quaternion.Euler(0,0, angle);
		
		GameObject canvas = GameObject.Find("Connections");
		newConnection.transform.parent = canvas.transform;
		
		
		
		}



    // Update is called once per frame
    void Update()
    {
        
    }
}
