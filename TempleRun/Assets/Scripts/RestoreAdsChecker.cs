using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class RestoreAdsChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        bool canRestore = false;

        switch (Application.platform)
        {
            case RuntimePlatform.WSAPlayerX86:
            case RuntimePlatform.WSAPlayerX64:
            case RuntimePlatform.WSAPlayerARM:

            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.tvOS:
                canRestore = true;
                break;

            case RuntimePlatform.Android:
                switch (StandardPurchasingModule.Instance().appStore)
                {
                    case AppStore.SamsungApps:
                    //case AppStore.CloudMoolah:
                        canRestore = true;
                        break;
                }
                break;
        }

        gameObject.SetActive(canRestore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
