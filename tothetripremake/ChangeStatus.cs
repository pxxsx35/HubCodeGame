using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStatus : MonoBehaviour
{
    // Start is called before the first frame update
    public bool addTurbo;
    [SerializeField] float radias;
     CarController car;
    void Start()
    {
        addTurbo = false;
        car = FindObjectOfType<CarController>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (car != null)
        {
            //หาระยะห่าง 
            float dist = Vector3.Distance(transform.position, car.transform.position);

                
            //ตรงนี้ก็ตั้งระยะห่างได้เลยว่า เผื่อ Object ขนาดไม่เท่ากันแล้วก็มาแก้ระยะห่างตรงนี้ได้ Z นี้คือ ข้างๆ ส่วน X คือ ระยะห่างของความใกล้จะชนอะ
                if (dist < radias && addTurbo == false)
                {
                    Debug.Log("Target is Close: ");
                    addTurbo = true;
                    car.turborAdd();
                }
                

           
           
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            addTurbo = true;
        }
    }
}
