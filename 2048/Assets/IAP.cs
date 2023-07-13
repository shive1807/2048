//using Firebase.Analytics;

using System;
using UnityEngine;
using UnityEngine.Purchasing;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager, 
// one of the existing Survival Shooter scripts.

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class IAP : MonoBehaviour, IStoreListener
{

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.


    public static string Remove_ads = "remove_ads";
    public static string unlock_all_items = "unlock_all_items";
    public static string unlock_inapp_item = "unlock_inapp_item";
    
    public static string buy_200_coins = "buy_200_coins";
    public static string buy_500_coins = "buy_500_coins";
    public static string buy_1000_coins = "buy_1000_coins";
    public static string buy_2000_coins = "buy_2000_coins";
    public static string buy_5000_coins = "buy_5000_coins";
    
    void Start()
    {

        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());


        builder.AddProduct(Remove_ads, ProductType.NonConsumable);
        builder.AddProduct(unlock_all_items, ProductType.NonConsumable);
        builder.AddProduct(unlock_inapp_item, ProductType.NonConsumable);
        builder.AddProduct(buy_200_coins, ProductType.Consumable);
        builder.AddProduct(buy_500_coins, ProductType.Consumable);
        builder.AddProduct(buy_1000_coins, ProductType.Consumable);
        builder.AddProduct(buy_2000_coins, ProductType.Consumable);
        builder.AddProduct(buy_5000_coins, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void BuyConsumableRemove_Ads()
    {

        BuyProductID(Remove_ads);
    }
    public void BuyConsumableUnlock_All()
    {

        BuyProductID(unlock_all_items);
    }
   
    public void UnlockAllInapp()
    {
        BuyProductID(unlock_inapp_item);
    }
    public void Buy200Coins()
    {

        BuyProductID(buy_200_coins);
    }
    public void Buy500Coins()
    {

        BuyProductID(buy_500_coins);
    }
    public void Buy1000Coins()
    {

        BuyProductID(buy_1000_coins);
    } public void Buy2000Coins()
    {

        BuyProductID(buy_2000_coins);
    }
    public void Buy5000Coins()
    {

        BuyProductID(buy_5000_coins);
    }
    public string Getproductpricefromgoogle(string id)
    {
        if (m_StoreController != null && m_StoreController.products != null)
        {
            return m_StoreController.products.WithID(id).metadata.localizedPriceString;

        }
        else
        {
            return "";
        }
    }


    public static bool notopenAdcall=false;
    void BuyProductID(string productId)
    {
        notopenAdcall = true;
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }


    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();

        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // A consumable product has been purchased by this user.


        if (String.Equals(args.purchasedProduct.definition.id, Remove_ads, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            print("Purcahse Remove ads");
            PlayerPrefs.SetInt("RemoveAds", 1);
            m_StoreController.ConfirmPendingPurchase(m_StoreController.products.WithID(Remove_ads));
        }
        if (String.Equals(args.purchasedProduct.definition.id, unlock_all_items, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            PlayerPrefs.SetInt("RemoveAds", 1);


            m_StoreController.ConfirmPendingPurchase(m_StoreController.products.WithID(unlock_all_items));

        }
        if (String.Equals(args.purchasedProduct.definition.id, unlock_inapp_item, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            m_StoreController.ConfirmPendingPurchase(m_StoreController.products.WithID(unlock_inapp_item));


        }
        if (String.Equals(args.purchasedProduct.definition.id, buy_200_coins, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + 200);
            GemsManager.Instance.AddGems(30);

        }
        //if (String.Equals(args.purchasedProduct.definition.id, buy_500_coins, StringComparison.Ordinal))
        //{
        //    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        //    PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + 500);
        //    GemsManager.Instance.AddGems(2000);

        //    m_StoreController.ConfirmPendingPurchase(m_StoreController.products.WithID(buy_500_coins));

        //}

        //if (String.Equals(args.purchasedProduct.definition.id, buy_1000_coins, StringComparison.Ordinal))
        //{
        //    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        //    PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + 1000);
        //    m_StoreController.ConfirmPendingPurchase(m_StoreController.products.WithID(buy_1000_coins));
        //}
        //if (String.Equals(args.purchasedProduct.definition.id, buy_2000_coins, StringComparison.Ordinal))
        //{
        //    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        //    PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + 2000);
        //}

        //if (String.Equals(args.purchasedProduct.definition.id, buy_5000_coins, StringComparison.Ordinal))
        //{
        //    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        //    PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + 5000);
        //}
        //else
        //{
        //    Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        //}

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        // throw new NotImplementedException();
    }
}