using System.Collections;
using UnityEngine;

public class ItemFalling : MonoBehaviour
{
    float timeCount = 0;
    public float coolTime;
    public GameObject[] item;
    public Transform player;
    private int fallCount;
    [Range(0,10)]
    public int count;
    [SerializeField] private int minAmount, maxAmount;
    [SerializeField] private float offset;
    private BoxCollider box;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        transform.position = new Vector3(player.position.x + offset, player.position.y, transform.position.z); //ให้ตำแหน่งที่ปล่อยระเบิด อยู่หน้าผู้เล่นตามจำนวน offset


        if(timeCount > coolTime)
        {
            fallCount = 0;
            if(Random.value > 0.3) // สุ่มว่าจะสร้างระเบิดไหม
            {
                StartCoroutine(RandomItemSpawner(Random.Range(minAmount, maxAmount)));
            }
            
            
            timeCount = 0;

        }
        else
        {
            timeCount += 1 * Time.deltaTime;
        }
    }

    IEnumerator RandomItemSpawner(int fallAmount)
    {
        yield return new WaitForSeconds(0.25f);
        if(fallCount < fallAmount)
        {
            int index = Random.Range(0, item.Length);


            if (item[index] != null) 
            {
                // สุ่มตำแหน่งจากภายใน box collider
                Vector3 pos = new Vector3(Random.Range((transform.position.x - (box.size.x / 2)), (transform.position.x + (box.size.x / 2))),
                    transform.position.y,
                    Random.Range((transform.position.z - (box.size.z / 2)), (transform.position.z + (box.size.z / 2))));

                GameObject items = Instantiate(item[index], pos, Quaternion.Euler(90, 0, 0));

                //สร้างระเบิดต่อจนกว่าจะครบจำนวนกำหนด
                fallCount++;
                StartCoroutine(RandomItemSpawner(fallAmount - 1));
            }
        }
    }
}
