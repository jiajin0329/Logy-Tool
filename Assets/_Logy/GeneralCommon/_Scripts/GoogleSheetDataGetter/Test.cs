using System;
using UnityEngine;

[Serializable]
public class Test
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private string _description;

    [SerializeField]
    private float _value;

    public string name => _name;
    public string description => _description;
    public float value => _value;
}