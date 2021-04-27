using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Connection : MonoBehaviour
{
	public static List <Connection> Connections = new List <Connection>();
	public Node NodeA;
	public Node NodeB;
	public bool positive;
	
    public void ClearConnections() {
        List<Connection> oldConnections = new List<Connection>(Connections);
        Connections.Clear();
        foreach (Connection connection in oldConnections) {
            Destroy(connection.gameObject);
        }
    }
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

	//Check if the connection exists
	public bool Check(Node Node1,Node Node2)
	{
		return((Node1==NodeA || Node1==NodeB)&&(Node2==NodeA||Node2==NodeB));
	}
	
	public bool Check(Connection Connection)
	{
		return(Check(Connection.NodeA,Connection.NodeB));
	}
	

    // Update is called once per frame
    void Update()
    {
        
    }
}
