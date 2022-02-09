using UnityEngine;
using UnityEngine.UI;

public class dragndrop_onclick : MonoBehaviour
{
	//Make sure to attach these Buttons in the Inspector
	public Button m_YourFirstButton;
	public Dropdown m_Dropdown;
	//public Transform prefab;
	public Transform prefab1;
	public Transform prefab2;
	private Transform spawn;

	void Start()
	{
		Button btn1 = m_YourFirstButton.GetComponent<Button>();
		Dropdown derp = m_Dropdown.GetComponent<Dropdown>();

		//Calls the TaskOnClick/TaskWithParameters method when you click the Button
		btn1.onClick.AddListener(delegate {
			Instantiate_Prefab(derp.value); 
		});

		derp.onValueChanged.AddListener (delegate {
			TaskWithParameters (derp.value.ToString());
		});
	}

	void Update()
	{
		
	}


	void TaskWithParameters(string message)
	{
		Debug.Log(message);
	}

	void Instantiate_Prefab(int value)
	{
		if (value == 0) {
			spawn = Instantiate (prefab1, new Vector3 (0, 0, 0), Quaternion.identity);
		}
		if (value == 1) {
			spawn = Instantiate (prefab2, new Vector3 (0, 0, 0), Quaternion.identity);
		}
	}
}