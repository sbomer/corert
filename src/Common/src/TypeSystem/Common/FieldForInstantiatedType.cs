// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Internal.TypeSystem
{
    public sealed class FieldForInstantiatedType : FieldDesc
    {
        private FieldDesc _fieldDef;
        private InstantiatedType _instantiatedType;

        internal FieldForInstantiatedType(FieldDesc fieldDef, InstantiatedType instantiatedType)
        {
            _fieldDef = fieldDef;
            _instantiatedType = instantiatedType;
        }

        public override TypeSystemContext Context
        {
            get
            {
                return _fieldDef.Context;
            }
        }

        public override DefType OwningType
        {
            get
            {
                return _instantiatedType;
            }
        }

        public override string Name
        {
            get
            {
                return _fieldDef.Name;
            }
        }

        public override TypeDesc FieldType
        {
            get
            {
                return _fieldDef.FieldType.InstantiateSignature(_instantiatedType.Instantiation, new Instantiation());
            }
        }

        public override bool IsStatic
        {
            get
            {
                return _fieldDef.IsStatic;
            }
        }

        public override bool IsInitOnly
        {
            get
            {
                return _fieldDef.IsInitOnly;
            }
        }

        public override bool IsThreadStatic
        {
            get
            {
                return _fieldDef.IsThreadStatic;
            }
        }

        public override bool HasRva
        {
            get
            {
                return _fieldDef.HasRva;
            }
        }

        public override bool IsLiteral
        {
            get
            {
                return _fieldDef.IsLiteral;
            }
        }

        public override bool HasCustomAttribute(string attributeNamespace, string attributeName)
        {
            return _fieldDef.HasCustomAttribute(attributeNamespace, attributeName);
        }

        public override FieldDesc GetTypicalFieldDefinition()
        {
            return _fieldDef;
        }
    }
}
