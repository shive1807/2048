//using Firebase.Analytics;

using System;
using UnityEngine;
using UnityEngine.Purchasing;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager, 
// one of the existing Survival Shooter scripts.

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class IAP : Singleton<IAP>, IStoreListener
{

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.


    public static string Remove_ads = "remove_ads";
    public static string unlock_all_items = "unlock_all_items";
    public static string unlock_inapp_item = "unlock_inapp_item";

    public static string buy_5_gems = "buy_5_coins";
    public static string buy_10_gems = "buy_10_coins";
    public static string buy_40_gems = "buy_40_coins";
    public static string buy_100_gems = "buy_100_coins";

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
        builder.AddProduct(buy_5_gems, ProductType.Consumable);
        builder.AddProduct(buy_10_gems, ProductType.Consumable);
        builder.AddProduct(buy_40_gems, ProductType.Consumable);
        builder.AddProduct(buy_100_gems, ProductType.Consumable);

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
    public void Buy5gems()
    {
        BuyProductID(buy_5_gems);
    }
    public void Buy10Coins()
    {
        BuyProductID(buy_10_gems);
    }
    public void Buy40Coins()
    {
        BuyProductID(buy_40_gems);
    }
    public void Buy100Coins()
    {
        BuyProductID(buy_100_gems);
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


    public static bool notopenAdcall = false;
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
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
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
        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.LogError("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // A consumable product has been purchased by this user.


        if (String.Equals(args.purchasedProduct.definition.id, Remove_ads, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            print("Purcahse Remove ads");

            m_StoreController.ConfirmPendingPurchase(m_StoreController.products.WithID(Remove_ads));
            AdManager.Instance.RemoveAds();
        }
        if (String.Equals(args.purchasedProduct.definition.id, buy_5_gems, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GemsManager.Instance.AddGems(5);
        }
        if (String.Equals(args.purchasedProduct.definition.id, buy_10_gems, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GemsManager.Instance.AddGems(10);

            m_StoreController.ConfirmPendingPurchase(m_StoreController.products.WithID(buy_10_gems));
        }
        if (String.Equals(args.purchasedProduct.definition.id, buy_40_gems, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GemsManager.Instance.AddGems(40);

            m_StoreController.ConfirmPendingPurchase(m_StoreController.products.WithID(buy_40_gems));
        }
        if (String.Equals(args.purchasedProduct.definition.id, buy_100_gems, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            GemsManager.Instance.AddGems(100);

            m_StoreController.ConfirmPendingPurchase(m_StoreController.products.WithID(buy_100_gems));
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

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