using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class BusController : MonoBehaviour
{
	public string host;
	public GameObject busPrefab;
	public float initialMaxLatitude, initialMaxLongitude, initialMinLatitude, initialMinLongitude;
	private Dictionary<string, GameObject> clones;
	private float maxLatitude, maxLongitude, minLatitude, minLongitude;

	// Start is called before the first frame update
	void Start()
	{
		clones = new Dictionary<string, GameObject>();
		
		maxLatitude = initialMaxLatitude;
		maxLongitude = initialMaxLongitude;
		minLatitude = initialMinLatitude;
		minLongitude = initialMinLongitude;

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

				updateBusLocations(response.Buses);
			}
		}

		yield return new WaitForSeconds(5);
		StartCoroutine(GetRequest(buildRequestURI()));
	}
	
	private string buildRequestURI()
	{
		return "/api/get-bus-locations?topLeft=" + minLongitude + "," + maxLatitude + "&bottomRight=" + maxLongitude + "," + minLatitude;
	}

	private void updateBusLocations(Bus[] updatedBuses)
	{
		var busIDsToRemove = new List<string>(clones.Keys.ToList());

		foreach (Bus bus in updatedBuses)
		{
			if (!busIDsToRemove.Contains(bus.ID))
			{
				// create
				var newBus = createBus(bus);
				clones[bus.ID] = newBus;
			}
			else
			{
				// update
				busIDsToRemove.Remove(bus.ID);

				updateBus(bus);
			}
		}

		foreach (string busID in busIDsToRemove)
		{
			removeBus(busID);
		}
	}

	private GameObject createBus(Bus bus)
	{
		var position = new Vector3(bus.Location.Longitude, 0, bus.Location.Latitude);
		var rotation = Quaternion.Euler(0, bus.Bearing, 0);

		var newBusInstance = Instantiate(busPrefab, position, rotation, this.GetComponent<Transform>());

		setBusColour(newBusInstance, Color.cyan);
		newBusInstance.name = bus.ID;

		return newBusInstance;
	}

	private void updateBus(Bus bus)
	{
		var position = new Vector3(bus.Location.Longitude, 0f, bus.Location.Latitude);
		var rotation = Quaternion.Euler(0, bus.Bearing, 0);

		// TODO: pass the new location to the bus to set as the next way point with a final bearing
		setBusColour(clones[bus.ID], Color.green);
	}

	private void removeBus(string busID)
	{
		// TODO: particle effect to show left?

		setBusColour(clones[busID], Color.red);
		Destroy(clones[busID], 1);
		clones.Remove(busID);
	}

	private void setBusColour(GameObject bus, Color colour)
	{
		var busRenderer = bus.GetComponent<Renderer>();
		busRenderer.material.SetColor("_Color", colour);
	}
}
