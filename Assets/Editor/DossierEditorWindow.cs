using UnityEngine;
using UnityEditor;

using CustomEditors.Dossier;

namespace DossierEditor
{
    public class DossierEditorWindow : EditorWindow
    {
        private string enemyName;

        private EnemyData[] enemyList;
        private EnemyData currentPage;
        private Vector2 listScrollPosition;
        private Vector2 pageScrollPosition;

        private EditorData editorData;

        [MenuItem("Custom Editors/Enemy Dossier")]
        public static void ShowWindow()
        {
            DossierEditorWindow window = GetWindow<DossierEditorWindow>("Dossier");

            window.minSize = new Vector2(600, 300);
            window.Show();
        }

        // This interface implementation is automatically called by Unity.
        public void AddItemsToMenu(GenericMenu menu)
        {
            GUIContent content = new GUIContent("Get Editor Data");
            editorData = Resources.LoadAll<EditorData>("")[0];
            menu.AddItem(content, false, MyCallback);
        }

        private void MyCallback()
        {
            Debug.Log(editorData);
        }

        private void OnGUI()
        {
            if (editorData == null)
                return;

            UpdateEnemyList();

            if (enemyList.Length == 0)
            {
                CreateDataEntry();
                currentPage = null;
                return;
            }

            if (currentPage == null)
            {
                currentPage = enemyList[0];
                return;
            }

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            EnemyList();
            GUILayout.Space(5);
            EnemyInfoPage();
            GUILayout.Space(5);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
        }

        private void UpdateEnemyList()
        {
            enemyList = Resources.LoadAll<EnemyData>("DossierEntries/");
        }

        private void EnemyList()
        {
            // Section layout
            GUILayout.BeginVertical(GUIStyle.none, GUILayout.MaxWidth(150), GUILayout.MinWidth(150));

            GUILayout.Space(5);

            // Enemy page button list
            listScrollPosition = GUILayout.BeginScrollView(listScrollPosition, "box");

            foreach (var data in enemyList)
            {
                if (GUILayout.Button(data.name, GUILayout.MinHeight(22)))
                {
                    GUI.FocusControl(null);
                    RefreshAssetDatabase();

                    currentPage = data;
                    pageScrollPosition = Vector2.zero;
                }
            }

            // Section layout
            GUILayout.EndScrollView();

            // Create new entry button
            if (GUILayout.Button("Create new entry", GUILayout.MinHeight(22)))
            {
                CreateDataEntry();
                UpdateEnemyList();
                currentPage = enemyList[enemyList.Length - 1];
            }

            GUILayout.Space(2);
            GUILayout.EndVertical();
        }

        private void EnemyInfoPage()
        {
            // Section layout
            GUILayout.BeginVertical(GUIStyle.none);
            pageScrollPosition = GUILayout.BeginScrollView(pageScrollPosition, GUIStyle.none);

            // Icon and name
            CreatePageHeader();

            // Description
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Description", GetHeaderStyle(Color.white, 15, TextAnchor.MiddleLeft));
            GUILayout.Space(5);

            GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea);
            textAreaStyle.wordWrap = true;
            var margin = editorData.descriptionMarginSize;
            textAreaStyle.padding = new RectOffset(margin, margin, margin, margin);

            currentPage.description = EditorGUILayout.TextArea(currentPage.description, textAreaStyle, GUILayout.ExpandHeight(true));

            DeleteDataEntry();

            // Section layout
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void CreatePageHeader()
        {
            // Section layout
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

            var icon = currentPage.icon == null ? editorData.defaultIcon : currentPage.icon;
            GUI.DrawTexture(new Rect(0, 9, 92, 92), icon, ScaleMode.StretchToFill, true, 10.0f);

            GUILayout.Space(100);
            GUILayout.BeginVertical("box", GUILayout.Height(100));

            // Enemy Name
            var previousPageName = currentPage.name;
            var currentPageName = EditorGUILayout.TextField(currentPage.name, GetHeaderStyle(Color.white, 25, TextAnchor.UpperLeft), GUILayout.Height(35));
            if (currentPageName != currentPage.name)
            {
                AssetDatabase.RenameAsset(GetPath(), currentPageName);
            }

            CreateStats();

            // Section layout
            GUILayout.EndVertical();
            GUILayout.Space(5);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
        }

        private void CreateStats()
        {
            var labelWidth = 120;
            var style = GetHeaderStyle(GUI.contentColor, 12, TextAnchor.UpperLeft);

            GUILayout.BeginHorizontal();
            GUILayout.Space(5);

            EditorGUILayout.LabelField("Health: ", style, GUILayout.Width(labelWidth));
            currentPage.health = EditorGUILayout.IntField(currentPage.health, GUILayout.Width(45));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Attack Damage: ", style, GUILayout.Width(labelWidth));
            currentPage.attackDamage = EditorGUILayout.IntField(currentPage.attackDamage, GUILayout.Width(45));

            GUILayout.EndHorizontal();
        }

        private void DeleteDataEntry()
        {
            if (GUILayout.Button("Delete Data Entry", GUILayout.MinHeight(22)))
            {
                GUI.FocusControl(null);

                var path = GetPath();
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.DeleteAsset(path + ".meta");

                RefreshAssetDatabase();

                UpdateEnemyList();

                if (enemyList.Length == 0)
                {
                    CreateDataEntry();
                }

                currentPage = enemyList[enemyList.Length - 1];
                pageScrollPosition = Vector2.zero;
            }
        }

        private void CreateDataEntry()
        {
            GUI.FocusControl(null);

            var asset = ScriptableObject.CreateInstance<EnemyData>();
            var itemIndex = System.String.Format("{0:000}", enemyList.Length);

            if (AssetDatabase.FindAssets("DataEntry_" + itemIndex).Length != 0)
            {
                var assetName = enemyList[enemyList.Length - 1].name;

                var characterIndex = 0;
                characterIndex = assetName.IndexOf("_");

                var index = int.Parse(assetName.Substring(characterIndex + 1, 2)) + 1;
                assetName = System.String.Format("{0:000}", index);

                AssetDatabase.CreateAsset(asset, "Assets/Resources/DossierEntries/DataEntry_" + assetName + ".asset");
            }
            else
            {
                AssetDatabase.CreateAsset(asset, "Assets/Resources/DossierEntries/DataEntry_" + itemIndex + ".asset");
            }

            RefreshAssetDatabase();

            pageScrollPosition = Vector2.zero;
        }

        private GUIStyle GetHeaderStyle(Color color, int fontSize = 10, TextAnchor anchor = TextAnchor.MiddleCenter)
        {
            // Header
            GUIStyle style = new GUIStyle();
            style.fontSize = fontSize;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = color;
            style.alignment = anchor;
            style.margin = new RectOffset(right: 5, left: 5, top: 5, bottom: 5);

            return style;
        }

        private void DrawGUILine(int i_height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);
            rect.height = i_height;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }

        private string GetPath()
        {
            return "Assets/Resources/Enemy Info/" + currentPage.name + ".asset";
        }

        private void RefreshAssetDatabase()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}