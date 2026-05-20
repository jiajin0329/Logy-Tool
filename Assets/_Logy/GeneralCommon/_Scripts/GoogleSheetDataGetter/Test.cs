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

    [SerializeField]
    private float _value2;

    public string name => _name;
    public string description => _description;
    public float value => _value;
    public float value2 => _value2;
}