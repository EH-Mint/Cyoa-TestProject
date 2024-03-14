#if UNITY_EDITOR
using UnityEngine;
using Sirenix.OdinInspector;

using Sirenix.OdinInspector.Editor;

using UnityEditor;
using Sirenix.Utilities.Editor;
using System.Linq;
using Sirenix.Utilities;
using System;

public class Tool : OdinMenuEditorWindow
{
    [UnityEditor.MenuItem("Data/Tool")]
    private static void OpenWindow()
    {
        GetWindow<Tool>().Show();
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;
        tree.Add("열생성",new CreatItemData<Row>());
        tree.Add("선택지생성",new CreatItemData<Choice>());
        tree.Add("대화문생성",new CreatItemData<NPCTalk>());
        tree.AddAssetAtPath("데이터","Assets/Resources/DataManager.asset",typeof(DM));

        tree.AddAllAssetsAtPath("열데이터", "Assets/Data/Row", typeof(Row), true, true);
        tree.AddAllAssetsAtPath("대화문데이터", "Assets/Data/Talk", typeof(NPCTalk), true, true);
        
        tree.AddAllAssetsAtPath("선택지데이터", "Assets/Data/Choice", typeof(Choice), true, true);
        tree.SortMenuItemsByName();
        tree.EnumerateTree().AddIcons<Choice>(x => x.Img);
        tree.AddAllAssetsAtPath("NPC데이터", "Assets/Data/NPC", typeof(NPC), true, true);
        tree.EnumerateTree().AddIcons<NPC>(x => x.Img);
        return tree;
    }
    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selected=this.MenuTree.Selection;
        SirenixEditorGUI.BeginHorizontalToolbar();{
            GUILayout.FlexibleSpace();
            if(SirenixEditorGUI.ToolbarButton("삭제")){
                if(selected.SelectedValue is Row){
                    Resources.Load<DM>("DataManager").RowList.Remove(selected.SelectedValue as Row);
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selected.SelectedValue as Row));
                }
                else if(selected.SelectedValue is NPCTalk){
                    Resources.Load<DM>("DataManager").RowList.Remove(selected.SelectedValue as NPCTalk);
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selected.SelectedValue as NPCTalk));
                }
                else if(selected.SelectedValue is Choice){
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selected.SelectedValue as Choice));
                    Resources.Load<DM>("DataManager").ConnectCYOA();
                }
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }
    public class CreatItemData<T>:OdinSelector<Type> where T : ScriptableObject{
        public CreatItemData(){
            Create();
        }
        [SerializeField,LabelText("이름")]
        private string Key;
        
        [InlineEditor(ObjectFieldMode =InlineEditorObjectFieldModes.Hidden)]
        public T data;
        [Button("생성")]
        private void CreatNewItem2(){
            switch(typeof(T)){
                case var dt when dt==typeof(Row):
                AssetDatabase.CreateAsset(data as Row,"Assets/Data/Row/"+Key+".asset");
                break;
                
                case var dt when dt==typeof(NPCTalk):
                AssetDatabase.CreateAsset(data as NPCTalk,"Assets/Data/Talk/"+Key+".asset");
                break; 

                case var dt when dt==typeof(Choice):
                var p=  data as Choice;
                p.Name=Key;
                AssetDatabase.CreateAsset(p,"Assets/Data/Choice/"+p.C_Group.ToString()+"-"+Key+".asset");
                break;

            }
            AssetDatabase.SaveAssets();
            Create();
        }
        [Button("생성후추가")]
        private void CreatNewItem(){
            switch(typeof(T)){
                case var dt when dt==typeof(Row):
                AssetDatabase.CreateAsset(data as Row,"Assets/Data/Row/"+Key+".asset");
                Resources.Load<DM>("DataManager").RowList.Add(AssetDatabase.LoadAssetAtPath<Row>("Assets/Data/Row/"+Key+".asset"));
                break;
                
                case var dt when dt==typeof(NPCTalk):
                AssetDatabase.CreateAsset(data as NPCTalk,"Assets/Data/Talk/"+Key+".asset");
                Resources.Load<DM>("DataManager").RowList.Add(AssetDatabase.LoadAssetAtPath<NPCTalk>("Assets/Data/Talk/"+Key+".asset"));
                break; 

                case var dt when dt==typeof(Choice):
                var p=  data as Choice;
                p.Name=Key;
                AssetDatabase.CreateAsset(p,"Assets/Data/Choice/"+p.C_Group.ToString()+"-"+Key+".asset");
                break;

            }
            AssetDatabase.SaveAssets();
            Create();
        }
        private void Create(){
            switch(typeof(T)){
                case var dt when dt==typeof(Row):
                data=ScriptableObject.CreateInstance<Row>() as T;
                break;

                case var dt when dt==typeof(NPCTalk):
                data=ScriptableObject.CreateInstance<NPCTalk>()as T;
                break; 

                case var dt when dt==typeof(Choice):
                var d=ScriptableObject.CreateInstance<Choice>() ;
                d.Front=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/GUI/선택지.png");
                data=d as T;
                break;

            }
            Key="new";
        }
        protected override void BuildSelectionTree(OdinMenuTree tree)
        {
            var scriptableObjectTypes = AssemblyUtilities.GetTypes(AssemblyCategory.ProjectSpecific)
                    .Where(x => x.IsClass && !x.IsAbstract && x.InheritsFrom(typeof(T)));

                tree.Selection.SupportsMultiSelect = false;
                tree.Config.DrawSearchToolbar = true;
                tree.AddRange(scriptableObjectTypes, x => x.GetNiceName())
                    .AddThumbnailIcons();
        }

    }
}
#endif