// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Internal.NativeFormat;

namespace Internal.TypeSystem
{
    public abstract partial class SignatureVariable : TypeDesc
    {
        private TypeSystemContext _context;
        private int _index;

        internal SignatureVariable(TypeSystemContext context, int index)
        {
            _context = context;
            _index = index;
        }

        public int Index
        {
            get
            {
                return _index;
            }
        }

        public override TypeSystemContext Context
        {
            get
            {
                return _context;
            }
        }

        public abstract bool IsMethodSignatureVariable
        {
            get;
        }
    }

    public sealed partial class SignatureTypeVariable : SignatureVariable
    {
        internal SignatureTypeVariable(TypeSystemContext context, int index) : base(context, index)
        {
        }

        public override bool IsMethodSignatureVariable
        {
            get
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return TypeHashingAlgorithms.ComputeSignatureVariableHashCode(Index, false);
        }

        protected override TypeFlags ComputeTypeFlags(TypeFlags mask)
        {
            // We might be able to compute the type flags for some masks, but for some this will always throw (e.g.
            // Category and GenericVariance should always throw). If you hit an exception, it means you need a
            // special case for IsSignatureVariable.
            throw new NotImplementedException();
        }

        public override TypeDesc InstantiateSignature(Instantiation typeInstantiation, Instantiation methodInstantiation)
        {
            return typeInstantiation.IsNull ? this : typeInstantiation[Index];
        }

        public override string ToString()
        {
            return "!" + Index.ToString();
        }
    }

    public sealed partial class SignatureMethodVariable : SignatureVariable
    {
        internal SignatureMethodVariable(TypeSystemContext context, int index) : base(context, index)
        {
        }

        public override bool IsMethodSignatureVariable
        {
            get
            {
                return true;
            }
        }

        public override int GetHashCode()
        {
            return TypeHashingAlgorithms.ComputeSignatureVariableHashCode(Index, true);
        }

        protected override TypeFlags ComputeTypeFlags(TypeFlags mask)
        {
            // We might be able to compute the type flags for some masks, but for some this will always throw (e.g.
            // Category and GenericVariance should always throw). If you hit an exception, it means you need a
            // special case for IsSignatureVariable.
            throw new NotImplementedException();
        }

        public override TypeDesc InstantiateSignature(Instantiation typeInstantiation, Instantiation methodInstantiation)
        {
            return methodInstantiation.IsNull ? this : methodInstantiation[Index];
        }

        public override string ToString()
        {
            return "!!" + Index.ToString();
        }
    }
}
