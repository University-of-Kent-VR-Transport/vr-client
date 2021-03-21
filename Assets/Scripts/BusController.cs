using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class BusController : MonoBehaviour
{
	public string host;
	public GameObject busPrefab;
	public float initalMaxLatitude, initalMaxLongitude, initalMinLatitude, initalMinLongitude;
	private Dictionary<string, GameObject> clones;
	private float maxLatitude, maxLongitude, minLatitude, minLongitude;

	// Start is called before the first frame update
	void Start()
	{
		clones = new Dictionary<string, GameObject>();
		
		maxLatitude = initalMaxLatitude;
		maxLongitude = initalMaxLongitude;
		minLatitude = initalMinLatitude;
		minLongitude = initalMinLongitude;

		var requestURI = buildRequestURI();
		StartCoroutine(GetRequest(requestURI));
	}

	public void setMaxLatitude(float latitude)
	{
		maxLatitude = latitude;
	}

	public void setMaxLongitude(float longitude)
	{
		maxLongitude = longitude;
	}
	
	public void setMinLatitude(float latitude)
	{
		minLongitude = latitude;
	}
	
	public void setMinLongitude(float longitude)
	{
		minLongitude = longitude;
	}

	private IEnumerator GetRequest(string uri)
	{
		UnityWebRequest webRequest = UnityWebRequest.Get(host + uri);
		yield return webRequest.SendWebRequest();

		if (webRequest.isNetworkError || webRequest.isHttpError)
		{
			Debug.Log("Error While Sending: " + webRequest.error);
		}
		else
		{
			if (webRequest.isDone)
			{
				EndpointResponse response = JsonUtility.FromJson<EndpointResponse>(webRequest.downloadHandler.text);
				
				var newBuses = response.Buses;
				var currentBusIDs = clones.Keys.ToList();
				
				var busIDsToRemove = getBusesIDsToRemove(currentBusIDs, newBuses);
				foreach (string busID in busIDsToRemove)
				{
					setBusColour(clones[busID], Color.red);
				}
				
				var busIDsToUpdate = getBusesIDsToUpdate(currentBusIDs, newBuses);
				foreach (string busID in busIDsToUpdate)
				{
					// var position = new Vector3(response.Buses[busID].Location.Longitude, 0, newBuses[busID].Location.Latitude);
					// var rotation = Quaternion.Euler(0, newBuses[busID].Bearing, 0);
					setBusColour(clones[busID], Color.green);
				}

				var busIDsToCreate = getBusesIDsToCreate(currentBusIDs, newBuses);
				foreach (string busID in busIDsToCreate)
				{
					// var bus = instantiateBus(busID, newBuses[busID].Location.Longitude, newBuses[busID].Location.Latitude, newBuses[busID].Bearing);
						
					// clones[busID] = bus;
				}
			}
		}

		yield return new WaitForSeconds(5);
		StartCoroutine(GetRequest(buildRequestURI()));
	}
	
	private string buildRequestURI()
	{
		return "/api/get-bus-locations?topLeft=" + maxLongitude + "," + minLatitude + "&bottomRight=" + maxLatitude + "," + maxLongitude;
	}

	private List<string> getBusesIDsToCreate(List<string> currentBusIDs, Bus[] newBuses)
	{
		var busIDsToCreate = new List<string>();

		foreach (Bus bus in newBuses)
		{
			if (!currentBusIDs.Contains(bus.ID))
			{
				busIDsToCreate.Add(bus.ID);
			}
		}

		return busIDsToCreate;
	}

	private List<string> getBusesIDsToUpdate(List<string> currentBusIDs, Bus[] newBuses)
	{
		var busIDsToUpdate = new List<string>();

		foreach (Bus bus in newBuses)
		{
			if (currentBusIDs.Contains(bus.ID))
			{
				busIDsToUpdate.Add(bus.ID);
			}
		}

		return busIDsToUpdate;
	}

	private List<string> getBusesIDsToRemove(List<string> currentBusIDs, Bus[] newBuses)
	{
		var busIDsToRemove = new List<string>(currentBusIDs);

		foreach (Bus bus in newBuses)
		{
			if (currentBusIDs.Contains(bus.ID))
			{
				busIDsToRemove.Remove(bus.ID);
			}
		}

		return busIDsToRemove;
	}

	private GameObject instantiateBus(string busID, float longitude, float latitude, float bearing)
	{
		var position = new Vector3(longitude, 0, latitude);
		var rotation = Quaternion.Euler(0, bearing, 0);
		
		var bus = Instantiate(busPrefab, position, rotation, this.GetComponent<Transform>());
		
		setBusColour(bus, Color.cyan);
		bus.name = busID;

		return bus;
	}
	private void setBusColour(GameObject bus, Color colour)
	{
		var busRenderer = bus.GetComponent<Renderer>();
		busRenderer.material.SetColor("_Color", colour);
	}
}
