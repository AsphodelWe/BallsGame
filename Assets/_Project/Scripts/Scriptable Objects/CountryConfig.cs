using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CountryConfig", menuName = "Scripts/Scriptable Objects/CountryConfig")]
public class CountryConfig : ScriptableObject
{

    public string CountryName;
    public GameObject BallPrefabPrew;
    public GameObject BallPrefabBattle;

    [Header("Configs")]
    public CountryGameplayConfig CountryGameplayConfig;
    public BallPhysicsConfig PhysicsConfig;

    [Header("Visual")]
    public Sprite CountryFlag;

    [Header("In UI Change")]
    [NonSerialized] public AttackerConfig SelectedAttacker;
    [NonSerialized] public int SelectedSlotIndex = -1;
    [NonSerialized] public SideConfig Side;

    [Header("Sounds")]
    [Range(0f, 1f)]public float Volume = 0.5f;
    public AudioClip HitSound;

    [Header("Default")]
    public AttackerConfig DefaultAttacker;
    public int DefaultAttackerSlotIndex = 0;
    public SideConfig DefaultSide;

    public Sprite CountrySlotsTemplate;

}
