﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    /// <summary>
    /// Compare all the fields of a class or struct (Note this derrives from BaseComparer, not TypeComparer)
    /// </summary>
    public class FieldComparer : BaseComparer
    {
        private readonly RootComparer _rootComparer;

        /// <summary>
        /// Constructor with a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public FieldComparer(RootComparer rootComparer)
        {
            _rootComparer = rootComparer;
        }

        /// <summary>
        /// Compare the fields of a class
        /// </summary>
        public void PerformCompareFields(CompareParms parms)
        {
            IEnumerable<FieldInfo> currentFields = null;

            //Interface Member Logic
            if (parms.Config.InterfaceMembers.Count > 0)
            {
                Type[] interfaces = parms.Object1Type.GetInterfaces();

                foreach (var type in parms.Config.InterfaceMembers)
                {
                    if (interfaces.Contains(type))
                    {
                        currentFields = Cache.GetFieldInfo(parms.Config, type);
                        break;
                    }
                }
            }

            if (currentFields == null)
                currentFields = Cache.GetFieldInfo(parms.Config, parms.Object1Type);


            foreach (FieldInfo item in currentFields)
            {
                //Skip if this is a shallow compare
                if (!parms.Config.CompareChildren && TypeHelper.CanHaveChildren(item.FieldType))
                    continue;

                //Skip if it should be excluded based on the configuration
                if (ExcludeLogic.ShouldExcludeMember(parms, item))
                    continue;                

                object objectValue1 = item.GetValue(parms.Object1);
                object objectValue2 = item.GetValue(parms.Object2);

                bool object1IsParent = objectValue1 != null && (objectValue1 == parms.Object1 || parms.Result.Parents.ContainsKey(objectValue1.GetHashCode()));
                bool object2IsParent = objectValue2 != null && (objectValue2 == parms.Object2 || parms.Result.Parents.ContainsKey(objectValue2.GetHashCode()));

                //Skip fields that point to the parent
                if ((TypeHelper.IsClass(item.FieldType) || TypeHelper.IsInterface(item.FieldType))
                    && (object1IsParent || object2IsParent))
                {
                    continue;
                }

                string currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, item.Name);

                CompareParms childParms = new CompareParms
                {
                    Result = parms.Result,
                    Config = parms.Config,
                    ParentObject1 = parms.Object1,
                    ParentObject2 = parms.Object2,
                    Object1 = objectValue1,
                    Object2 = objectValue2,
                    MemberPath = parms.MemberPath + "." + item.Name,
                    BreadCrumb = currentBreadCrumb,
                    ClassDepth = parms.ClassDepth
                };

                _rootComparer.Compare(childParms);

                if (parms.Result.ExceededDifferences)
                    return;
            }
        }


    }
}
