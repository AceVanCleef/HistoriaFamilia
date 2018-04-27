using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

	public List<Node> Neighbours;
	// Position
	public int x;
	public int y;

	public Node(int xCordinate, int yCoordinate)
	{
		x = xCordinate;
		y = yCoordinate;
		Neighbours = new List<Node>();
	}

	public Node()
	{
		Neighbours = new List<Node>();
	}

	//returns distance in euklidian units (Todo: need to google what it is).
	public float DistanceTo(Node n)
	{
		return Vector2.Distance(
			new Vector2 (x, y),
			new Vector2 (n.x, n.y)
		);
	}
}
