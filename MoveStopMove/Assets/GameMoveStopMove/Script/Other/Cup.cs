using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    [SerializeField] private Material[] materialCup;
    [SerializeField] private Material materialDefault;
    [SerializeField] private Material materialChange;
    private PlayerMain playerMain;
    private Vector3 position;
    private bool isOnrangeChange;
    private bool beforeState;
    private bool afterState;
    private void Start()
    {
        materialCup = GetComponent<MeshRenderer>().materials;
        playerMain = PlayerMain.Instance;
        position = this.gameObject.transform.position;
        isOnrangeChange = false;
        beforeState = isOnrangeChange;
        afterState = beforeState;
    }
    private void Update()
    {
        if (Vector3.Distance(playerMain.PlayerMainTransform.position, position) <= playerMain.RangeAttack)
        {
            ChangeMaterial();
        }
        else
        {
            SetDefaultMaterial();
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("arrow"))
        {
            other.gameObject.SetActive(false);
        }
    }
    public void ChangeMaterial()
    {
        var mats = new Material[1];
        mats[0] = materialChange;
        GetComponent<MeshRenderer>().materials = mats;
    }
    public void SetDefaultMaterial()
    {
        var mats = new Material[1];
        mats[0] = materialDefault;
        GetComponent<MeshRenderer>().materials = mats;
    }
}
