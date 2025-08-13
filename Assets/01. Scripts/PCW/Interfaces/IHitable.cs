using UnityEngine;

public interface IHitable
{
    protected int Health { get; set; }
    public void Hit(int value)
    {
        Health -= value;
    }
}
