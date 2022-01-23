using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPrences : MonoBehaviour
{
    private InputDevice targetDevice;

    private List<InputDevice> devices = new List<InputDevice>();

    public InputDeviceCharacteristics characteristics;

    public List<GameObject> controllerModels;

    private GameObject spwanedController;

    private GameObject spwanedHand;

    public GameObject handPrefab;

    public bool isHand = true;

    private Animator handAnimator;
    void Start()
    {
        //StartCoroutine(GetDevices(1.0f));
        deviceInit();
    }

    void deviceInit()
    {

        //InputDevices.GetDevices(devices);
        //InputDeviceCharacteristics charcacter = InputDeviceCharacteristics.Right;
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);

        foreach (var item in devices)
        {
           // Debug.Log(item.name + item.characteristics);
        }
        //Debug.Log("Finished!");
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerModels.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {

                spwanedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.Log("没有找到模型");
                spwanedController = Instantiate(controllerModels[0], transform);
            }
            spwanedHand = Instantiate(handPrefab, transform);
            handAnimator = spwanedHand.GetComponentInChildren<Animator>();
            Debug.Log(handAnimator.name);
        }

    }

    void updateAnimator()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerData))
        {
            handAnimator.SetFloat("Trigger", triggerData);
            
        }
            
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripData))
        {
            handAnimator.SetFloat("Grip", gripData);
        }
            

    }

    // Update is called once per frame
    void Update()
    {
       
        if (!targetDevice.isValid)
        {
            deviceInit();
           
        }
        // 
        if (isHand  && targetDevice.isValid)
        {
            spwanedHand.SetActive(true);
            spwanedController.SetActive(false);
            updateAnimator();
        }
        else if(!isHand && targetDevice.isValid)
        {
            spwanedController.SetActive(true);
            spwanedHand.SetActive(false);
        }


        //targetDevice.TryGetFeatureValue(CommonUsages.bu)
    }
}
