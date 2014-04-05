using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HexGrid
{
	public static int SpaceTile = 0;
	public static int AsteroidTile = 1;

	/// <summary>Stores terrain data by mapping coordinates to an integer. The integer refers to a unique terrain type.</summary>
	public Dictionary<HexCoord, int> tiles = new Dictionary<HexCoord, int>();

	public HexGrid() {
	}


	public HashSet<HexCoord> getMovementRange(HexCoord startNode, int range) {
		HashSet<HexCoord> moveableTiles = new HashSet<HexCoord>();

		Queue<HexCoord> searchQueue = new Queue<HexCoord>();
		Dictionary<HexCoord, int> ranges = new Dictionary<HexCoord, int>();

		searchQueue.Enqueue(startNode);
		ranges[startNode] = 0;

		while(searchQueue.Count > 0) {
			HexCoord currentNode = searchQueue.Dequeue();
			
			if(ranges[currentNode] <= range) {
				moveableTiles.Add(currentNode);
				
				foreach(HexCoord neighbor in currentNode.getNeighbors()){
					if(!ranges.ContainsKey(neighbor) && tiles.ContainsKey(neighbor)) {
						//TODO: Also confirm that this is a movable square
						
						ranges[neighbor] = ranges[currentNode] + 1;
						searchQueue.Enqueue(neighbor);
					}
				}
			}
		}

		moveableTiles.Remove(startNode);
		return moveableTiles;
	}
}

