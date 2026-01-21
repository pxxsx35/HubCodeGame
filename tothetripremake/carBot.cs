using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeshMaterialData
{
    public MeshRenderer renderer;
    public Material[] originalMaterials;
}

public class carBot : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private int speed;

    [Header("Visual Effects")]
    [SerializeField] private Material burnMat;
    [SerializeField] private ParticleSystem effBomb, effFire;

    [Header("Debug Info")]
    [SerializeField] private List<MeshMaterialData> meshDataList = new List<MeshMaterialData>();

    private Rigidbody rb;
    private Collider colliderCar;
    private Transform player;

    private bool breakCar;
    private bool isBroken;
    private bool isLoseControl;
    private float countdown = 0;

    private void Awake()
    {
        // Cache references ครั้งเดียวเพื่อประหยัดทรัพยากร
        if (GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody>();
        colliderCar = GetComponent<Collider>();

        // เก็บข้อมูล Material เดิมทั้งหมดของทุกส่วนย่อยในตัวรถ
        CacheOriginalMaterials();
    }

    private void OnEnable()
    {
        // หยุดการทำงานที่ค้างมาจากรอบที่แล้วทั้งหมด
        StopAllCoroutines();

        // รีเซ็ตสถานะทางฟิสิกส์และตัวแปรควบคุม
        ResetState();

        // คืนค่าความสวยงามให้เหมือนรถใหม่
        ResetVisuals();
    }

    private void CacheOriginalMaterials()
    {
        meshDataList.Clear();
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            MeshMaterialData data = new MeshMaterialData();
            data.renderer = mesh;
            data.originalMaterials = mesh.sharedMaterials;
            meshDataList.Add(data);
        }
    }

    private void ResetState()
    {
        isBroken = false;
        breakCar = false;
        isLoseControl = false;
        countdown = 0;
        speed = Random.Range(25, 35);

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;
        }
        if (colliderCar != null) colliderCar.isTrigger = false;
    }

    private void ResetVisuals()
    {
        // คืนค่า Material เดิมตามลำดับ Array ที่ถูกต้อง
        foreach (var data in meshDataList)
        {
            if (data.renderer != null) data.renderer.materials = data.originalMaterials;
        }

        if (effBomb != null) effBomb.Stop();
        if (effFire != null) effFire.Stop();
    }

    void Update()
    {
        if (isBroken || player == null) return;

        if (!isLoseControl)
        {
            HandleDriving();
        }
        else
        {
            HandleLoseControl();
        }
    }

    private void HandleDriving()
    {
        RaycastHit hit;
        // ปรับทิศทาง Ray ตามแกนที่รถคุณหันไป (เช่น transform.right หรือ transform.forward)
        if (Physics.Raycast(transform.position, transform.right, out hit, 3f) && !breakCar)
        {
            breakCar = true;
            StartCoroutine(UnbreakRoutine(0.5f));
        }

        float currentSpeed = breakCar ? Mathf.Lerp(10, speed, Time.deltaTime * 2) : speed;
        rb.velocity = new Vector3(currentSpeed, -10f, rb.velocity.z);
    }

    private void HandleLoseControl()
    {
        countdown += Time.deltaTime;
        if (countdown > 0.65f) isLoseControl = false;
    }

    public void BounceOff()
    {
        countdown = 0;
        isLoseControl = true;
        rb.AddForce(new Vector3(6f, 0f, Random.Range(-6f, 6f)), ForceMode.Impulse);
    }

    public void Dead()
    {
        if (isBroken) return;
        isBroken = true;

        ApplyBurnMaterial();

        if (effBomb != null) effBomb.Play();
        if (effFire != null) effFire.Play();

        rb.AddForce(new Vector3(20f, 10f, Random.Range(-15f, 15f)), ForceMode.Impulse);
        colliderCar.isTrigger = true;

        // ใช้ Coroutine แทน Invoke เพื่อแก้ปัญหา Argument Error
        StartCoroutine(DeactivateRoutine(2f));
    }

    private void ApplyBurnMaterial()
    {
        foreach (var data in meshDataList)
        {
            if (data.renderer == null) continue;

            Material[] burnArray = new Material[data.originalMaterials.Length];
            for (int i = 0; i < burnArray.Length; i++) burnArray[i] = burnMat;
            data.renderer.materials = burnArray;
        }
    }

    IEnumerator UnbreakRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        breakCar = false;
    }

    IEnumerator DeactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false); // ปิดเพื่อคืนเข้า Pool ให้ GenerateCar มองเห็น
    }
}