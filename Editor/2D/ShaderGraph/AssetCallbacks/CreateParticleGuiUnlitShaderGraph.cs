using System;
using UnityEditor.ShaderGraph;
using UnityEngine.Rendering;

namespace UnityEditor.Rendering.Universal.ShaderGraph
{
    static class CreateParticleGuiUnlitShaderGraph
    {
        [MenuItem("Assets/Create/Shader Graph/URP/Particle Gui Unlit Shader Graph", priority = CoreUtils.Sections.section1 + CoreUtils.Priorities.assetsCreateShaderMenuPriority)]
        public static void CreateParticleGuiUnlitGraph()
        {
            var target = (UniversalTarget)Activator.CreateInstance(typeof(UniversalTarget));
            target.TrySetActiveSubTarget(typeof(UniversalParticleGuiUnlitSubTarget));

            var blockDescriptors = new[]
            {
                BlockFields.VertexDescription.Position,
                BlockFields.VertexDescription.Normal,
                BlockFields.VertexDescription.Tangent,
                BlockFields.SurfaceDescription.BaseColor,
                BlockFields.SurfaceDescription.Alpha,
                BlockFieldsTafio.StencilDescription.Stencil
            };

            GraphUtil.CreateNewGraphWithOutputs(new[] { target }, blockDescriptors);
        }


    }
}