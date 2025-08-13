
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Karin/Magic/MagicMissile")]
public class MagicMissileSO : MagicSO
{
    [SerializeField] private MagicMissile _magicPrefab;
    

    public override GameObject UseMagic()
    {
        MagicMissile magic = Instantiate(_magicPrefab);
        magic.SetUp(Damage, 8);
        return magic.gameObject;
    }
}