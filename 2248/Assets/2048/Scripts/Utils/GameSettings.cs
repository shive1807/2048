using UnityEngine;

public class GameSettings
{
    public const int GRID_WIDTH        = 6;
    public const int GRID_HEIGHT       = 8;
    public const float SPACING         = 150;

    public const float GAME_END_TIME         = 3f;

    public const string BannerUnitId        = "ca-app-pub-3940256099942544/6300978111";
    public const string InterstitialUnitId  = "ca-app-pub-3940256099942544/1033173712";
    public const string RewardedUnitId      = "ca-app-pub-3940256099942544/5224354917";

    public static float GRID_SPACING;
    public static float BLOCK_SIZE;

    public          static  Vector2 SAFE_AREA_SIZE;
    public          static  Vector2 GRID_SIZE;
    public readonly static  Vector2 GRID = new Vector2(6, 8);

}