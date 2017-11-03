using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ClickableTile : MonoBehaviour {

	public int tileX;
	public int tileY;
	public TileMap2 map;
    public bool isVacant = true; // bool to check if tile is vacant
    public bool isSelected = false;

    public Texture textureEdge;

    public float bulletSpeed = 10;
    public Rigidbody bullet;

    public Transform bulletSpawn;

    void OnMouseUp() {

        // can be use to change between two materials on clicked object (glow)
        //GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().materials[0];


        //TODO Split clickable tile inheret from tile class(create one) use to remove selected once placed.
        if (isSelected & this.tag == "Tower")
        {
            isSelected = false;
            GetComponent<MeshRenderer>().material.SetTexture("_MainTex", null);
            Debug.Log("Not Selected");
        }
        else if (this.tag == "Tower")
        {
            isSelected = true;
            GetComponent<MeshRenderer>().material.SetTexture("_MainTex", textureEdge);
            Debug.Log("Selected");
        }
        
        // can be use to change the color on clicked object (select)
        //this.GetComponent<Renderer>().material.color = Color.red;

        Debug.Log (this.tag);

        if (EventSystem.current.IsPointerOverGameObject())
           return;

        //Moved to start of level
        map.GeneratePathTo(tileX, tileY);

        //TODO shooting

        Fire();

    }

    void Fire()
    {
        Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, bulletSpawn.position, transform.rotation);
        bulletClone.velocity = bulletSpawn.forward * bulletSpeed;
    }

}
