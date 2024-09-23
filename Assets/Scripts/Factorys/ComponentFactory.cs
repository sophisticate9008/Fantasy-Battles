
using System;
using MyBase;
using MyComponents;
using UnityEngine;

namespace Factorys
{
    public class ComponentFactory
    {
        public static ComponentBase Create(string componentName, GameObject selfObj, params object[] args)
        {

            return componentName switch
            {
                "穿透" => new PenetrableComponent(componentName, "enter", selfObj),
                "反弹" => new ReboundComponent(componentName, "update", selfObj),
                "分裂" => new FissionableComponent(componentName, "enter", selfObj),
                "冰冻" => new FreezeComponent(componentName, "enter", selfObj),

                _ => throw new ArgumentException($"Unknown component: {componentName}")
            };
        }
    }
}
