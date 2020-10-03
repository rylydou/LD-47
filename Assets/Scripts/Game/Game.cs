﻿#pragma warning disable 649
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public Player activePlayer;
	public List<PlayerGhost> ghostPlayers = new List<PlayerGhost>();
	public List<Past> pasts = new List<Past>();

	[SerializeField] GameObject pfPlayer;
	[SerializeField] GameObject pfGhostPlayer;
	[Space]
	[SerializeField] Transform tSpawnPoint;
	[SerializeField] Transform tEndPoint;
	[SerializeField] float fTimeToExit;
	[SerializeField] LayerMask lmPlayer;

	float fTimeOnExit;

	void Awake()
	{
		activePlayer = Instantiate(pfPlayer, tSpawnPoint.position, Quaternion.identity).GetComponent<Player>();
	}

	void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		else if (Input.GetKeyDown(KeyCode.Space))
			Redo();

		if (Physics2D.OverlapBox(tEndPoint.position, tEndPoint.localScale, 0, lmPlayer)) fTimeOnExit += Time.deltaTime; else fTimeOnExit = 0;
		if (fTimeOnExit > fTimeToExit) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void Redo()
	{
		pasts.Add(activePlayer.past);
		Destroy(activePlayer.gameObject);
		activePlayer = Instantiate(pfPlayer, tSpawnPoint.position, Quaternion.identity).GetComponent<Player>();

		ghostPlayers.ForEach(x => Destroy(x.gameObject));
		ghostPlayers = new List<PlayerGhost>();
		pasts.ForEach(x => ghostPlayers.Add(Instantiate(pfGhostPlayer, tSpawnPoint.position, Quaternion.identity).GetComponent<PlayerGhost>().SetPast(x)));
	}
}