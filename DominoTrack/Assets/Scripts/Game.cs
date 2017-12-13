using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	public static Track track {
		get;
		private set;
	} 

	void Start () {
		track = new Track ();
	}

}
