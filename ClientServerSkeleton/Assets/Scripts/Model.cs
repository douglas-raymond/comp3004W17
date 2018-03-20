using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model {

	private int[] state;

	public void makeModel(int numPlayers){
		state = new int[numPlayers];
		for (int i = 0; i < numPlayers; i++) {
			state [i] = i;
		}
	}

	public int GetState(int i){
		return state [i];
	}
}
