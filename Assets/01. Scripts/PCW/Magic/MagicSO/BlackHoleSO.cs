using UnityEngine;

[CreateAssetMenu(menuName = "SO/Karin/BlackHole")]
public class BlackHoleSO : MagicSO
{
    [SerializeField] BlackHole _blackHoldPrefab;

    public override Magic UseMagic()
    {
        return Instantiate(_blackHoldPrefab); 
    }
}
