using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private Wizard _wizard;
    [SerializeField] private Transform _spawnTrm;

    void Start()
    {
        Wizard wizard = Instantiate(_wizard, _spawnTrm.position, Quaternion.identity);
        wizard.transform.rotation = Quaternion.Euler(-90f, 0, 0);
    }
}
