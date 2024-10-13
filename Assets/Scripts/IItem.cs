using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    void Use(GameObject target);//아이템을 사용하는 대상을 매개변수로 받음
}
