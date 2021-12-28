#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;

namespace Quixel
{
    public class MegascansMaterialUtils : MonoBehaviour
    {
        public static Material CreateMaterial(int shaderType, string matPath, bool isAlembic, int dispType, int texPack)
        {
            if ((shaderType == 0 || shaderType == 1) && isAlembic)
            {
                Debug.Log("Alembic files are not supported in LWRP/HDRP. Please change your export file format in Bridge or change your SRP in Unity.");
                return null;
            }

            try
            {
                string rp = matPath + ".mat";
                Material mat = (Material)AssetDatabase.LoadAssetAtPath(rp, typeof(Material));
                if (!mat)
                {
                    mat = new Material(Shader.Find("Standard"));
                    AssetDatabase.CreateAsset(mat, rp);
                    AssetDatabase.Refresh();
                    if (shaderType < 1)
                    {
                        mat.shader = Shader.Find("HDRenderPipeline/Lit");
#if UNITY_2018_3 || UNITY_2018_4 || UNITY_2019 || UNITY_2020
                        mat.shader = Shader.Find("HDRP/Lit");
#endif
                        mat.SetInt("_DisplacementMode", dispType);
                    }
                    if (shaderType > 0)
                    {
#if UNITY_2019_3 || UNITY_2019_4 || UNITY_2020
                        mat.shader = Shader.Find("Universal Render Pipeline/Lit");
#else
                        if (MegascansUtilities.isLegacy())
                            mat.shader = Shader.Find("LightweightPipeline/Standard (Physically Based)");
                        else
                            mat.shader = Shader.Find("Lightweight Render Pipeline/Lit");
#endif
                    }
                    if (shaderType > 1)
                    {
                        if (isAlembic)
                        {
                            mat.shader = Shader.Find("Alembic/Standard");
                            if (texPack > 0)
                                mat.shader = Shader.Find("Alembic/Standard (Specular setup)");
                        }
                        else
                        {
                            mat.shader = Shader.Find("Standard");
                            if (texPack > 0)
                                mat.shader = Shader.Find("Standard (Specular setup)");
                        }
                    }
                }
                return mat;
            }
            catch (Exception ex)
            {
                Debug.Log("MegascansMaterialUtils::CreateMaterial::Exception: " + ex.ToString());
                MegascansUtilities.HideProgressBar();
                return null;
            }
        }

        public static void AddSSSSettings(Material mat, int shaderType)
        {
            mat.SetInt("_MaterialID", 0);
            mat.EnableKeyword("_MATERIAL_FEATURE_SUBSURFACE_SCATTERING");
            mat.EnableKeyword("_MATERIAL_FEATURE_TRANSMISSION");

            if (shaderType == 0)
            {
                mat.SetShaderPassEnabled("DistortionVectors", false);
                mat.SetShaderPassEnabled("TransparentDepthPrepass", false);
                mat.SetShaderPassEnabled("TransparentDepthPostpass", false);
                mat.SetShaderPassEnabled("TransparentBackface", false);
                mat.SetShaderPassEnabled("MOTIONVECTORS", false);

                mat.SetColor("_EmissionColor", Color.white);

                mat.SetFloat("_DistortionBlurDstBlend", 1f);
                mat.SetFloat("_DistortionBlurSrcBlend", 1f);
                mat.SetFloat("_DistortionDstBlend", 1f);
                mat.SetFloat("_DistortionSrcBlend", 1f);
                mat.SetFloat("_StencilRef", 4f);
                mat.SetFloat("_StencilRefDepth", 8f);
                mat.SetFloat("_StencilRefGBuffer", 14f);
                mat.SetFloat("_StencilRefMV", 40f);
                mat.SetFloat("_StencilWriteMask", 6f);
                mat.SetFloat("_StencilWriteMaskGBuffer", 14f);
                mat.SetFloat("_StencilWriteMaskMV", 40f);
                mat.SetFloat("_ZTestDepthEqualForOpaque", 3f);
                mat.SetFloat("_ZTestModeDistortion", 4f);
            }
        }
    }
}
#endif