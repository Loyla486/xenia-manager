﻿using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;

// Imported
using Serilog;
using Tomlyn.Model;

namespace XeniaManager.DesktopApp.Pages
{
    public partial class XeniaSettings
    {
        // Functions for loading Settings into the UI
        /// <summary>
        /// Loads the GPU Settings into the UI
        /// </summary>
        /// <param name="sectionTable">Portion of .toml file dedicated to GPU Settings</param>
        private void LoadGpuSettings(TomlTable sectionTable)
        {
            // "clear_memory_page_state" setting
            if (sectionTable.ContainsKey("clear_memory_page_state"))
            {
                Log.Information($"clear_memory_page_state - {(bool)sectionTable["clear_memory_page_state"]}");
                ChkClearGpuCache.IsChecked = (bool)sectionTable["clear_memory_page_state"];
            }

            // "draw_resolution_scale" setting
            if (sectionTable.ContainsKey("draw_resolution_scale_x") &&
                sectionTable.ContainsKey("draw_resolution_scale_y"))
            {
                Log.Information($"draw_resolution_scale_x - {sectionTable["draw_resolution_scale_x"]}");
                SldDrawResolutionScale.Value = int.Parse(sectionTable["draw_resolution_scale_x"].ToString());
                Log.Information($"draw_resolution_scale_y - {sectionTable["draw_resolution_scale_y"]}");
                SldDrawResolutionScale.Value = int.Parse(sectionTable["draw_resolution_scale_y"].ToString());
            }

            // "framerate_limit" setting
            if (sectionTable.ContainsKey("framerate_limit"))
            {
                Log.Information($"framerate_limit - {sectionTable["framerate_limit"]}");
                SldXeniaFramerate.Value = int.Parse(sectionTable["framerate_limit"].ToString());
                AutomationProperties.SetName(SldXeniaFramerate,
                    $"Xenia Framerate Limiter: {SldXeniaFramerate.Value} FPS");
            }

            // "gamma_render_target_as_srgb" setting
            if (sectionTable.ContainsKey("gamma_render_target_as_srgb"))
            {
                Log.Information($"gamma_render_target_as_srgb - {sectionTable["gamma_render_target_as_srgb"]}");
                ChkGammaRenderTargetAsSrgb.IsChecked = (bool)sectionTable["gamma_render_target_as_srgb"];
            }

            // "gpu" setting
            if (sectionTable.ContainsKey("gpu"))
            {
                Log.Information($"gpu - {sectionTable["gpu"] as string}");
                switch (sectionTable["gpu"] as string)
                {
                    case "d3d12":
                        CmbGpuApi.SelectedIndex = 1;
                        break;
                    case "vulkan":
                        CmbGpuApi.SelectedIndex = 2;
                        break;
                    default:
                        CmbGpuApi.SelectedIndex = 0;
                        break;
                }
            }

            // "gpu_allow_invalid_fetch_constants" setting
            if (sectionTable.ContainsKey("gpu_allow_invalid_fetch_constants"))
            {
                Log.Information(
                    $"gpu_allow_invalid_fetch_constants - {sectionTable["gpu_allow_invalid_fetch_constants"]}");
                ChkAllowInvalidFetchConstants.IsChecked = (bool)sectionTable["gpu_allow_invalid_fetch_constants"];
            }

            // "render_target_path_d3d12" setting
            if (sectionTable.ContainsKey("render_target_path_d3d12"))
            {
                Log.Information($"render_target_path_d3d12 - {sectionTable["render_target_path_d3d12"] as string}");
                switch (sectionTable["render_target_path_d3d12"] as string)
                {
                    case "rtv":
                        CmbD3D12RenderTargetPath.SelectedIndex = 1;
                        break;
                    case "rov":
                        CmbD3D12RenderTargetPath.SelectedIndex = 2;
                        break;
                    default:
                        CmbD3D12RenderTargetPath.SelectedIndex = 0;
                        break;
                }
            }

            // "render_target_path_vulkan" setting
            if (sectionTable.ContainsKey("render_target_path_vulkan"))
            {
                Log.Information($"render_target_path_vulkan - {sectionTable["render_target_path_vulkan"] as string}");
                switch (sectionTable["render_target_path_vulkan"] as string)
                {
                    case "fbo":
                        CmbVulkanRenderTargetPath.SelectedIndex = 1;
                        break;
                    case "fsi":
                        CmbVulkanRenderTargetPath.SelectedIndex = 2;
                        break;
                    default:
                        CmbVulkanRenderTargetPath.SelectedIndex = 0;
                        break;
                }
            }

            // "use_fuzzy_alpha_epsilon" setting
            if (sectionTable.ContainsKey("use_fuzzy_alpha_epsilon"))
            {
                Log.Information($"use_fuzzy_alpha_epsilon - {sectionTable["use_fuzzy_alpha_epsilon"]}");
                ChkFuzzyAlphaEpsilon.IsChecked = (bool)sectionTable["use_fuzzy_alpha_epsilon"];
            }

            // "vsync" setting
            if (sectionTable.ContainsKey("vsync"))
            {
                Log.Information($"vsync - {sectionTable["vsync"]}");
                ChkVSync.IsChecked = (bool)sectionTable["vsync"];
            }

            // "query_occlusion_fake_sample_count" setting
            if (sectionTable.ContainsKey("query_occlusion_fake_sample_count"))
            {
                Log.Information(
                    $"query_occlusion_fake_sample_count - {sectionTable["query_occlusion_fake_sample_count"]}");
                TxtQueryOcclusionFakeSampleCount.Text =
                    sectionTable["query_occlusion_fake_sample_count"].ToString() ?? string.Empty;
            }
        }

        /// <summary>
        /// Saves the GPU Settings into the configuration file
        /// </summary>
        /// <param name="sectionTable">Portion of .toml file dedicated to GPU Settings</param>
        private void SaveGpuSettings(TomlTable sectionTable)
        {
            // "clear_memory_page_state" setting
            if (sectionTable.ContainsKey("clear_memory_page_state"))
            {
                Log.Information($"clear_memory_page_state - {ChkClearGpuCache.IsChecked}");
                sectionTable["clear_memory_page_state"] = ChkClearGpuCache.IsChecked;
            }

            // "draw_resolution_scale" setting
            if (sectionTable.ContainsKey("draw_resolution_scale_x") &&
                sectionTable.ContainsKey("draw_resolution_scale_y"))
            {
                Log.Information($"draw_resolution_scale_x - {(int)SldDrawResolutionScale.Value}");
                sectionTable["draw_resolution_scale_x"] = (int)SldDrawResolutionScale.Value;
                Log.Information($"draw_resolution_scale_y - {(int)SldDrawResolutionScale.Value}");
                sectionTable["draw_resolution_scale_y"] = (int)SldDrawResolutionScale.Value;
            }

            // "framerate_limit" setting
            if (sectionTable.ContainsKey("framerate_limit"))
            {
                Log.Information($"framerate_limit - {SldXeniaFramerate.Value}");
                sectionTable["framerate_limit"] = (int)SldXeniaFramerate.Value;
            }

            // "gamma_render_target_as_srgb" setting
            if (sectionTable.ContainsKey("gamma_render_target_as_srgb"))
            {
                Log.Information($"gamma_render_target_as_srgb - {ChkGammaRenderTargetAsSrgb.IsChecked}");
                sectionTable["gamma_render_target_as_srgb"] = ChkGammaRenderTargetAsSrgb.IsChecked;
            }

            // "gpu" setting
            if (sectionTable.ContainsKey("gpu"))
            {
                Log.Information($"gpu - {(CmbGpuApi.SelectedItem as ComboBoxItem).Content}");
                switch (CmbGpuApi.SelectedIndex)
                {
                    case 1:
                        // "d3d12"
                        sectionTable["gpu"] = "d3d12";
                        break;
                    case 2:
                        // "vulkan"
                        sectionTable["gpu"] = "vulkan";
                        break;
                    default:
                        // "any"
                        sectionTable["gpu"] = "any";
                        break;
                }
            }

            // "gpu_allow_invalid_fetch_constants" setting
            if (sectionTable.ContainsKey("gpu_allow_invalid_fetch_constants"))
            {
                Log.Information($"gpu_allow_invalid_fetch_constants - {ChkAllowInvalidFetchConstants.IsChecked}");
                sectionTable["gpu_allow_invalid_fetch_constants"] = ChkAllowInvalidFetchConstants.IsChecked;
            }

            // "render_target_path_d3d12" setting
            if (sectionTable.ContainsKey("render_target_path_d3d12"))
            {
                Log.Information(
                    $"render_target_path_d3d12 - {(CmbD3D12RenderTargetPath.SelectedItem as ComboBoxItem).Content}");
                switch (CmbD3D12RenderTargetPath.SelectedIndex)
                {
                    case 1:
                        // "rtv"
                        sectionTable["render_target_path_d3d12"] = "rtv";
                        break;
                    case 2:
                        // "rov"
                        sectionTable["render_target_path_d3d12"] = "rov";
                        break;
                    default:
                        // "any"
                        sectionTable["render_target_path_d3d12"] = "";
                        break;
                }
            }

            // "render_target_path_vulkan" setting
            if (sectionTable.ContainsKey("render_target_path_vulkan"))
            {
                Log.Information(
                    $"render_target_path_vulkan - {(CmbVulkanRenderTargetPath.SelectedItem as ComboBoxItem).Content}");
                switch (CmbVulkanRenderTargetPath.SelectedIndex)
                {
                    case 1:
                        // "fbo"
                        sectionTable["render_target_path_vulkan"] = "fbo";
                        break;
                    case 2:
                        // "fsi"
                        sectionTable["render_target_path_vulkan"] = "fsi";
                        break;
                    default:
                        // "any"
                        sectionTable["render_target_path_vulkan"] = "";
                        break;
                }
            }

            // "use_fuzzy_alpha_epsilon" setting
            if (sectionTable.ContainsKey("use_fuzzy_alpha_epsilon"))
            {
                Log.Information($"use_fuzzy_alpha_epsilon - {ChkFuzzyAlphaEpsilon.IsChecked}");
                sectionTable["use_fuzzy_alpha_epsilon"] = ChkFuzzyAlphaEpsilon.IsChecked;
            }

            // "vsync" setting
            if (sectionTable.ContainsKey("vsync"))
            {
                Log.Information($"vsync - {ChkVSync.IsChecked}");
                sectionTable["vsync"] = ChkVSync.IsChecked;
            }

            // "query_occlusion_fake_sample_count" setting
            if (sectionTable.ContainsKey("query_occlusion_fake_sample_count"))
            {
                try
                {
                    int queryOcclusionFakeSampleCountInt = int.Parse(TxtQueryOcclusionFakeSampleCount.Text);
                    Log.Information(
                        $"query_occlusion_fake_sample_count - {queryOcclusionFakeSampleCountInt.ToString()}");
                    sectionTable["query_occlusion_fake_sample_count"] = queryOcclusionFakeSampleCountInt;
                }
                catch (Exception ex)
                {
                    // If the input is incorrect, do the default
                    Log.Error(ex.Message + "\nFull Error:\n" + ex);
                    sectionTable["query_occlusion_fake_sample_count"] = 100;
                    TxtQueryOcclusionFakeSampleCount.Text = "100";
                    MessageBox.Show(
                        "Invalid input: query_occlusion_fake_sample_count must be a number.\nSetting the default value of 100.");
                }
            }
        }
    }
}