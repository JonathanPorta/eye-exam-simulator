using UnityEngine;
using System.Collections;

public class Lens {
	private int rate;

	public Lens(int rate){
		SetRate (rate);
	}

	public int GetRate(){
		return rate;
	}

	public void SetRate(int rate){
		this.rate = rate;
	}

}
