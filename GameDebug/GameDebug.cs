using System.Collections.Generic;
using UnityEngine;

namespace DebugToScreen
{
    [DefaultExecutionOrder(-10)]
    public class GameDebug : MonoBehaviour, IComparer<IGameLog>
    {
        #region Static
        public static GameDebug Instance { get; private set; }
        #endregion

        #region Serialized Fields
        [SerializeField, Range(3, 100)] private int fontSize = 30;
        [SerializeField, Range(0f, 1f)] private float xOffset = 0f;
        [SerializeField, Range(0f, 1f)] private float yOffset = 0f;
        [SerializeField] private float logOffset = 4f;
        [SerializeField] private Color infoColor;
        [SerializeField] private Color messageColor;
        public bool acceptInput;
        #endregion

        #region Private Fields
        private readonly float _lineOffset = 4f;
        private List<IGameLog> _allLogs;
        private ObjectLog _objectLogs;
        private Rect _rectInfo;
        private Rect _rectMess;
        private float _height;
        #endregion

        #region Properties
        public int FontSize
        {
            get => fontSize;
            set
            {
                if (value <= 3) fontSize = 3;
                else fontSize = value;

                Styles.MessageStyle.fontSize = fontSize;
                Styles.ObjectLogStyle.fontSize = fontSize;
                _rectInfo.height = fontSize;
                _rectMess.height = fontSize;
            }
        }

        public float XOffset
        {
            get => xOffset;
            set
            {
                if (value <= 0) xOffset = 0f;
                else if (value >= 1) xOffset = 1f;
                else xOffset = value;
                UpdateRects();
            }
        }

        public float YOffset
        {
            get => yOffset;
            set
            {
                if (value <= 0) YOffset = 0;
                else if (value >= 0) YOffset = 1;
                else yOffset = value;
                UpdateRects();
            }
        }
        #endregion

        #region Unity Callbacks
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _allLogs = new List<IGameLog>();
                _objectLogs = new ObjectLog();
                DontDestroyOnLoad(gameObject);
                Init();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnGUI()
        {
            #if UNITY_EDITOR
            Init();
            #endif


            float y = _height * yOffset;
            _rectInfo.y = y;
            _rectMess.y = y;
            foreach (IGameLog message in _allLogs)
            {
                message.DrawSelf(_rectMess);
                _rectMess.y += (fontSize + _lineOffset) * message.LinesCount + logOffset;
                if (_rectMess.y >= _height) return;
            }

            _objectLogs.DrawSelf(_rectInfo);
        }
        #endregion

        #region Functionality
        private void Init()
        {
            Styles.MessageStyle = new GUIStyle
            {
                normal = { textColor = messageColor },
                alignment = TextAnchor.UpperRight,
                fontSize = fontSize
            };

            Styles.ObjectLogStyle = new GUIStyle
            {
                normal = { textColor = infoColor },
                alignment = TextAnchor.UpperLeft,
                fontSize = fontSize
            };

            UpdateRects();
        }
        
        private void UpdateRects()
        {
            _height = Screen.height;
            float width = Screen.width;
            float xPos = width * xOffset;
            float yPos = _height * yOffset;
            _rectInfo = new Rect(x: xPos, y: yPos, width: width, height: fontSize);
            _rectMess = new Rect(x: -xPos, y: yPos, width: width, height: fontSize);
        }

        private void AddToLogStack(IGameLog log)
        {
            this._allLogs.Add(log);
            this._allLogs.Sort(this);
        }
        
        internal static void RemoveLog(IGameLog gameLog)
        {
            if (Instance._allLogs.Contains(gameLog))
            {
                Instance._allLogs.Remove(gameLog);
            }
        }
        #endregion

        #region Interface
        public int Compare(IGameLog x, IGameLog y)
        {
            if (x != null && y != null) return x.Priority.CompareTo(y.Priority);
            return 0;
        }
        #endregion

        #region Static Methods
        public static void ClearLogs()
        {
            Instance._allLogs.Clear();
        }
        
        public static Message Log(string message)
        {
            Message r = new Message(message);
            Instance.AddToLogStack(r);
            return r;
        }

        public static Message Log(string message, int priority)
        {
            Message r = new Message(message);
            r.Priority = priority;
            Instance.AddToLogStack(r);
            return r;
        }

        public static Message LogTemp(string message, float duration)
        {
            TempMessage r = new TempMessage(message, duration);
            Instance.AddToLogStack(r);
            return r;
        }

        public static Message LogTemp(string message, float duration, int priority)
        {
            TempMessage r = new TempMessage(message, duration);
            r.Priority = priority;
            Instance.AddToLogStack(r);
            return r;
        }

        // public static void TrackMyField(string fieldName, bool isConstant)
        // {
        //     FieldInfo fi = FirstFieldByName(caller, fieldName);
        //     if (fi != null) Instance._objectLogs[caller].Track(fi, isConstant);
        //     else Debug.LogError($"Cannot find Field with name {fieldName}");
        // }
        //
        // public static void TrackMyProperty(string fieldName, bool isConstant)
        // {
        //     PropertyInfo fi = FirstPropByName(caller, fieldName);
        //     if (fi != null) Instance._objectLogs[caller].Track(fi, isConstant);
        //     else Debug.LogError($"Cannot find Field with name {fieldName}");
        // }

        public static void SetInfo(string message)
        {
            if (Instance.acceptInput)
                Instance._objectLogs.SetText(message);
        }

        public static void AddText(string message)
        {
            if (Instance.acceptInput)
                Instance._objectLogs.AddText(message);
        }

        public static void AddLine(string message)
        {
            if (Instance.acceptInput)
                Instance._objectLogs.AddText($"\n{message}");
        }

        public static void Clear()
        {
            if (Instance.acceptInput)
                Instance._objectLogs.SetText("");
        }
        #endregion
    }
}