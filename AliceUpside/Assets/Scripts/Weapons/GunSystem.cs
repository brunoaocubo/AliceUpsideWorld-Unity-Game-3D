using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GunSystem : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private LayerMask gunLayerMask;
    [SerializeField] private Transform pivotGun;
    //[SerializeField] private Transform parentBullet;
    private Camera mainCamera;
    private RaycastHit hit;

    [Header("Gun Stats")]
    [SerializeField] private float damageBullet;
    [SerializeField] private int magazineGun;
    [SerializeField] private int bulletPerTap;
    [SerializeField] private float timeBetweenShooting;
    [SerializeField] private float timeBetweenShoots;
    [SerializeField] private float recoil;
    [SerializeField] private float rangeFire;
    [SerializeField] private float reloadTimeGun;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI magazineGun_Text;

    [Header("VFX")]
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject impactHit;


    private int bulletLeft;
    private int bulletsShots;
    private bool isReadyToShoot;
    private bool isShooting;
    private bool isReloading;
    private bool buttonHold;

    private void Awake()
    {
        mainCamera = Camera.main;
        bulletLeft = magazineGun;
        isReadyToShoot = true;
    }

    private void Start()
    {
        //InputController.Instance.ShootAction.performed += _ => CheckInputShoot();
    }

    private void Update()
    {
        CheckInputShoot();
        magazineGun_Text.SetText(bulletLeft + " / " + magazineGun);
    }

    private void ShootGun() 
    {
        InstanceMuzzleFlash();
        StartRecoil();
        bulletLeft--;
        bulletsShots--;
        isReadyToShoot = false;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, rangeFire, gunLayerMask)) 
        {
            if(hit.collider.gameObject.CompareTag("Enemy")) 
            {
                hit.collider.GetComponent<MonsterIA>().TakeDamage(damageBullet);
                StartCoroutine(ImpactHit());
                //Instantiate(impactHit, hit.point, Quaternion.identity);
            }
            else if(hit.collider.gameObject.isStatic)
            {
                StartCoroutine(ImpactHit());
            }
        }

        Invoke("ResetShootGun", timeBetweenShooting);
        if(bulletsShots > 0 && bulletLeft > 0) 
        {
            Invoke("ShootGun", timeBetweenShoots);
        }
    }

    private void ResetShootGun() 
    {
        isReadyToShoot = true;
    }

    private void ReloadGun() 
    {
        isReloading = true;
        Invoke("ReloadFinished", reloadTimeGun);
    }

    private void ReloadFinished() 
    {
        bulletLeft = magazineGun;
        isReloading = false;
    }

    private void CheckInputShoot() 
    {
        if(buttonHold == true) 
        {
            isShooting = InputController.Instance.ShootAction.IsPressed();
        }
        else 
        {
            isShooting = InputController.Instance.ShootAction.triggered;
        }

        if(isReloading != true && bulletLeft < magazineGun && Input.GetKey(KeyCode.R))
        {
            ReloadGun();
        }

        if(isReadyToShoot == true && isReloading == false && isShooting == true && bulletLeft > 0) 
        {
            ShootGun();
        }
        bulletsShots = bulletPerTap;
    }

    private void StartRecoil() 
    {
        float recoil_X = Random.Range(-recoil, recoil);
        float recoil_Y = Random.Range(-recoil, recoil);
        Vector3 recoilDirection = mainCamera.transform.forward + new Vector3(recoil_X, recoil_Y, 0);
    }

    private void InstanceMuzzleFlash() 
    {
        GameObject muzzle = Instantiate(muzzleFlash, pivotGun.transform.position, pivotGun.transform.rotation);
        muzzle.transform.localScale = Vector3.one;
        muzzle.transform.SetParent(gameObject.transform);
        Destroy(muzzle, 0.15f);
    }

    private IEnumerator ImpactHit() 
    {
        impactHit.transform.position = hit.point;
        impactHit.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        impactHit.SetActive(false);
    }
}
