using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Omnix.Flow.Tests
{
    public class FlowItemPrefab : MonoBehaviour
    {
        [FormerlySerializedAs("nameField")] [SerializeField] private TextMeshProUGUI _nameField;
        [field: SerializeField] public Toggle ToggleForFlowItem { get; private set; }

        public void Setup(BaseFlowStep data, FlowUiBuilder builder)
        {
            RectTransform nameRt = _nameField.rectTransform;
            Vector2 pos = nameRt.anchoredPosition;
            pos.x += builder.Indent;
            nameRt.anchoredPosition = pos;

            string type = data switch
            {
                Branch => "(Branch) ",
                SubFlow => "(Flow) ",
                ParallelSubFlow => "(FlowPara) ",
                _ => ""
            };

            _nameField.text = $"{type}{data.name}";
        }
    }
}