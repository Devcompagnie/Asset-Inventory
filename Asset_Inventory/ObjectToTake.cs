using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToTake : MonoBehaviour
{
    public int TypeOfObjectToClone;
    public GameObject Player;
    public int TypeOfObject;

    Inventory InventoryScript;

    public bool OnTrigger;
    public bool ToDestroy;
    bool Take;
    // Start is called before the first frame update
    void Start()
    {
        ToDestroy = false;
        InventoryScript = GameObject.Find("GameManager").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OnTrigger == true && Input.GetButtonDown("Action"))
        {
            InventoryScript.TakeObject(TypeOfObject, TypeOfObjectToClone, gameObject.GetComponent<ObjectToTake>());
        }

        if (ToDestroy == true)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            OnTrigger = false;
        }
    }
}
