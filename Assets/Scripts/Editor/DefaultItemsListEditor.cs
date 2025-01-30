using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DefaultItemsList))]
public class DefaultItemsListEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DefaultItemsList itemsList = (DefaultItemsList)target;

        // ���� ������ ��������� ����, ������ ���� �������
        if (itemsList.defaultItems == null || itemsList.defaultItems.Count == 0)
        {
            itemsList.defaultItems.Add(new DefaultItems());  // ��������� ����� ������ �������
        }

        // ���������� ��� �������� � ������
        for (int i = 0; i < itemsList.defaultItems.Count; i++)
        {
            DefaultItems item = itemsList.defaultItems[i];

            // ������ ���� � ������ ��� ������� ��������
            EditorGUILayout.BeginVertical(GUI.skin.box);
            item.type = (ItemType)EditorGUILayout.EnumPopup("Type", item.type);  // �������� ��� ��������

            // �������� ������ � UpdateDefaults ��� ��������� placeindex
            item.UpdateDefaults(i);  // ��������� �������� �� ������ ����

            // � ����������� �� ����, ���������� ���� name
            if (item.type == ItemType.Clothes)
            {
                item.itemName = EditorGUILayout.EnumPopup("Clothes Item", (ClothesType)Enum.Parse(typeof(ClothesType), item.itemName)).ToString();
            }
            else
            {
                item.itemName = EditorGUILayout.TextField("Item Name", item.itemName);
            }

            // ���� ��� ����� ���������� ���������
            item.itemAmount = EditorGUILayout.IntField("Amount", item.itemAmount);

            // ������� ������ ��� ���������� � �������� ���������
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Remove Item"))
            {
                itemsList.defaultItems.RemoveAt(i);
                break;
            }
            if (GUILayout.Button("Add Item"))
            {
                itemsList.defaultItems.Add(new DefaultItems());
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        // �������� ������� ����� ��� ���������� ���������
        DrawDefaultInspector();
    }
}
public enum ClothesType
{
    Hat,
    Helmet,
    Jacket,
    Armor
}
