// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Debug = System.Diagnostics.Debug;

namespace Internal.TypeSystem
{
    internal static class RuntimeDeterminedTypeUtilities
    {
        public static Instantiation ConvertInstantiationToSharedRuntimeForm(Instantiation instantiation, Instantiation openInstantiation, out bool changed)
        {
            Debug.Assert(instantiation.Length == openInstantiation.Length);

            TypeDesc[] sharedInstantiation = null;

            CanonicalFormKind currentPolicy = CanonicalFormKind.Specific;
            CanonicalFormKind startLoopPolicy;

            do
            {
                startLoopPolicy = currentPolicy;

                for (int instantiationIndex = 0; instantiationIndex < instantiation.Length; instantiationIndex++)
                {
                    TypeDesc typeToConvert = instantiation[instantiationIndex];
                    TypeDesc canonForm = RuntimeDeterminedCanonicalizationAlgorithm.ConvertToCanon(typeToConvert, ref currentPolicy);
                    TypeDesc runtimeDeterminedForm = typeToConvert;

                    Debug.Assert(canonForm is DefType);
                    Debug.Assert(openInstantiation[instantiationIndex] is GenericParameterDesc);

                    if ((typeToConvert != canonForm) || typeToConvert.IsCanonicalType)
                    {
                        if (sharedInstantiation == null)
                        {
                            sharedInstantiation = new TypeDesc[instantiation.Length];
                            for (int i = 0; i < instantiationIndex; i++)
                                sharedInstantiation[i] = instantiation[i];
                        }

                        TypeSystemContext context = typeToConvert.Context;
                        runtimeDeterminedForm = context.GetRuntimeDeterminedType(
                            (DefType)canonForm, (GenericParameterDesc)openInstantiation[instantiationIndex]);
                    }

                    if (sharedInstantiation != null)
                    {
                        sharedInstantiation[instantiationIndex] = runtimeDeterminedForm;
                    }
                }

                // Optimization: even if canonical policy changed, we don't actually need to re-run the loop
                // for instantiations that only have a single element.
                if (instantiation.Length == 1)
                {
                    break;
                }

            } while (currentPolicy != startLoopPolicy);

            changed = sharedInstantiation != null;
            if (changed)
            {
                return new Instantiation(sharedInstantiation);
            }

            return instantiation;
        }
    }
}
