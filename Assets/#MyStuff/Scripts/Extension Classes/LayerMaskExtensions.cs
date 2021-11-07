using UnityEngine;

namespace NonViolentFPS.Extension_Classes
{
	public static class LayerMaskExtensions
	{
		public static bool IsGameObjectInMask(this LayerMask _layerMask, GameObject _gameObject)
		{
			int objectLayerMask = (1 << _gameObject.layer);
			return ((_layerMask.value & objectLayerMask) > 0);
		}
	}
}