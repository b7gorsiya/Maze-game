using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    public void PurchasedPack1(Product product)
    {
        GameController.instanse.UpdateCoins(250);
        Debug.Log("Pack purchsed "+product);
        GameController.instanse.ShowMsg(" 250 Coins Added in Storage ",null);
    }
    public void PurchasedPack2(Product product)
    {
        GameController.instanse.UpdateCoins(700);
        Debug.Log("Pack purchsed " + product);
        GameController.instanse.ShowMsg(" 700 Coins Added in Storage ", null);

    }
    public void PurchasedPack3(Product product)
    {
        GameController.instanse.UpdateCoins(1200);
        Debug.Log("Pack purchsed " + product);
        GameController.instanse.ShowMsg(" 1200 Coins Added in Storage\n And Enjoy Adfree Experience ", null);
        if (PlayerPrefs.GetInt("AdFree", 0) == 0)
        {
            PlayerPrefs.SetInt("AdFree", 1);
           // GameController.instanse.adPlugin.HideBannerAd();
           // GameController.instanse.admobAds.bannerView.Hide();
           // GameController.instanse.unityAds.HideBannerAd();
            FindObjectOfType<Start_Scene_UI_Mangment>().removeAd.gameObject.SetActive(false);
        }
    }

    public void AdfreePurchased()
    {
        PlayerPrefs.SetInt("AdFree", 1);
        GameController.instanse.ShowMsg(" Enjoy Adfree Experience ", null);
        //GameController.instanse.admobAds.bannerView.Hide();
       // GameController.instanse.adPlugin.HideBannerAd();
      //  GameController.instanse.unityAds.HideBannerAd();
        FindObjectOfType<Start_Scene_UI_Mangment>().removeAd.gameObject.SetActive(false);
    }
}
