using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionReport : MonoBehaviour
{
    private Langevin_v2 Lange;
    private SaveContacts SaveC;
    

    private void Start()
    {
        
        Lange = GameObject.Find("Simulation").GetComponent<Langevin_v2>();
        SaveC = GameObject.Find("Simulation").GetComponent<SaveContacts>();
        SaveC.MatSize = new int[Lange.GOS.Count, Lange.GOS.Count];

        //Lange = GameObject.Find("Simulation").GetComponent<Langevin_v2>();
        //Debug.Log(Lange == null);
        //Debug.Log("GAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAH!!!!!!!!!!!");
        //SaveC = GameObject.Find("Simulation").GetComponent<SaveContacts>();

        //Debug.Log(SaveC.MatSize.Length + " length!!!!!!!!!!!!!!!!!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        //discriminate between intra and inter molecular contacts
        if (SaveC.MatSize != null)
        {
            Rigidbody Collider = collision.collider.attachedRigidbody;
            Rigidbody Collother = this.gameObject.GetComponent<Rigidbody>();



            //if (Collider.transform.root == Collother.transform.root)
            {
                //Debug.Log(Collider.transform.root.name + " " + Collother.transform.root.name);

                int IndexOwn = Lange.GOS.IndexOf(Collider);
                int IndexOther = Lange.GOS.IndexOf(Collother);
                Debug.Log("My Index is " + IndexOwn + " : Its Index is " + IndexOther);
                Debug.Log("Collider " + Collider + " : Collother " + Collother);
                SaveC.modMatrix(IndexOwn, IndexOther);
            }
        }
        else
        {
            //Debug.Log("MatSize is null");
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    //discriminate between intra and inter molecular contacts

    //    Rigidbody Collider = collision.collider.attachedRigidbody;
    //    Rigidbody Collother = this.gameObject.GetComponent<Rigidbody>();

    //    if (Collider.transform.root == Collother.transform.root)
    //    {
    //        //Debug.Log(Collider.name + Collother.name);

    //        if(!Lange.GOS.Contains(Collider) || !Lange.GOS.Contains(Collother))
    //        {
    //            //Debug.Log(Collider.transform.root.name + " " + Collother.transform.root.name);

    //            int IndexOwn = Lange.GOS.IndexOf(Collider);
    //            int IndexOther = Lange.GOS.IndexOf(Collother);
    //            Debug.Log("My Index is " + IndexOwn + " : Its Index is " + IndexOther);
    //            Debug.Log("Collider " + Collider + " : Collother " + Collother);
    //            SaveContacts.modMatrix(IndexOwn, IndexOther);
    //        }
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (SaveContacts.MatSize != null)
    //    {
    //        Rigidbody Collider = collision.collider.attachedRigidbody;
    //        Rigidbody Collother = this.gameObject.GetComponent<Rigidbody>();

    //        int IndexOwn = Lange.GOS.IndexOf(Collider);
    //        int IndexOther = Lange.GOS.IndexOf(Collother);
    //        //Debug.Log("My Index is " + IndexOwn + " : Its Index is " + IndexOther);
    //        //Debug.Log("Collider " + Collider + " : Collother " + Collother);
    //        SaveContacts.modMatrix(IndexOwn, IndexOther);



    //    }
    //    else
    //    {
    //        //Debug.Log("MatSize is null");
    //    }
    //}
}
