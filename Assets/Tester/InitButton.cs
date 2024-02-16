using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YannickSCF.LSTournaments.Common.Controllers;

public class InitButton : MonoBehaviour
{
    [SerializeField] ConfiguratorController _prefab;
    [SerializeField] Transform _parent;

    public void Init() {
        ConfiguratorController controller = Instantiate(_prefab, _parent);
        controller.Init("");
    }
}
