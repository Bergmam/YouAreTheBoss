using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Parameters
{

    public static float scrollDelay = 0.15f;
    public static float MAX_MELE_RANGE = 1.0f;
    public static float PROJECTILE_SPEED = 2.0f;
    public static float PROJECTILE_RANGE = 0.5f;
    public static float PROJECTILE_SCALE = 0.3f;
    public static Color PROJECTILE_COLOR = Color.red;
    public static float ARENA_BOTTOM = -4.0f;

    public static float SLOW_ATTACK_LIMIT = 2.5f;

    public static Color AIM_DEFAULT_COLOR = new Color(0.0f, 0.0f, 0.0f, 0.2f);

    public static Color AIM_DAMAGE_COLOR = new Color(1.0f, 0.0f, 0.0f, 0.7f);

    public static Color[] COLOR_LIST = new Color[3] { Color.red, Color.blue, Color.green };
    public static Color ENEMY_ATTACK_COLOR = Color.black;

    public static float ENEMY_SPAWN_RADIUS = 5f;
    public static int NUMBER_OF_ATTACKS = 4;

    public static float STRONG_ENEMY_MIN_DAMAGE = 2.0f;
    public static float FAST_ENEMY_MIN_SPEED = 3.0f;
    public static float DURABLE_ENEMY_MIN_HEALTH = 200f;
    public static int ATTACK_UPGRADE_WAVE_NUMBER = 5;

    public static float STANDARD_WAVE_DURATION = 4.5f;

    public static float HEALTH_SCALE = 0.7f;

    public static float SPRITE_SCALE_FACTOR = 0.75f;
    public static Color AttACK_BUTTON_COOLDOWN_COLOR = new Color32(0, 229, 225, 160);
}
