using System;
using UnityEngine;

[Serializable]
public class DefaultItems
{
    [HideInInspector] public int placeindex; // ����������� �������������
    public ItemType type;

    public string itemName;  // ��� ���� ����� ����������� �������������
    public int itemAmount;

    // ���������� �������� � ����������� �� ����
    public void UpdateDefaults(int index)
    {
        placeindex = index;  // ������������� placeindex �� 0 �� 29

        switch (type)
        {
            case ItemType.Ammo556:
                itemName = "Ammo556";
                itemAmount = Mathf.Clamp(itemAmount, 1, 100);  // ���������� �� 1 �� 100
                break;

            case ItemType.Ammo918:
                itemName = "Ammo918";
                itemAmount = Mathf.Clamp(itemAmount, 1, 50);  // ���������� �� 1 �� 50
                break;

            case ItemType.MedKit:
                itemName = "MedKit";
                itemAmount = Mathf.Clamp(itemAmount, 1, 6);  // ���������� �� 1 �� 6
                break;

            case ItemType.Clothes:
                itemName = "Hat";  // �� ��������� �������� "Hat"
                itemAmount = 1;  // ��� ������ ������ 1
                break;
        }
    }
}

