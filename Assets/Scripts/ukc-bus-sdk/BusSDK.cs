using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;

public class BusSDK : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

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
	}

	public void SetMaxLatitude(float latitude)
	{
		maxLatitude = latitude;
	}

	public void SetMaxLongitude(float longitude)
	{
		maxLongitude = longitude;
	}
	
	public void SetMinLatitude(float latitude)
	{
		minLongitude = latitude;
	}
	
	public void SetMinLongitude(float longitude)
	{
		minLongitude = longitude;
	}

    public void UpdateBuses()
    {
        StartCoroutine(getNewBusLocations());
    }

	private IEnumerator getNewBusLocations()
	{
        var uri = buildRequestURI();
		UnityWebRequest webRequest = UnityWebRequest.Get(host + uri);
		yield return webRequest.SendWebRequest();

		if (webRequest.isNetworkError || webRequest.isHttpError)
		{
			Debug.Log("Error While Sending: " + webRequest.error + ". URI: " + uri);
		}
		else
		{
			if (webRequest.isDone)
			{
				EndpointResponse response = JsonUtility.FromJson<EndpointResponse>(webRequest.downloadHandler.text);

				updateBusLocations(response.Buses);
			}
		}
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

				updateBus(bus, clones[bus.ID]);
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

        var worldPosition = Conversions.GeoToWorldPosition(position, _map.CenterMercator, _map.WorldRelativeScale);

        var newBusInstance = Instantiate(busPrefab, worldPosition, rotation, this.GetComponent<Transform>());

		setBusColour(newBusInstance, Color.cyan);
		newBusInstance.name = bus.ID;

		return newBusInstance;
	}

	private void updateBus(Bus bus, GameObject busObject)
	{
		var position = new Vector3(bus.Location.Longitude, 0, bus.Location.Latitude);
		var rotation = Quaternion.Euler(0, bus.Bearing, 0);

		// TODO: pass the new location to the bus to set as the next way point with a final bearing
		setBusColour(clones[bus.ID], Color.green);
        busObject.GetComponent<Transform>().SetPositionAndRotation(position, rotation);
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
