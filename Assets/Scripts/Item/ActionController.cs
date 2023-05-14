using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; // 습득 가능한 최대 거리.

    private bool pickupActivated = false; // 습득 가능할 시 true

    private RaycastHit hitInfo; // 충돌체의 정보 저장.

    [SerializeField]
    private LayerMask layerMask; // 아이템 레이어에만 반응하도록 레이어 마스크를 설정

    // 필요한 컴포넌트

    [SerializeField]
    private Inventory theInventory;

    // Update is called once per frame
    private void Update()
    {
        //TryAction();
    }

    // 현재 방식은 플레이어에서 발사하는 Raycast가 거리 내에 닿으면 아이템 정보 뜸 , 이것을 그냥 플레이어 쪽으로 옮겨되 돌 것 같다.
    
    private void CheckItem(GameObject _item) 
    {
        
        theInventory.AcquireItem(_item.GetComponent<ItemPickUp>().item, _item);
    }

    // 실제로 아이템을 흭득하는 부분, 인벤토리에 AcquireItem을 해주면서 넣어준다.
   

    // 아이템 정보 생성
    private void ItemInfoAppear() 
    {
        pickupActivated = true;
       
    }

    // 아이템 정보 제거
    private void InfoDisappear()
    {
        pickupActivated = false;
      
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, transform.forward * range);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            CheckItem(other.gameObject);
            //CanPickUp();
        }
    }
    
}
