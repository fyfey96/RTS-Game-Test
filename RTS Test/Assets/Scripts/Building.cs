using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingPlacement
{
    VALID,
    INVALID,
    FIXED
};

public class Building
{
    private BuildingData _data;
    private Transform _transform;
    private int _currentHealth;
    private BuildingPlacement _placement;
    private BuildingManager _buildingManager;
    private List<Material> _materials;

    public Building(BuildingData data)
    {
        _data = data;
        _currentHealth = data.HP;

        _materials = new List<Material>();

        foreach(Material material in _transform.Find("Mesh").GetComponent<Renderer>().materials)
        {
            _materials.Add(new Material(material));
        }

        GameObject g = GameObject.Instantiate(Resources.Load($"Prefabs/Buildings/{_data.Code}")) as GameObject;
        _transform = g.transform;
        _buildingManager =g.GetComponent<BuildingManager>();
        _placement = BuildingPlacement.VALID;
        SetMaterials();
    }

    public void SetMaterials() {SetMaterials(_placement);}
    public void SetMaterials(BuildingPlacement placement)
    {
        List<Material> materials;
        if(placement == BuildingPlacement.VALID)
        {
            Material refMaterial = Resources.Load("Materials/Valid") as Material;
            materials = new List<Material>();
            for (int i =0; i < _materials.Count; i++)
            {
                materials.Add(refMaterial);
            }
        }
        else if (placement == BuildingPlacement.FIXED)
        {
            materials = _materials;
        }
        else
        {
            return;
        }

        _transform.Find("Mesh").GetComponent<Renderer>().materials = materials.ToArray();
    }

    public void SetPosition(Vector3 position)
    {
        _transform.position = position;
    }

    public string Code {get => _data.Code;}
    public Transform Transform {get => _transform;}
    public int HP {get => _currentHealth; set => _currentHealth = value;}
    public int MaxHP {get => _data.HP;}
    
    public int DataIndex
    {
        get {
            for (int i = 0; i < Globals.BUILDING_DATA.Length; i++)
            {
                if(Globals.BUILDING_DATA[i].Code == _data.Code)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public void Place()
    {
        _placement = BuildingPlacement.FIXED;
        SetMaterials();

        _transform.GetComponent<BoxCollider>().isTrigger = false;
    }


    public void CheckValidPlacement()
    {
        if (_placement ==BuildingPlacement.FIXED) return;

        _placement = _buildingManager.CheckPlacement() ?BuildingPlacement.VALID : BuildingPlacement.INVALID;
    }
    public bool IsFixed{get => _placement == BuildingPlacement.FIXED;}
    public bool HasValidPlacment {get=> _placement == BuildingPlacement.VALID;}
}
