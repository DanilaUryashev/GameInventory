using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DefaultItemsList))]
public class DefaultItemsListEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DefaultItemsList itemsList = (DefaultItemsList)target;

        // Если список предметов пуст, создаём один элемент
        if (itemsList.defaultItems == null || itemsList.defaultItems.Count == 0)
        {
            itemsList.defaultItems.Add(new DefaultItems());  // Добавляем новый пустой элемент
        }

        // Отображаем все предметы в списке
        for (int i = 0; i < itemsList.defaultItems.Count; i++)
        {
            DefaultItems item = itemsList.defaultItems[i];

            // Создаём блок с полями для каждого предмета
            EditorGUILayout.BeginVertical(GUI.skin.box);
            item.type = (ItemType)EditorGUILayout.EnumPopup("Type", item.type);  // Выбираем тип предмета

            // Передаем индекс в UpdateDefaults для установки placeindex
            item.UpdateDefaults(i);  // Обновляем значения на основе типа

            // В зависимости от типа, изменяется поле name
            if (item.type == ItemType.Clothes)
            {
                item.itemName = EditorGUILayout.EnumPopup("Clothes Item", (ClothesType)Enum.Parse(typeof(ClothesType), item.itemName)).ToString();
            }
            else
            {
                item.itemName = EditorGUILayout.TextField("Item Name", item.itemName);
            }

            // Поле для ввода количества предметов
            item.itemAmount = EditorGUILayout.IntField("Amount", item.itemAmount);

            // Выводим кнопки для добавления и удаления предметов
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

        // Вызываем базовый метод для применения изменений
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
