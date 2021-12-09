using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private MeshRenderer book;
    [SerializeField] private MeshRenderer kitchen;
    [SerializeField] private MeshRenderer living;
    [SerializeField] private MeshRenderer room;
    [SerializeField] private MeshRenderer library;
    [SerializeField] private MeshRenderer grave;
    [SerializeField] private MeshRenderer bath;

    private void Start()
    {
        book.enabled = false;
        kitchen.enabled = false;
        living.enabled = false;
        room.enabled = false;
        library.enabled = false;
        grave.enabled = false;
        bath.enabled = false;
    }

    public void putObject(ExplorableObject explorableObject)
    {
        switch (explorableObject.getType())
        {
            case WorldManager.ObjectTypes.BATH:
                bath.enabled = true;
                break;
            case WorldManager.ObjectTypes.BOOK:
                book.enabled = true;
                break;
            case WorldManager.ObjectTypes.ROOM:
                room.enabled = true;
                break;
            case WorldManager.ObjectTypes.KITCHEN:
                kitchen.enabled = true;
                break;
            case WorldManager.ObjectTypes.LIBRARY:
                library.enabled = true;
                break;
            case WorldManager.ObjectTypes.GRAVEYARD:
                grave.enabled = true;
                break;
        }
    }
}