using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Wall : TileContent
{
    public Transform turretSpawnPoint = default;
    [SerializeField]
    private Color hoverColor = default;
    

    // ---- INTERN ----
    private GameObject turret;
    private Turret turretBlueprint;

    private Renderer rend;
    private Color startColor;


    void Awake()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
