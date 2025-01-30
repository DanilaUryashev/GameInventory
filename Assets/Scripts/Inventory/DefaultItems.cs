using System;
using UnityEngine;

[Serializable]
public class DefaultItems
{
    [HideInInspector] public int placeindex; // Назначается автоматически
    public ItemType type;

    public string itemName;  // Это поле будет заполняться автоматически
    public int itemAmount;

    // Обновление значений в зависимости от типа
    public void UpdateDefaults(int index)
    {
        placeindex = index;  // Устанавливаем placeindex от 0 до 29

        switch (type)
        {
            case ItemType.Ammo556:
                itemName = "Ammo556";
                itemAmount = Mathf.Clamp(itemAmount, 1, 100);  // Количество от 1 до 100
                break;

            case ItemType.Ammo918:
                itemName = "Ammo918";
                itemAmount = Mathf.Clamp(itemAmount, 1, 50);  // Количество от 1 до 50
                break;

            case ItemType.MedKit:
                itemName = "MedKit";
                itemAmount = Mathf.Clamp(itemAmount, 1, 6);  // Количество от 1 до 6
                break;

            case ItemType.Clothes:
                itemName = "Hat";  // По умолчанию выбираем "Hat"
                itemAmount = 1;  // Для одежды всегда 1
                break;
        }
    }
}

