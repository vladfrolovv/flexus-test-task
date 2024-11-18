using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Inputs
{
    public class InputBlocker
    {

        private const string BlockedLayer = "UI";

        private readonly GraphicRaycaster _raycaster;

        public InputBlocker(GraphicRaycaster raycaster)
        {
            _raycaster = raycaster;
        }

        public bool IsPointerOverUI()
        {
            PointerEventData eventData = new(EventSystem.current)
            {
                position = Input.mousePosition,
            };

            List<RaycastResult> results = new();
            _raycaster.Raycast(eventData, results);

            return results.Any(result => result.gameObject.layer == LayerMask.NameToLayer(BlockedLayer));
        }

    }
}
