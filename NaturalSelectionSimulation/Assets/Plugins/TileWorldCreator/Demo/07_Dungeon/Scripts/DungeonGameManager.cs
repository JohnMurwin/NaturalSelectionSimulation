using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TWC;

namespace TWC.Demo
{
	public class DungeonGameManager : MonoBehaviour
	{
	
		private static DungeonGameManager _instance;
		public static DungeonGameManager Instance { get { return _instance; } }
		
		
		public TileWorldCreator tileWorldCreator;
		public GameObject loadingScreen;
		
		void OnEnable()
		{
			tileWorldCreator.OnBlueprintLayersComplete += BlueprintMapReady;
			tileWorldCreator.OnBuildLayersComplete += MapReady;
		}
		
		void OnDisable()
		{
			tileWorldCreator.OnBlueprintLayersComplete -= BlueprintMapReady;
			tileWorldCreator.OnBuildLayersComplete -= MapReady;
		}
		
		void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(this.gameObject);
			} 
			else 
			{
				_instance = this;
			}
		}
	
		// Generate new dungeon on start
	    void Start()
	    {
		    GenerateDungeon();
	    }
		
		// Player has reached the exit, generate new dungeon
		public void ExitReached()
		{
			Debug.Log("Player has reached portal - generating new dungeon");
			
			// Destroy player object and portal object to avoid any physics errors
			Destroy(GameObject.FindObjectOfType<PlayerMovement>().gameObject);
			Destroy(GameObject.FindObjectOfType<PlayerPortal>().gameObject);
			
			StartCoroutine(GenerateDungeonIE());
		}
		
		IEnumerator GenerateDungeonIE()
		{
			loadingScreen.SetActive(true);
			
			yield return new WaitForSeconds(1f);
			
			GenerateDungeon();
		}
		
		void GenerateDungeon()
		{
			loadingScreen.SetActive(true);
			tileWorldCreator.SetCustomRandomSeed(System.Environment.TickCount);
			tileWorldCreator.ExecuteAllBlueprintLayers();
		}
		
		
		void BlueprintMapReady(TileWorldCreator _twc)
		{
			Debug.Log("Blueprint ready");
			tileWorldCreator.ExecuteAllBuildLayers(true);
		}
		
		void MapReady(TileWorldCreator _twc)
		{
			Debug.Log("Build complete");
			loadingScreen.SetActive(false);
		}
	}
}
