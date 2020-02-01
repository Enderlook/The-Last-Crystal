using System;
using UnityEngine;

namespace Additions.Serializables
{
    [Serializable]
    public class WeightedGameObjects : WeightedElements<WeightedGameObject, GameObject> { }

    [Serializable]
    public class WeightedGameObject : WeightedElement<GameObject> { }
}
