using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FactionName
{
    Gaorrul,
    Vyrzir,
    Alumi,
    Riven
}
public enum WeaponName
{
    Axe,
    Sword,
    Bow,
    Gun
}
public enum Range {
    Melee,
    Ranged
}
public enum Aim
{
    Single,
    Row,
    Column,
    RowColumn,
    All
}
public enum Splash
{
    Single,
    Adjacent,
    All
}

public enum DamageType
{
    Physical,
    Magical
}
// OBSERSER
public enum NotifyAction
{ 
    HoverIn,
    HoverOut,
    Select,
    Deselect
}
public enum Row
{
    Top,
    Bottom
}
public enum Column
{
    Front,
    Back
}
public enum PlayerPosition
{
    Left,
    Right
}